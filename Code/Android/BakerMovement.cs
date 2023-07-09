using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakerMovement : MonoBehaviour
{
    public static BakerMovement SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts for the same owner.
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public List<GameObject> pooledbakers; //Declare a new list to store the bakers
    public List<GameObject> pooledbakercollisionbounds; //Declare a new list to store the baker collision bounds
    public int poolamount = 1; //Declare a variable to store the maximum number of copies to allow in the pool
    public int poolindex = 0; //Declare a variable to store an index for keeping track of all bakers in the scene

    Rigidbody2D baker; //Declare the rigidbody of the baker
    public Animator bakeranimator; //Declare the animator component for the baker
    Rigidbody2D player; //Declare the rigidbody of the player
    public float bakermargin; //Declare a variable that will hold the player's inscribed circle radius for handling out-of-camera-bound cases
    public GameObject bakerobj; //Declare a new GameObject which will be used to find and store the baker
    public GameObject bakercollisionboundobj; //Declare a new GameObject which will be used to find and store the baker collision bound
    public GameObject[] bakercopy; //Declare a new GameObject which will be used to store baker copies
    public GameObject[] bakercollisionboundcopy; //Declare a new GameObject which will be used to store baker collision bound copies
    public GameObject mainground; //Declare a new GameObject which will be used to find and store the main ground object
    public float spawninitial = 40f; //Declare a variable to store the initial spawn interval
    public float spawnconsecutiveinitial = 20f; //Declare a variable to store the initial consecutive spawn interval
    public float spawnconsecutive; //Declare a variable to store the interval between consecutive spawn events
    public float chaseinterval = 5f; //Declare a variable to store the interval between baker chase events
    public bool isspawnright = true; //Declare a variable to store the direction from which the baker will spawn
    public float playermargin; //Declare a variable to represent a margin to be used for calculating collisions with the player
    public float groundmargin; //Declare a variable to represent a margin to be used for spawning baker instances
    public bool isbakerdead = false; //Declare a variable to keep track of whether the baker instance has been destroyed or not

    public float difficultymultiplier = 1f; //Declare a variable to decrease the spawn interval with difficulty. Initialize it at 1.

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource bakercuesound; //Declare a new sound source
    public AudioSource jumpsound; //Declare a new sound source
    public AudioSource collisionsound; //Declare a new sound source
    public AudioSource burpsound; //Declare a new sound source

    // Awake allows declaration or initialization of variables before the program runs. 
    void Awake()
    {
        SharedInstance = this; //This is the shared instance. Used if you want to access variables between scripts for the same owner.
    }


    // Start is called before the first frame update
    void Start()
    {
        //Declare rigid bodies in case they need to be used
        player = gameObject.GetComponent<Rigidbody2D>();
        baker = bakerobj.GetComponent<Rigidbody2D>();

        playermargin = (gameObject.transform.localScale.x / 2) + (bakerobj.transform.localScale.x / 2) + 0.05f; //Set the burger-player collision calculation margin to the sum of half the width of each of the respective square sprites, as well as a safety margin
        bakermargin = (bakerobj.transform.localScale.x) / 2; //Initialize the variable that will hold the player's inscribed circle radius for handling out-of-camera-bound cases
        groundmargin = mainground.transform.position.y + (mainground.transform.localScale.y / 2) + (bakerobj.transform.localScale.y / 2) + 0.05f; //Set a ground margin for pizza spawning to the sum of the main ground block's y-position, half the height of the main ground block, half the height of the square baker sprite, and a safety margin

        bakercopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated bakers present in the scene
        bakercollisionboundcopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated baker collision bounds present in the scene

        //Initialize pooling by populating the GameObject list
        pooledbakers = new List<GameObject>(); //Initialize list to store the pooled bakers
        GameObject tmp; //Temporary variable to hold an instance of the baker
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(bakerobj);
            tmp.SetActive(false);
            pooledbakers.Add(tmp); //Add bakers copies to the list of pooled objects
        }

        //Initialize pooling by populating the GameObject list
        pooledbakercollisionbounds = new List<GameObject>(); //Initialize list to store the pooled bakercollisionbounds
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(bakercollisionboundobj);
            tmp.SetActive(false);
            pooledbakercollisionbounds.Add(tmp); //Add bakercollisionbound copies to the list of pooled objects
        }

        //Initialize sound
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences
        bakercuesound.volume = soundeffectsvolume; //Set the volume of the sound source
        jumpsound.volume = soundeffectsvolume; //Set the volume of the sound source
        collisionsound.volume = soundeffectsvolume; //Set the volume of the sound source
        burpsound.volume = soundeffectsvolume; //Set the volume of the sound source

        spawnconsecutive = spawnconsecutiveinitial; //Set the consecutive spawn interval to its initial value
        InvokeRepeating("DelayAction", spawninitial, spawnconsecutive); //Repeat the function 'DelayAction' after initial time 'spawninterval', and every 'spawninterval' thereafter
        InvokeRepeating("BakerChase", chaseinterval, chaseinterval); //Repeat the function 'BakerChase' after initial time 'chaseinterval' and every 'chaseinterval' thereafter
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation(); //Call function to handle the baker's animations
        HandleDifficulty(); //Update the difficulty based on the time since the scene last loaded
        HandleBounding(); //Wrap the baker around the screen if they are out of the camera-width bounds
        CheckPlayerCollision(); //Check if the baker has collided with the player

    }

    // After every spawninterval
    private void DelayAction()
    {
        if (bakercopy[poolindex] is null) //If there are no bakers in the scene
        {
            //If the index that keeps track of all the bakers in the scene goes out of bounds
            if (poolindex >= poolamount)
            {
                poolindex = 0; //Reset the index that keeps track of all the bakers in the scene
            }

            //Spawn a new baker instance
            bakercopy[poolindex] = BakerMovement.SharedInstance.GetPooledBaker(); //Get pooled objects
            bakercollisionboundcopy[poolindex] = BakerMovement.SharedInstance.GetPooledBakerCollisionBound(); //Get pooled baker collision bounds
            //Define a random spawn point on either side of the camera width, but near ground level
            float yloc = groundmargin;
            float xloc = 0f;
            System.Random rand = new System.Random(); //Declare a new instance of Random
            isspawnright = rand.Next(2) == 1; //Set the boolean variable 'isspawnright' to a random value
            if (isspawnright is false)
            {
                xloc = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 1.5f; //Set the x-location to a value 1.5f to the left of the camera's left-most bound
            }
            else if (isspawnright is true)
            {
                xloc = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1.5f; //Set the x-location to a value 1.5f to the right of the camera's right-most bound
            }

            Vector2 spawnpos = new Vector2(xloc, yloc);
            //Set baker spawn position
            bakercopy[poolindex].transform.position = spawnpos; //Set the baker copy position to that of the spawn point
            bakercollisionboundcopy[poolindex].transform.position = spawnpos; //Set the baker collision bound copy position to that of the spawn point
            bakercopy[poolindex].SetActive(true); //Spawn the baker
            bakercollisionboundcopy[poolindex].SetActive(true); //Spawn the baker collision bound

            bakeranimator = bakercopy[poolindex].GetComponent<Animator>(); //Bind animator component of the owner to the specified variable

            bakercollisionboundcopy[poolindex].transform.parent = bakercopy[poolindex].transform; //Set the object instance as the parent of the collision bound instance

            //Re-enable physics. This has to be done explicitly since we disable physics for each instance when we destroy it to allow time for the animation.
            bakercopy[poolindex].GetComponent<Rigidbody2D>().simulated = true; //Enable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
            bakercopy[poolindex].GetComponent<Rigidbody2D>().WakeUp(); //Enable rigidbody of the instance
            //Setup the collider of the instance's collision bound
            bakercollisionboundcopy[poolindex].GetComponent<Collider2D>().isTrigger = true;
            bakercollisionboundcopy[poolindex].tag = "bakercollision";

            isbakerdead = false;

            bakercuesound.Play(); //Play the pizza spawn sound

            //poolindex = poolindex + 1; //Increment the index that keeps track of all the bakers in the scene

            //Update spawnconsecutive to tweak the consecutive spawn time based on difficulty
            float spawnconsecutivelower = spawnconsecutiveinitial / difficultymultiplier; //Calculate the spawnconsecutive lower bound every frame based on the time that has elapsed
            float spawnconsecutiveupper = (spawnconsecutiveinitial * 2f) / difficultymultiplier; //Calculate the spawnconsecutive upper bound every frame based on the time that has elapsed
            System.Random randdub = new System.Random(); //Declare a new instance of Random
            spawnconsecutive = ((float)randdub.NextDouble() * (spawnconsecutiveupper - spawnconsecutivelower)) + spawnconsecutivelower; //Update the spawn consecutive to cater for the difficulty. y = mx + c
            InvokeRepeating("DelayAction", spawnconsecutive, spawnconsecutive); //We have to call InvokeRepeating again to cater for the fact that the parameter spawnconsecutive is updated constantly.

        }

    }

    // After every chaseinterval
    private void BakerChase()
    {
        //Set baker motion
        if ((bakercopy[poolindex] != null) && (isbakerdead is false) && (bakercopy[poolindex].transform.position.y <= groundmargin)) //If there is a baker in the scene and they are touching the ground
        {
            jumpsound.Play(); //Play the jump sound
            float bakerplayerseparation = player.transform.position.x - bakercopy[poolindex].transform.position.x; //Get the vector distance between the baker and the player
            float bakerplayertime = 2f; //Set the time the baker takes to reach the player's position
            bakercopy[poolindex].GetComponent<Rigidbody2D>().velocity = new Vector2((bakerplayerseparation/bakerplayertime), 10f); //Set the baker's velocity to follow the player while jumping as well
        }

        
    }

    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBaker()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledbakers[i].activeInHierarchy)
            {
                return pooledbakers[i];
            }
        }
        return null;
    }


    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBakerCollisionBound()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledbakercollisionbounds[i].activeInHierarchy)
            {
                return pooledbakercollisionbounds[i];
            }
        }
        return null;
    }


    void HandleBounding()
    {
        if ((bakercopy[poolindex] != null) && (isbakerdead is false)) //If there is a baker in the scene
        {
            if (bakercopy[poolindex].transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - bakermargin) //If the baker goes past the left-most camera bound
            {
                float xloc = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + bakermargin - 0.1f; //Set the respawn x-location off-screen to the right, offset by a safety margin to prevent pinballing between camera-bound respawn positions
                float yloc = bakercopy[poolindex].transform.position.y; //Maintain y-value of the baker
                Vector2 spawnpos = new Vector2(xloc, yloc);
                bakercopy[poolindex].transform.position = spawnpos;
            }
            else if (bakercopy[poolindex].transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + bakermargin) //If the baker goes past the right-most camera bound
            {
                float xloc = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - bakermargin + 0.1f; //Set the respawn x-location off-screen to the left, offset by a safety margin to prevent pinballing between camera-bound respawn positions
                float yloc = bakercopy[poolindex].transform.position.y; //Maintain y-value of the baker
                Vector2 spawnpos = new Vector2(xloc, yloc);
                bakercopy[poolindex].transform.position = spawnpos;

            }
        }

    }

    void CheckPlayerCollision()
    {
        //Manage collisions. No 'for' loop is required since we will only deal with one baker on the screen at a time.
        //If the baker array slot is not empty AND the baker is near enough to the player to constitute a collision
        if ((bakercopy[poolindex] != null) && (isbakerdead is false) && (charactermovementscript.livesleft > 0) && (charactermovementscript.isdiving is false) && (bakercopy[poolindex].transform.position.x <= player.transform.position.x + playermargin) && (bakercopy[poolindex].transform.position.x >= player.transform.position.x - playermargin) && (bakercopy[poolindex].transform.position.y <= player.transform.position.y + playermargin) && (bakercopy[poolindex].transform.position.y >= player.transform.position.y - playermargin))
        {
            charactermovementscript.livesleft = charactermovementscript.livesleft - 1; //Decrement player's lives left
            charactermovementscript.canshoot = false; //Deactivate the player's ability to throw dumbbells if they are hit

        }
        //else if ((bakercopy[poolindex] != null) && (charactermovementscript.livesleft > 0) && (charactermovementscript.isdiving is true) && (bakercopy[poolindex].transform.position.x <= player.transform.position.x + playermargin) && (bakercopy[poolindex].transform.position.x >= player.transform.position.x - playermargin) && (bakercopy[poolindex].transform.position.y <= player.transform.position.y + playermargin))
        //{
            //bakercopy[poolindex].SetActive(false); //Remove instance
            //bakercopy[poolindex] = null; //Clear the array slot
        //}

    }

    void HandleDifficulty()
    {
        //Set the difficulty multiplier based on the time since the scene loaded
        if (Time.timeSinceLevelLoad <= 50f)
        {
            difficultymultiplier = 1f;
        }
        else if ((Time.timeSinceLevelLoad > 50f) && (Time.timeSinceLevelLoad <= 100f))
        {
            difficultymultiplier = 1.25f;
        }
        else if ((Time.timeSinceLevelLoad > 100f) && (Time.timeSinceLevelLoad <= 150f))
        {
            difficultymultiplier = 1.5f;
        }
        else if (Time.timeSinceLevelLoad > 150f)
        {
            difficultymultiplier = 2f;
        }
    }

    void HandleAnimation()
    {
        if ((bakercopy[poolindex] != null) && (isbakerdead is false)) //If there is a baker in the scene
        {
            if ((bakercopy[poolindex].transform.position.y > groundmargin) && (bakercopy[poolindex].transform.position.x <= player.transform.position.x)) //If the baker is in the air to the left of the player
            {
                bakeranimator.Play("baker_jump_right"); //Play the baker jump right animation
            }
            else if ((bakercopy[poolindex].transform.position.y > groundmargin) && (bakercopy[poolindex].transform.position.x > player.transform.position.x)) //If the baker is in the air to the right of the player
            {
                bakeranimator.Play("baker_jump_left"); //Play the baker jump left animation
            }
            else if ((bakercopy[poolindex].transform.position.y <= groundmargin) && (bakercopy[poolindex].transform.position.x <= player.transform.position.x)) //If the baker is on the ground to the left of the player
            {
                bakeranimator.Play("baker_idle_right"); //Play the baker idle right animation
            }
            else if ((bakercopy[poolindex].transform.position.y <= groundmargin) && (bakercopy[poolindex].transform.position.x > player.transform.position.x)) //If the baker is on the ground to the right of the player
            {
                bakeranimator.Play("baker_idle_left"); //Play the baker idle left animation
            }

        }
    }


    //The 'OnTriggerEnter2D' function is a preset Unity function that will be called automatically when the owner of this script's collider makes contact with another collider.
    //For the function to work, the owner of the script must have a RigidBody2D, and a Collider2D. The other object must have otherobject.GetComponent<Collider2D>().isTrigger = true
    //The other object's tag must also already exist in the tag library, even if you assign it to randomly generated objects at runtime.
    //To facilitate player collision and ground collision, this function must exist in a script attached to the player and a separate one attached to the ground.
    //This function does not need to be called.
    void OnTriggerEnter2D(Collider2D bakercollider)
    {
        if ((bakercollider.tag == "bakercollision") && (isbakerdead is false) && (charactermovementscript.livesleft > 0))
        {
            if (bakercollisionboundcopy[poolindex] == bakercollider.gameObject)
            {
                if (charactermovementscript.livesleft > 1)
                {
                    burpsound.Play(); //If the hazard collides with the player, but they don't die immediately after, make them burp
                }
                charactermovementscript.livesleft = charactermovementscript.livesleft - 1; //Decrement player's lives left
                charactermovementscript.canshoot = false; //Deactivate the player's ability to throw dumbbells if they are hit

            }

        }

    }

    public void PlayBakerDeath()
    {
        collisionsound.Play(); //Play the collision sound
        isbakerdead = true; //The baker has been vanquished
        bakeranimator.Play("baker_death"); //Play the baker death animation
        
    }

    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }


}
