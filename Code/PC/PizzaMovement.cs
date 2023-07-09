using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaMovement : MonoBehaviour
{
    public static PizzaMovement SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts for the same owner.
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public BakerMovement bakermovementscript; //Reference the baker movement script
    public List<GameObject> pooledpizzas; //Declare a new list to store the pizzas
    public List<GameObject> pooledpizzacollisionbounds; //Declare a new list to store the pizza collision bounds
    public int poolamount = 1; //Declare a variable to store the maximum number of copies to allow in the pool
    public int poolindex = 0; //Declare a variable to store an index for keeping track of all pizzas in the scene

    Rigidbody2D pizza; //Declare the rigidbody of the pizza
    public Animator pizzaanimator; //Declare the animator component for the pizza instance
    Rigidbody2D player; //Declare the rigidbody of the player
    Rigidbody2D baker; //Declare the rigidbody of the baker
    public GameObject pizzaobj; //Declare a new GameObject which will be used to find and store the pizza
    public GameObject pizzacollisionboundobj; //Declare a new GameObject which will be used to find and store the pizza collision bound
    public GameObject bakerobj; //Declare a new GameObject which will be used to find and store the baker
    public GameObject[] pizzacopy; //Declare a new GameObject which will be used to store pizza copies
    public GameObject[] pizzacollisionboundcopy; //Declare a new GameObject which will be used to store pizza collision bound copies
    public GameObject mainground; //Declare a new GameObject which will be used to find and store the main ground object
    public float spawninitial = 10f; //Declare a variable to store the initial spawn interval
    public float spawnconsecutiveinitial = 20f; //Declare a variable to store the initial consecutive spawn interval
    public float spawnconsecutive; //Declare a variable to store the interval between consecutive spawn events
    public bool isspawnright = true; //Declare a variable to store the direction from which the pizza will spawn
    public float pizzaspeed = 5f; //Declare a variable that will hold the pizza's movement speed
    public float playerinscribedradius; //Declare a variable to get the radius of the inscribed circle of the player's square sprite. For use as an approximation in collision calculations without using the sensor.
    public float bakerinscribedradius; //Declare a variable to get the radius of the inscribed circle of the baker's square sprite. For use as an approximation in collision calculations without using the sensor.
    public float pizzaradius; //Declare a variable to get the radius of the pizza's circular sprite
    public float groundmargin; //Declare a variable to represent a margin to be used for spawning pizza instances

    public float difficultymultiplier=1f; //Declare a variable to decrease the spawn interval with difficulty. Initialize it at 1.
    public bool ispizzadead = false; //Declare a variable to keep track of whether the pizza instance has been destroyed or not

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource pizzacuesound; //Declare a new sound source
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
        pizza = pizzaobj.GetComponent<Rigidbody2D>();
        baker = bakerobj.GetComponent<Rigidbody2D>();

        playerinscribedradius = (gameObject.transform.localScale.x)/2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        bakerinscribedradius = (bakerobj.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        pizzaradius = (pizzaobj.transform.localScale.x) / 2; //Set the variable storing the radius of the pizza to half the width of the circular sprite 
        groundmargin = mainground.transform.position.y + (mainground.transform.localScale.y / 2) + (pizzaobj.transform.localScale.y / 2) + 0.05f; //Set a ground margin for pizza spawning to the sum of the main ground block's y-position, half the height of the main ground block, half the height of the pizza circular sprite, and a safety margin

        pizzacopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated pizzas present in the scene
        pizzacollisionboundcopy = new GameObject[poolamount]; //Declare an array to keep track of all instantiated pizza collision bounds present in the scene

        //Initialize pooling by populating the GameObject list
        pooledpizzas = new List<GameObject>(); //Initialize list to store the pooled pizzas
        GameObject tmp; //Temporary variable to hold an instance of the pizza
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(pizzaobj);
            tmp.SetActive(false);
            pooledpizzas.Add(tmp); //Add pizzas copies to the list of pooled objects
        }

        //Initialize pooling by populating the GameObject list
        pooledpizzacollisionbounds = new List<GameObject>(); //Initialize list to store the pooled pizzacollisionbounds
        for (int i = 0; i < poolamount; i++)
        {
            tmp = Instantiate(pizzacollisionboundobj);
            tmp.SetActive(false);
            pooledpizzacollisionbounds.Add(tmp); //Add pizzacollisionbound copies to the list of pooled objects
        }

        //Initialize sound
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences
        pizzacuesound.volume = soundeffectsvolume; //Set the volume of the sound source
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
        
        //Manage Out-Of-Bounds
        //If the pizza array slot is not empty AND the pizza goes out of bounds
        if ((pizzacopy[poolindex] != null) && (ispizzadead is false) && (isspawnright is true) && (pizzacopy[poolindex].transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - pizzaradius)) //If the pizza instance spawned on the right and the pizza goes past the left-most bound of the camera
        {
            pizzacopy[poolindex].SetActive(false); //Remove instance
            pizzacopy[poolindex] = null; //Clear the array slot
        }
        else if ((pizzacopy[poolindex] != null) && (ispizzadead is false) && (isspawnright is false) && (pizzacopy[poolindex].transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + pizzaradius)) //If the pizza instance spawned on the left and the pizza goes past the right-most bound of the camera
        {
            pizzacopy[poolindex].SetActive(false); //Remove instance
            pizzacopy[poolindex] = null; //Clear the array slot
        }

        //Set pizza motion, based on the side of the screen it spawned on
        if ((pizzacopy[poolindex] != null) && (ispizzadead is false) && isspawnright is false)
        {
            pizzacopy[poolindex].GetComponent<Rigidbody2D>().velocity = new Vector2(pizzaspeed, 0f); //The pizza spawned on the left side of the screen, so its initial velocity must be set as positive
            pizzaanimator.Play("pizza_roll_right"); //Play the pizza roll right animation
        }
        else if ((pizzacopy[poolindex] != null) && (ispizzadead is false) && isspawnright is true)
        {
            pizzacopy[poolindex].GetComponent<Rigidbody2D>().velocity = new Vector2(-pizzaspeed, 0f); //The pizza spawned on the right side of the screen, so its initial velocity must be set as negative
            pizzaanimator.Play("pizza_roll_left"); //Play the pizza roll left animation
        }


    }

    // After every spawninterval
    private void DelayAction()
    {
        if (pizzacopy[poolindex] is null) //If there are no pizzas in the scene
        {
            //If the index that keeps track of all the pizzas in the scene goes out of bounds
            if (poolindex >= poolamount)
            {
                poolindex = 0; //Reset the index that keeps track of all the pizzas in the scene
            }

            //Spawn a new pizza instance
            pizzacopy[poolindex] = PizzaMovement.SharedInstance.GetPooledPizza(); //Get pooled pizzas
            pizzacollisionboundcopy[poolindex] = PizzaMovement.SharedInstance.GetPooledPizzaCollisionBound(); //Get pooled pizza collision bounds
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
            //Set pizza spawn position
            pizzacopy[poolindex].transform.position = spawnpos; //Set the pizza copy position to that of the random spawn point
            pizzacollisionboundcopy[poolindex].transform.position = spawnpos; //Set the pizza collision bound copy position to that of the random spawn point
            pizzacopy[poolindex].SetActive(true); //Spawn the pizza
            pizzacollisionboundcopy[poolindex].SetActive(true); //Spawn the pizza collision bound

            pizzaanimator = pizzacopy[poolindex].GetComponent<Animator>(); //Bind animator component of the owner to the specified variable

            pizzacollisionboundcopy[poolindex].transform.parent = pizzacopy[poolindex].transform; //Set the object instance as the parent of the collision bound instance

            //Re-enable physics. This has to be done explicitly since we disable physics for each instance when we destroy it to allow time for the animation.
            pizzacopy[poolindex].GetComponent<Rigidbody2D>().simulated = true; //Enable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
            pizzacopy[poolindex].GetComponent<Rigidbody2D>().WakeUp(); //Enable rigidbody of the instance

            pizzacollisionboundcopy[poolindex].GetComponent<Collider2D>().isTrigger = true;
            pizzacollisionboundcopy[poolindex].tag = "pizzacollision";

            ispizzadead = false;

            pizzacuesound.Play(); //Play the pizza spawn sound

            //poolindex = poolindex + 1; //Increment the index that keeps track of all the pizzas in the scene

            //Update spawnconsecutive to tweak the consecutive spawn time based on difficulty
            float spawnconsecutivelower = spawnconsecutiveinitial / difficultymultiplier; //Calculate the spawnconsecutive lower bound every frame based on the time that has elapsed
            float spawnconsecutiveupper = (spawnconsecutiveinitial * 2f) / difficultymultiplier; //Calculate the spawnconsecutive upper bound every frame based on the time that has elapsed
            System.Random randdub = new System.Random(); //Declare a new instance of Random
            spawnconsecutive = ((float)randdub.NextDouble() * (spawnconsecutiveupper - spawnconsecutivelower)) + spawnconsecutivelower; //Update the spawn consecutive to cater for the difficulty. y = mx + c
            InvokeRepeating("DelayAction", spawnconsecutive, spawnconsecutive); //We have to call InvokeRepeating again to cater for the fact that the parameter spawnconsecutive is updated constantly.

        }

    }

    // Define a function to deal with pooling of game objects
    public GameObject GetPooledPizza()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledpizzas[i].activeInHierarchy)
            {
                return pooledpizzas[i];
            }
        }
        return null;
    }


    // Define a function to deal with pooling of game objects
    public GameObject GetPooledPizzaCollisionBound()
    {
        for (int i = 0; i < poolamount; i++)
        {
            if (!pooledpizzacollisionbounds[i].activeInHierarchy)
            {
                return pooledpizzacollisionbounds[i];
            }
        }
        return null;
    }


    //The 'OnTriggerEnter2D' function is a preset Unity function that will be called automatically when the owner of this script's collider makes contact with another collider.
    //For the function to work, the owner of the script must have a RigidBody2D, and a Collider2D. The other object must have otherobject.GetComponent<Collider2D>().isTrigger = true
    //The other object's tag must also already exist in the tag library, even if you assign it to randomly generated objects at runtime.
    //To facilitate player collision and ground collision, this function must exist in a script attached to the player and a separate one attached to the ground.
    //This function does not need to be called.
    void OnTriggerEnter2D(Collider2D pizzacollider)
    {
        if ((pizzacollider.tag == "pizzacollision") && (ispizzadead is false) && (charactermovementscript.livesleft > 0))
        {
            if (pizzacollisionboundcopy[poolindex] == pizzacollider.gameObject) //If the player collides with a tag with the label 'pizzacollision'
            {
                if (charactermovementscript.livesleft > 1)
                {
                    burpsound.Play(); //If the hazard collides with the player, but they don't die immediately after, make them burp
                }
                charactermovementscript.livesleft = charactermovementscript.livesleft - 1; //Decrement player's lives left
                charactermovementscript.canshoot = false; //Deactivate the player's ability to throw dumbbells if they are hit

                PlayPizzaDeath(); //Call the function that plays the pizza death animation
                                                      //Execute a delay, then delete the pizza instance elements
                pizzacopy[poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                pizzacopy[poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance
                Invoke("HandlePizzaDeath", charactermovementscript.deathinterval); //Invoke the function 'HandlePizzaDeath' once with the same 'deathinterval' as the player character

                
            }

        }

    }


    public void PlayPizzaDeath()
    {
        collisionsound.Play(); //Play the collision sound
        pizzaanimator.Play("pizza_death"); //Play the pizza death animation
        ispizzadead = true; //The pizza has been vanquished

    }


    private void HandlePizzaDeath()
    {
        pizzacopy[poolindex].SetActive(false); //Remove instance
        pizzacopy[poolindex] = null; //Clear array slot
        pizzacollisionboundcopy[poolindex].SetActive(false); //Remove instance
        pizzacollisionboundcopy[poolindex] = null; //Clear array slot

    }

    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }


}
