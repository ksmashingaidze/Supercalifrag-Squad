using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public static BirdMovement SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts for the same owner.
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public BakerMovement bakermovementscript; //Reference the baker movement script
    public List<GameObject> pooledbirds; //Declare a new list to store the birds
    public List<GameObject> pooledbirdcollisionbounds; //Declare a new list to store the bird collision bounds
    public int poolamount = 1; //Declare a variable to store the maximum number of copies to allow in the pool
    public int poolindex = 0; //Declare a variable to store an index for keeping track of all birds in the scene

    Rigidbody2D bird; //Declare the rigidbody of the bird
    public Animator birdanimator; //Declare the animator component for the bird
    Rigidbody2D player; //Declare the rigidbody of the player
    Rigidbody2D baker; //Declare the rigidbody of the baker
    public GameObject birdobj; //Declare a new GameObject which will be used to find and store the bird
    public GameObject birdcollisionboundobj; //Declare a new GameObject which will be used to find and store the bird collision bound
    public GameObject bakerobj; //Declare a new GameObject which will be used to find and store the baker
    public GameObject[] birdcopy; //Declare a new GameObject which will be used to store bird copies
    public GameObject[] birdcollisionboundcopy; //Declare a new GameObject which will be used to store bird collision bound copies
    public float spawninitial = 15f; //Declare a variable to store the initial spawn interval
    public float spawnconsecutiveinitial = 15f; //Declare a variable to store the initial consecutive spawn interval
    public float spawnconsecutive; //Declare a variable to store the interval between consecutive spawn events
    public bool isspawnright = true; //Declare a variable to store the direction from which the bird will spawn
    public float birdspeed = 7f; //Declare a variable that will hold the bird's movement speed
    public float playerinscribedradius; //Declare a variable to get the radius of the inscribed circle of the player's square sprite. For use as an approximation in collision calculations without using the sensor.
    public float bakerinscribedradius; //Declare a variable to get the radius of the inscribed circle of the baker's square sprite. For use as an approximation in collision calculations without using the sensor.
    public float birdinscribedradius; //Declare a variable to get the radius of the bird's circular sprite

    public float difficultymultiplier = 1f; //Declare a variable to decrease the spawn interval with difficulty. Initialize it at 1.
    public bool isbirddead = false; //Declare a variable to keep track of whether the bird instance has been destroyed or not

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource birdcuesound; //Declare a new sound source
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
        bird = birdobj.GetComponent<Rigidbody2D>();
        baker = bakerobj.GetComponent<Rigidbody2D>();
        
        playerinscribedradius = (gameObject.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        bakerinscribedradius = (bakerobj.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        birdinscribedradius = ((birdobj.transform.localScale.x) / 2) + 0.05f; //Set the variable storing the radius of the bird to half the width of the square sprite, plus a safety margin

        birdcopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated birds present in the scene
        birdcollisionboundcopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated bird collision bounds present in the scene

        //Initialize pooling by populating the GameObject list
        pooledbirds = new List<GameObject>(); //Initialize list to store the pooled birds
        GameObject tmp; //Temporary variable to hold an instance of the bird
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(birdobj);
            tmp.SetActive(false);
            pooledbirds.Add(tmp); //Add birds copies to the list of pooled objects
        }

        //Initialize pooling by populating the GameObject list
        pooledbirdcollisionbounds = new List<GameObject>(); //Initialize list to store the pooled birdcollisionbounds
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(birdcollisionboundobj);
            tmp.SetActive(false);
            pooledbirdcollisionbounds.Add(tmp); //Add birdcollisionbound copies to the list of pooled objects
        }

        //Initialize sound
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences
        birdcuesound.volume = soundeffectsvolume; //Set the volume of the sound source
        collisionsound.volume = soundeffectsvolume; //Set the volume of the sound source
        burpsound.volume = soundeffectsvolume; //Set the volume of the sound source

        spawnconsecutive = spawnconsecutiveinitial; //Set the consecutive spawn interval to its initial value
        InvokeRepeating("DelayAction", spawninitial, spawnconsecutive); //Repeat the function 'DelayAction' after initial time 'spawninterval', and every 'spawninterval' thereafter
    }

    // Update is called once per frame
    void Update()
    {
        //Set the difficulty multiplier based on the time since the scene loaded
        if (Time.timeSinceLevelLoad <= 20f)
        {
            difficultymultiplier = 1f;
        }
        else if ((Time.timeSinceLevelLoad > 20f) && (Time.timeSinceLevelLoad <= 60f))
        {
            difficultymultiplier = 1.25f;
        }
        else if ((Time.timeSinceLevelLoad > 60f) && (Time.timeSinceLevelLoad <= 120f))
        {
            difficultymultiplier = 1.5f;
        }
        else if (Time.timeSinceLevelLoad > 120f)
        {
            difficultymultiplier = 2f;
        }

        
        //If the bird array slot is not empty AND the bird goes out of bounds
        if ((birdcopy[poolindex] != null) && (isspawnright is true) && (birdcopy[poolindex].transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - 1.5f)) //If the bird instance spawned on the right and the bird goes past the left-most bound of the camera
        {
            birdcopy[poolindex].SetActive(false); //Remove instance
            birdcopy[poolindex] = null; //Clear the array slot
        }
        else if ((birdcopy[poolindex] != null) && (isspawnright is false) && (birdcopy[poolindex].transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1.5f)) //If the bird instance spawned on the left and the bird goes past the right-most bound of the camera
        {
            birdcopy[poolindex].SetActive(false); //Remove instance
            birdcopy[poolindex] = null; //Clear the array slot
        }

        //Set bird motion, based on the side of the screen it spawned on
        if ((birdcopy[poolindex] != null) && (isbirddead is false) && isspawnright is false)
        {
            birdcopy[poolindex].GetComponent<Rigidbody2D>().velocity = new Vector2(birdspeed, 0.1f); //The bird spawned on the left side of the screen, so its initial velocity must be set as positive
            birdanimator.Play("bird_right"); //Play the bird fly right animation
        }
        else if ((birdcopy[poolindex] != null) && (isbirddead is false) && isspawnright is true)
        {
            birdcopy[poolindex].GetComponent<Rigidbody2D>().velocity = new Vector2(-birdspeed, 0.1f); //The bird spawned on the right side of the screen, so its initial velocity must be set as negative
            birdanimator.Play("bird_left"); //Play the bird fly left animation
        }


    }

    // After every spawninterval
    private void DelayAction()
    {
        if (birdcopy[poolindex] is null) //If there are no birds in the scene
        {
            //If the index that keeps track of all the birds in the scene goes out of bounds
            if (poolindex >= poolamount)
            {
                poolindex = 0; //Reset the index that keeps track of all the birds in the scene
            }

            //Spawn a new bird instance
            birdcopy[poolindex] = BirdMovement.SharedInstance.GetPooledBird(); //Get pooled objects
            birdcollisionboundcopy[poolindex] = BirdMovement.SharedInstance.GetPooledBirdCollisionBound(); //Get pooled bird collision bounds
            //Define a random spawn point on either side of the camera width, but near ground level
            float yloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height - (Screen.height / 3))).y, Camera.main.ScreenToWorldPoint(new Vector2(0, (Screen.height))).y); //Set y-location for spawning to a random value between the height of the camera and half the height of the camera
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
            //Set bird spawn position
            birdcopy[poolindex].transform.position = spawnpos; //Set the bird copy position to that of the random spawn point
            birdcollisionboundcopy[poolindex].transform.position = spawnpos; //Set the bird collision bound copy position to that of the random spawn point

            birdcopy[poolindex].SetActive(true); //Spawn the bird
            birdcollisionboundcopy[poolindex].SetActive(true); //Spawn the bird collision bound

            birdanimator = birdcopy[poolindex].GetComponent<Animator>(); //Bind animator component of the owner to the specified variable

            birdcollisionboundcopy[poolindex].transform.parent = birdcopy[poolindex].transform; //Set the object instance as the parent of the collision bound instance

            //Re-enable physics. This has to be done explicitly since we disable physics for each instance when we destroy it to allow time for the animation.
            birdcopy[poolindex].GetComponent<Rigidbody2D>().simulated = true; //Enable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
            birdcopy[poolindex].GetComponent<Rigidbody2D>().WakeUp(); //Enable rigidbody of the instance

            birdcollisionboundcopy[poolindex].GetComponent<Collider2D>().isTrigger = true;
            birdcollisionboundcopy[poolindex].tag = "birdcollision";

            isbirddead = false;

            birdcuesound.Play(); //Play the pizza spawn sound

            //poolindex = poolindex + 1; //Increment the index that keeps track of all the birds in the scene

            //Update spawnconsecutive to tweak the consecutive spawn time based on difficulty
            float spawnconsecutivelower = spawnconsecutiveinitial / difficultymultiplier; //Calculate the spawnconsecutive lower bound every frame based on the time that has elapsed
            float spawnconsecutiveupper = (spawnconsecutiveinitial * 2f) / difficultymultiplier; //Calculate the spawnconsecutive upper bound every frame based on the time that has elapsed
            System.Random randdub = new System.Random(); //Declare a new instance of Random
            spawnconsecutive = ((float)randdub.NextDouble() * (spawnconsecutiveupper - spawnconsecutivelower)) + spawnconsecutivelower; //Update the spawn consecutive to cater for the difficulty. y = mx + c
            InvokeRepeating("DelayAction", spawnconsecutive, spawnconsecutive); //We have to call InvokeRepeating again to cater for the fact that the parameter spawnconsecutive is updated constantly.

        }

    }

    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBird()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledbirds[i].activeInHierarchy)
            {
                return pooledbirds[i];
            }
        }
        return null;
    }


    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBirdCollisionBound()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledbirdcollisionbounds[i].activeInHierarchy)
            {
                return pooledbirdcollisionbounds[i];
            }
        }
        return null;
    }


    //The 'OnTriggerEnter2D' function is a preset Unity function that will be called automatically when the owner of this script's collider makes contact with another collider.
    //For the function to work, the owner of the script must have a RigidBody2D, and a Collider2D. The other object must have otherobject.GetComponent<Collider2D>().isTrigger = true
    //The other object's tag must also already exist in the tag library, even if you assign it to randomly generated objects at runtime.
    //To facilitate player collision and ground collision, this function must exist in a script attached to the player and a separate one attached to the ground.
    //This function does not need to be called.
    void OnTriggerEnter2D(Collider2D birdcollider)
    {
        if ((birdcollider.tag == "birdcollision") && (isbirddead is false) && (charactermovementscript.livesleft > 0))
        {
            if (birdcollisionboundcopy[poolindex] == birdcollider.gameObject)
            {
                if (charactermovementscript.livesleft > 1)
                {
                    burpsound.Play(); //If the hazard collides with the player, but they don't die immediately after, make them burp
                }
                
                charactermovementscript.livesleft = charactermovementscript.livesleft - 1; //Decrement player's lives left
                charactermovementscript.canshoot = false; //Deactivate the player's ability to throw dumbbells if they are hit

                PlayBirdDeath(); //Call the function that plays the bird death animation
                                  //Execute a delay, then delete the bird instance elements
                birdcopy[poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                birdcopy[poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance
                Invoke("HandleBirdDeath", charactermovementscript.deathinterval); //Invoke the function 'HandlebirdDeath' once with the same 'deathinterval' as the player character

            }

        }

    }


    public void PlayBirdDeath()
    {
        collisionsound.Play(); //Play the collision sound
        birdanimator.Play("bird_death"); //Play the bird death animation
        isbirddead = true; //The bird has been vanquished

    }


    private void HandleBirdDeath()
    {
        birdcopy[poolindex].SetActive(false); //Remove instance
        birdcopy[poolindex] = null; //Clear array slot
        birdcollisionboundcopy[poolindex].SetActive(false); //Remove instance
        birdcollisionboundcopy[poolindex] = null; //Clear array slot

    }


    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }


}
