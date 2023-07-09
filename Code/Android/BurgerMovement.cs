using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerMovement : MonoBehaviour
{
    public static BurgerMovement SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts for the same owner.
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public List<GameObject> pooledburgers; //Declare a new list to store the burgers
    public List<GameObject> pooledburgercollisionbounds; //Declare a new list to store the burger collision bounds
    public int poolamount = 15; //Declare a variable to store the maximum number of copies to allow in the pool
    public int poolindex = 0; //Declare a variable to store an index for keeping track of all burgers in the scene

    Rigidbody2D burger; //Declare the rigidbody of the burger
    public Animator[] burgeranimator; //Declare the animator component of the multiple burgers
    Rigidbody2D player; //Declare the rigidbody of the player
    public GameObject burgerobj; //Declare a new GameObject which will be used to find and store the burger
    public GameObject burgercollisionboundobj; //Declare a new GameObject which will be used to find and store the burger collision bound
    public GameObject[] burgercopy; //Declare a new GameObject which will be used to store burger copies
    public GameObject[] burgercollisionboundcopy; //Declare a new GameObject which will be used to store burger collision bound copies
    public GameObject mainground; //Declare a new GameObject which will be used to find and store the main ground object
    public float spawninitial = 1f; //Declare a variable to store the time from the first spawn
    public float spawnconsecutiveinitial = 10f; //Declare a variable to store the initial consecutive spawn interval
    public float spawnconsecutive; //Declare a variable to store the interval between consecutive spawn events
    public float groundmargin; //Declare a variable to represent a margin to be used for calculating collisions with the ground
    public float playermargin; //Declare a variable to represent a margin to be used for calculating collisions with the player

    public float difficultymultiplier = 1f; //Declare a variable to decrease the spawn interval with difficulty. Initialize it at 1.
    public bool[] isburgerdead;

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
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
        burger = burgerobj.GetComponent<Rigidbody2D>();

        playermargin = (gameObject.transform.localScale.x/2)+ (burgerobj.transform.localScale.x / 2) + 0.05f; //Set the burger-player collision calculation margin to the sum of half the width of each of the respective square sprites, as well as a safety margin
        groundmargin = mainground.transform.position.y + (mainground.transform.localScale.y / 2) + (burgerobj.transform.localScale.y/2) + 0.05f; //Set the burger-ground collision calculation margin to the sum of the main ground block's y-position, half the height of the main ground block, half the height of the burger square sprite, and a safety margin

        burgercopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated burgers present in the scene
        burgercollisionboundcopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated burger collision bounds present in the scene

        burgeranimator = new Animator[poolamount]; //Declare an array to keep track of the animator components of each instance
        isburgerdead = new bool[poolamount]; //Declare an array to keep track of whether each instance is still existing or not

        //Initialize pooling by populating the GameObject list
        pooledburgers = new List<GameObject>(); //Initialize list to store the pooled burgers
        GameObject tmp; //Temporary variable to hold an instance of the burger
        for (int i=0; i<poolamount; i++)
        {
            tmp = Instantiate(burgerobj);
            tmp.SetActive(false);
            pooledburgers.Add(tmp); //Add burgers copies to the list of pooled objects
        }

        //Initialize pooling by populating the GameObject list
        pooledburgercollisionbounds = new List<GameObject>(); //Initialize list to store the pooled burgercollisionbounds
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(burgercollisionboundobj);
            tmp.SetActive(false);
            pooledburgercollisionbounds.Add(tmp); //Add burgercollisionbound copies to the list of pooled objects
        }

        //Initialize sound
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences
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
            difficultymultiplier = 1f;
        }
        else if ((Time.timeSinceLevelLoad > 60f) && (Time.timeSinceLevelLoad <= 120f))
        {
            difficultymultiplier = 1.1f;
        }
        else if (Time.timeSinceLevelLoad > 120f)
        {
            difficultymultiplier = 1.25f;
        }

    }


    // After every spawninterval
    private void DelayAction()
    {
        //If the index that keeps track of all the burgers in the scene goes out of bounds
        if (poolindex >= poolamount) 
        {
            poolindex = 0; //Reset the index that keeps track of all the burgers in the scene
        }

        if (burgercopy[poolindex] is null) //Had to add this to make sure we don't try and instantiate more burgers than the maximum we set. Thus if an instance is in use, we will not spawn a burger
        {
            //Spawn a new burger instance, as well as an instance of the burgercollisionbound
            burgercopy[poolindex] = BurgerMovement.SharedInstance.GetPooledBurger(); //Get pooled objects
            burgercollisionboundcopy[poolindex] = BurgerMovement.SharedInstance.GetPooledBurgerCollisionBound(); //Get pooled objects
                                                                                                                 //Define a random spawn point above the camera height, but within the camera width
            float yloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, (Screen.height + 50f))).y);
            float xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
            Vector2 spawnpos = new Vector2(xloc, yloc);
            //Set burger spawn position, as well as that of the burgercollisionbound
            burgercopy[poolindex].transform.position = spawnpos; //Set the burger copy position to that of the random spawn point
            burgercollisionboundcopy[poolindex].transform.position = spawnpos; //Set the burgercollisionbound copy position to that of the random spawn point
            burgercopy[poolindex].SetActive(true); //Spawn the burger
            burgercollisionboundcopy[poolindex].SetActive(true); //Spawn the burgercollisionbound

            burgeranimator[poolindex] = burgercopy[poolindex].GetComponent<Animator>(); //Bind animator component of the owner to the specified variable

            burgercollisionboundcopy[poolindex].transform.parent = burgercopy[poolindex].transform; //Set the object instance as the parent of the collision bound instance

            //Re-enable physics. This has to be done explicitly since we disable physics for each instance when we destroy it to allow time for the animation.
            burgercopy[poolindex].GetComponent<Rigidbody2D>().simulated = true; //Enable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
            burgercopy[poolindex].GetComponent<Rigidbody2D>().WakeUp(); //Enable rigidbody of the instance

            burgercollisionboundcopy[poolindex].GetComponent<Collider2D>().isTrigger = true;
            burgercollisionboundcopy[poolindex].tag = "burgercollision";

            isburgerdead[poolindex] = false;

            
        }

        
        poolindex = poolindex + 1; //Increment the index that keeps track of all the burgers in the scene. We do this regardless of which instances are in use or not.


        //Update spawnconsecutive to tweak the consecutive spawn time based on difficulty, regardless of which instances are in use or not.
        float spawnconsecutivelower = spawnconsecutiveinitial / difficultymultiplier; //Calculate the spawnconsecutive lower bound every frame based on the time that has elapsed
        float spawnconsecutiveupper = (spawnconsecutiveinitial * 1.2f) / difficultymultiplier; //Calculate the spawnconsecutive upper bound every frame based on the time that has elapsed
        System.Random randdub = new System.Random(); //Declare a new instance of Random
        spawnconsecutive = ((float)randdub.NextDouble() * (spawnconsecutiveupper - spawnconsecutivelower)) + spawnconsecutivelower; //Update the spawn consecutive to cater for the difficulty. y = mx + c
        InvokeRepeating("DelayAction", spawnconsecutive, spawnconsecutive); //We have to call InvokeRepeating again to cater for the fact that the parameter spawnconsecutive is updated constantly.

    }

    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBurger()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledburgers[i].activeInHierarchy)
            {
                return pooledburgers[i];
            }
        }
        return null;
    }

    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBurgerCollisionBound()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledburgercollisionbounds[i].activeInHierarchy)
            {
                return pooledburgercollisionbounds[i];
            }
        }
        return null;
    }

    //The 'OnTriggerEnter2D' function is a preset Unity function that will be called automatically when the owner of this script's collider makes contact with another collider.
    //For the function to work, the owner of the script must have a RigidBody2D, and a Collider2D. The other object must have otherobject.GetComponent<Collider2D>().isTrigger = true
    //The other object's tag must also already exist in the tag library, even if you assign it to randomly generated objects at runtime.
    //To facilitate player collision and ground collision, this function must exist in a script attached to the player and a separate one attached to the ground.
    //This function does not need to be called.
    void OnTriggerEnter2D(Collider2D burgercollider)
    {
        if ((burgercollider.tag == "burgercollision") && (charactermovementscript.livesleft >0))
        {
            for (int i=0; i<poolamount; i++)
            {
                if (burgercollisionboundcopy[i] == burgercollider.gameObject)
                {
                    if (charactermovementscript.livesleft > 1)
                    {
                        burpsound.Play(); //If the hazard collides with the player, but they don't die immediately after, make them burp
                    }
                    charactermovementscript.livesleft = charactermovementscript.livesleft - 1; //Decrement player's lives left
                    charactermovementscript.canshoot = false; //Deactivate the player's ability to throw dumbbells if they are hit

                    PlayBurgerDeath(i); //Call the function that plays the burger death animation
                                                                            //Execute a delay, then delete the burger instance elements
                    burgercopy[i].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                    burgercopy[i].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                    Invoke("HandleBurgerDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                }

            }
            
        }
        
    }

    public void PlayBurgerDeath(int burgerdeathindex)
    {
        isburgerdead[burgerdeathindex] = true; //The burger has been vanquished
        for (int i=0; i<poolamount; i++)
        {
            if ((isburgerdead[i] is true) && (burgercopy[i] != null))
            {
                collisionsound.Play(); //Play the collision sound
                burgeranimator[burgerdeathindex].Play("burger_death"); //Play the burger death animation
            }
        }

    }

    private void HandleBurgerDeath()
    {
        for (int i=0; i<poolamount; i++)
        {
            if ((isburgerdead[i] is true) && (burgercopy[i] != null))
            {
                burgercopy[i].SetActive(false); //Remove instance
                burgercopy[i] = null; //Clear array slot
                burgercollisionboundcopy[i].SetActive(false); //Remove instance
                burgercollisionboundcopy[i] = null; //Clear array slot
            }
        }
        

    }

    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }

}
