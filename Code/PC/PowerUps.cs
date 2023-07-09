using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public static PowerUps SharedInstance; //Declare a new shared instance
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public HUDScript hudscript; //Reference the HUD script
    public BurgerMovement burgermovementscript; //Reference the burger movement script
    public PizzaMovement pizzamovementscript; //Reference the pizza movement script
    public BirdMovement birdmovementscript; //Reference the bird movement script
    public BakerMovement bakermovementscript; //Reference the baker movement script
    Rigidbody2D player; //Declare the rigidbody that will own this script
    public List<GameObject> pooledcreatines; //Declare a new list to store the creatine power-ups
    public List<GameObject> pooledchickens; //Declare a new list to store the chicken power-ups
    public List<GameObject> pooleddumbbells; //Declare a new list to store the dumbbell power-ups
    public List<GameObject> pooledbullets; //Declare a new list to store the dumbbell bullets
    public List<GameObject> pooledbulletcollisionbounds; //Declare a new list to store the bullet collision bounds
    public int creatineamount = 1; //Declare a variable to store the maximum number of creatine power-ups to allow in the pool
    public int chickenamount = 1; //Declare a variable to store the maximum number of chicken power-ups to allow in the pool
    public int dumbbellamount = 1; //Declare a variable to store the maximum number of dumbbell power-ups to allow in the pool
    public int bulletamount = 100; //Declare a variable to store the maximum number of dumbbell bullets to allow in the pool

    public int creatineindex = 0; //Declare a variable to store an index for keeping track of all creatine power-ups in the scene
    public int chickenindex = 0; //Declare a variable to store an index for keeping track of all chicken power-ups in the scene
    public int dumbbellindex = 0; //Declare a variable to store an index for keeping track of all dumbbell power-ups in the scene
    public int bulletindex = 0; //Declare a variable to store an index for keeping track of all dumbbell bullets in the scene

    public Animator[] bulletanimator; //Declare the animator component of the multiple bullets
    public GameObject creatineobj; //Declare a new GameObject which will be used to find and store the creatine power-up
    public GameObject chickenobj; //Declare a new GameObject which will be used to find and store the chicken power-up
    public GameObject dumbbellobj; //Declare a new GameObject which will be used to find and store the dumbell power-up
    public GameObject bulletobj; //Declare a new GameObject which will be used to find and store the dumbell bullet
    public GameObject bulletcollisionboundobj; //Declare a new GameObject which will be used to find and store the bullet collision bound
    public GameObject[] creatinecopy; //Declare a new GameObject which will be used to store creatine copies
    public GameObject[] chickencopy; //Declare a new GameObject which will be used to store creatine copies
    public GameObject[] dumbbellcopy; //Declare a new GameObject which will be used to store dumbbell copies
    public GameObject[] bulletcopy; //Declare a new GameObject which will be used to store dumbbell bullet copies
    public GameObject[] bulletcollisionboundcopy; //Declare a new GameObject which will be used to store bullet collision bound copies
    public GameObject mainground; //Declare a new GameObject which will be used to find and store the main ground object
    public float spawninitial = 5f; //Declare a variable to store the initial spawn interval
    public float spawnconsecutiveinitial = 10f; //Declare a variable to store the initial consecutive spawn interval
    public float spawnconsecutive; //Declare a variable to store the interval between consecutive spawn events
    public float playerinscribedradius; //Declare a variable to get the radius of the inscribed circle of the player's square sprite. For use as an approximation in collision calculations without using the sensor.
    public float creatineinscribedradius; //Declare a variable to get the inscribed radius of the creatine's square sprite
    public float chickeninscribedradius; //Declare a variable to get the inscribed radius of the chicken's square sprite
    public float dumbbellinscribedradius; //Declare a variable to get the inscribed radius of the dumbbell's square sprite
    public float creatinegroundmargin; //Declare a variable to represent a margin to be used for spawning creatine power-up instances
    public float chickengroundmargin; //Declare a variable to represent a margin to be used for spawning chicken power-up instances
    public float dumbbellgroundmargin; //Declare a variable to represent a margin to be used for spawning dumbbell power-up instances

    public float burgerinscribedradius; //Declare a variable to store an enemy/hazard inscribed radius
    public float pizzaradius; //Declare a variable to store an enemy/hazard radius
    public float birdinscribedradius; //Declare a variable to store an enemy/hazard inscribed radius
    public float bakerinscribedradius; //Declare a variable to store an enemy/hazard inscribed radius

    public int whichpowerup; //Declare a variable from 1 to 3, which will randomly determine which power up to spawn
    public bool[] isbulletdead;

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource powerupssound; //Declare a new sound source
    public AudioSource collisionsound; //Declare a new sound source

    // Awake allows declaration or initialization of variables before the program runs. 
    void Awake()
    {
        SharedInstance = this; //This is the shared instance
    }


    // Start is called before the first frame update
    void Start()
    {
        //Declare rigid bodies in case they need to be used
        player = gameObject.GetComponent<Rigidbody2D>();

        playerinscribedradius = (gameObject.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        creatineinscribedradius = (creatineobj.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        chickeninscribedradius = (chickenobj.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        dumbbellinscribedradius = (dumbbellobj.transform.localScale.x) / 2; //Set the variable storing the radius of the inscribed circle to half the width of the square sprite
        creatinegroundmargin = mainground.transform.position.y + (mainground.transform.localScale.y / 2) + (creatineobj.transform.localScale.y / 2) - 0.2f; //Set a ground margin for creatine power-up spawning to the sum of the main ground block's y-position, half the height of the main ground block, half the height of the creatine sprite, and a safety margin
        chickengroundmargin = mainground.transform.position.y + (mainground.transform.localScale.y / 2) + (chickenobj.transform.localScale.y / 2) - 0.25f; //Set a ground margin for chicken power-up spawning to the sum of the main ground block's y-position, half the height of the main ground block, half the height of the chicken sprite, and a safety margin
        dumbbellgroundmargin = mainground.transform.position.y + (mainground.transform.localScale.y / 2) + (dumbbellobj.transform.localScale.y / 2) - 0.15f; //Set a ground margin for dumbbell power-up spawning to the sum of the main ground block's y-position, half the height of the main ground block, half the height of the dumbbell sprite, and a safety margin

        burgerinscribedradius = (burgermovementscript.burgerobj.transform.localScale.x / 2)+0.05f; //Declare enemy inscribed radius with a safety margin
        pizzaradius = (pizzamovementscript.pizzaobj.transform.localScale.x / 2) +0.05f; //Declare enemy inscribed radius with a safety margin
        birdinscribedradius = (birdmovementscript.birdobj.transform.localScale.x / 2) + 0.05f; //Declare enemy inscribed radius with a safety margin
        bakerinscribedradius = (bakermovementscript.bakerobj.transform.localScale.x / 2) + 0.05f; //Declare enemy inscribed radius with a safety margin

        creatinecopy = new GameObject[creatineamount]; //Declare an array to keep track of all instantiated creatines present in the scene
        chickencopy = new GameObject[chickenamount]; //Declare an array to keep track of all instantiated chicken present in the scene
        dumbbellcopy = new GameObject[dumbbellamount]; //Declare an array to keep track of all instantiated dumbbell present in the scene
        bulletcopy = new GameObject[bulletamount]; //Declare an array to keep track of all instantiated dumbbell bullets present in the scene
        bulletcollisionboundcopy = new GameObject[bulletamount]; //Declare an array to keep track of all instantiated bullet collision bounds present in the scene

        bulletanimator = new Animator[bulletamount]; //Declare an array to keep track of the animator components of each instance
        isbulletdead = new bool[bulletamount]; //Declare an array to keep track of whether each instance is still existing or not

        //Initialize pooling by populating the GameObject list
        pooledcreatines = new List<GameObject>(); //Initialize list to store the pooled creatines
        GameObject tmp; //Temporary variable to hold an instance of the creatine
        for (int i = 0; i < creatineamount; i++)
        {
            tmp = Instantiate(creatineobj);
            tmp.SetActive(false);
            pooledcreatines.Add(tmp); //Add creatine copies to the list of pooled objects
        }

        pooledchickens = new List<GameObject>(); //Initialize list to store the pooled chickens
        for (int i = 0; i < chickenamount; i++)
        {
            tmp = Instantiate(chickenobj);
            tmp.SetActive(false);
            pooledchickens.Add(tmp); //Add chicken copies to the list of pooled objects
        }

        pooleddumbbells = new List<GameObject>(); //Initialize list to store the pooled dumbbells
        for (int i = 0; i < dumbbellamount; i++)
        {
            tmp = Instantiate(dumbbellobj);
            tmp.SetActive(false);
            pooleddumbbells.Add(tmp); //Add dumbbell copies to the list of pooled objects
        }

        pooledbullets = new List<GameObject>(); //Initialize list to store the pooled bullets
        for (int i = 0; i < bulletamount; i++)
        {
            tmp = Instantiate(bulletobj);
            tmp.SetActive(false);
            pooledbullets.Add(tmp); //Add bullet copies to the list of pooled objects
        }

        //Initialize pooling by populating the GameObject list
        pooledbulletcollisionbounds = new List<GameObject>(); //Initialize list to store the pooled bulletcollisionbounds
        for (int i = 0; i < bulletamount; i++)
        {
            tmp = Instantiate(bulletcollisionboundobj);
            tmp.SetActive(false);
            pooledbulletcollisionbounds.Add(tmp); //Add bulletcollisionbound copies to the list of pooled objects
        }

        //Initialize sound
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences
        powerupssound.volume = soundeffectsvolume; //Set the volume of the sound source
        collisionsound.volume = soundeffectsvolume; //Set the volume of the sound source

        spawnconsecutive = spawnconsecutiveinitial; //Set the consecutive spawn interval to its initial value
        InvokeRepeating("DelayAction", spawninitial, spawnconsecutive); //Repeat the function 'DelayAction' after initial time 'spawninterval', and every 'spawninterval' thereafter
    }

    // Update is called once per frame
    void Update()
    {
        //Manage power-up collisions. No 'for' loop is required since we will only deal with one creatine on the screen at a time.

        //#########################################################################################################################
        //## FEELING WAY TOO LAZY TO CHANGE THE DISTANCE BASED COLLISION APPROXIMATION TO THE CORRECT 'OnTriggerEnter2D' METHOD. ##
        //## LET THE POWER - UPS RETAIN THE OLD METHOD FOR POSTERITY AND VARIETY. IT WORKS WELL ENOUGH.                          ##
        //#########################################################################################################################

        //If the creatine array slot is not empty AND the creatine is near enough to the player to constitute a collision
        if ((creatinecopy[creatineindex] != null) && (charactermovementscript.livesleft > 0) && (Vector2.Distance(creatinecopy[creatineindex].transform.position, gameObject.transform.position) <= (playerinscribedradius + creatineinscribedradius)))
        {
            powerupssound.Play(); //Play the power ups sound
            charactermovementscript.livesleft = charactermovementscript.livesleft + 1; //Increment the player's number of lives
            creatinecopy[creatineindex].SetActive(false); //Remove instance
            creatinecopy[creatineindex] = null; //Clear the array slot
            
        }

        //If the chicken array slot is not empty AND the chicken is near enough to the player to constitute a collision.
        if ((chickencopy[chickenindex] != null) && (charactermovementscript.livesleft > 0) && (Vector2.Distance(chickencopy[chickenindex].transform.position, gameObject.transform.position) <= (playerinscribedradius + chickeninscribedradius)))
        {
            powerupssound.Play(); //Play the power ups sound
            chickencopy[chickenindex].SetActive(false); //Remove instance
            chickencopy[chickenindex] = null; //Clear the array slot
            hudscript.additionalscore = hudscript.additionalscore + 5f; //Implement extra points
        }

        //If the dumbbell array slot is not empty AND the dumbbell is near enough to the player to constitute a collision
        if ((dumbbellcopy[dumbbellindex] != null) && (charactermovementscript.livesleft > 0) && (Vector2.Distance(dumbbellcopy[dumbbellindex].transform.position, gameObject.transform.position) <= (playerinscribedradius + dumbbellinscribedradius)))
        {
            powerupssound.Play(); //Play the power ups sound
            charactermovementscript.canshoot = true; //Give the player the ability to shoot
            dumbbellcopy[dumbbellindex].SetActive(false); //Remove instance
            dumbbellcopy[dumbbellindex] = null; //Clear the array slot
        }


    }

    // After every spawninterval
    private void DelayAction()
    {
        //Update spawnconsecutive to tweak the consecutive spawn time based on difficulty
        float spawnconsecutivelower = spawnconsecutiveinitial; //Calculate the spawnconsecutive lower bound every every instance creation period
        float spawnconsecutiveupper = (spawnconsecutiveinitial * 2f); //Calculate the spawnconsecutive upper bound every instance creation period
        System.Random randdub = new System.Random(); //Declare a new instance of Random
        spawnconsecutive = ((float)randdub.NextDouble() * (spawnconsecutiveupper - spawnconsecutivelower)) + spawnconsecutivelower; //Update the spawn consecutive to cater for the difficulty. y = mx + c
        InvokeRepeating("DelayAction", spawnconsecutive, spawnconsecutive); //We have to call InvokeRepeating again to cater for the fact that the parameter spawnconsecutive is updated constantly.

        //Randomly choose which power-up to spawn
        System.Random randint = new System.Random(); //Declare a new instance of Random
        whichpowerup = randint.Next(1,5); //Generate a random number between 1 and 4. The random function is exclusive of the upper bound. The '4' position is empty to serve as a dummy filler.

        if (whichpowerup == 1)
        {
            if (creatinecopy[creatineindex] is null) //If there are no creatines in the scene
            {
                //If the index that keeps track of all the creatines in the scene goes out of bounds
                if (creatineindex >= creatineamount)
                {
                    creatineindex = 0; //Reset the index that keeps track of all the creatines in the scene
                }

                //Spawn a new creatine instance
                creatinecopy[creatineindex] = PowerUps.SharedInstance.GetPooledCreatine(); //Get pooled objects
                //Define a random spawn point within the bounds of the camera width
                float yloc = creatinegroundmargin;
                float xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                Vector2 spawnpos = new Vector2(xloc, yloc);
                //Make sure that there are at least five player radiuses between the player and the power-up. If not, recalculate the spawn x-position
                if ((charactermovementscript.livesleft > 0) && (Vector2.Distance(spawnpos, gameObject.transform.position) <= (5 * playerinscribedradius)))
                {
                    xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                }

                spawnpos = new Vector2(xloc, yloc);
                //Set creatine spawn position
                creatinecopy[creatineindex].transform.position = spawnpos; //Set the creatine copy position to that of the random spawn point
                creatinecopy[creatineindex].SetActive(true); //Spawn the creatine

                //creatineindex = creatineindex + 1; //Increment the index that keeps track of all the creatines in the scene
            }
        }
        else if (whichpowerup == 2)
        {
            if (chickencopy[chickenindex] is null) //If there are no chickens in the scene
            {
                //If the index that keeps track of all the chickens in the scene goes out of bounds
                if (chickenindex >= chickenamount)
                {
                    chickenindex = 0; //Reset the index that keeps track of all the chickens in the scene
                }

                //Spawn a new chicken instance
                chickencopy[chickenindex] = PowerUps.SharedInstance.GetPooledChicken(); //Get pooled objects
                //Define a random spawn point within the bounds of the camera width
                float yloc = chickengroundmargin;
                float xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                Vector2 spawnpos = new Vector2(xloc, yloc);
                //Make sure that there are at least five player radiuses between the player and the power-up. If not, recalculate the spawn x-position
                if ((charactermovementscript.livesleft > 0) && (Vector2.Distance(spawnpos, gameObject.transform.position) <= (5 * playerinscribedradius)))
                {
                    xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                }

                spawnpos = new Vector2(xloc, yloc);
                //Set chicken spawn position
                chickencopy[chickenindex].transform.position = spawnpos; //Set the chicken copy position to that of the random spawn point
                chickencopy[chickenindex].SetActive(true); //Spawn the chicken

                //chickenindex = chickenindex + 1; //Increment the index that keeps track of all the chickens in the scene
            }
        }
        else if (whichpowerup == 3)
        {
            if (dumbbellcopy[dumbbellindex] is null) //If there are no dumbbells in the scene
            {
                //If the index that keeps track of all the dumbbells in the scene goes out of bounds
                if (dumbbellindex >= dumbbellamount)
                {
                    dumbbellindex = 0; //Reset the index that keeps track of all the dumbbells in the scene
                }

                //Spawn a new dumbbell instance
                dumbbellcopy[dumbbellindex] = PowerUps.SharedInstance.GetPooledDumbbell(); //Get pooled objects
                //Define a random spawn point within the bounds of the camera width
                float yloc = dumbbellgroundmargin;
                float xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                Vector2 spawnpos = new Vector2(xloc, yloc);
                //Make sure that there are at least five player radiuses between the player and the power-up. If not, recalculate the spawn x-position
                if ((charactermovementscript.livesleft > 0) && (Vector2.Distance(spawnpos, gameObject.transform.position) <= (5 * playerinscribedradius)))
                {
                    xloc = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);
                }

                spawnpos = new Vector2(xloc, yloc);
                //Set dumbbell spawn position
                dumbbellcopy[dumbbellindex].transform.position = spawnpos; //Set the dumbbell copy position to that of the random spawn point
                dumbbellcopy[dumbbellindex].SetActive(true); //Spawn the dumbbell

                //dumbbellindex = dumbbellindex + 1; //Increment the index that keeps track of all the dumbbells in the scene
            }

        }

    }

    // Define a function to deal with pooling of game creatine power-ups
    public GameObject GetPooledCreatine()
    {
        for (int i = 0; i < creatineamount; i++)
        {
            if (!pooledcreatines[i].activeInHierarchy)
            {
                return pooledcreatines[i];
            }
        }
        return null;
    }

    // Define a function to deal with pooling of game chicken power-ups
    public GameObject GetPooledChicken()
    {
        for (int i = 0; i < chickenamount; i++)
        {
            if (!pooledchickens[i].activeInHierarchy)
            {
                return pooledchickens[i];
            }
        }
        return null;
    }

    // Define a function to deal with pooling of game dumbbell power-ups
    public GameObject GetPooledDumbbell()
    {
        for (int i = 0; i < dumbbellamount; i++)
        {
            if (!pooleddumbbells[i].activeInHierarchy)
            {
                return pooleddumbbells[i];
            }
        }
        return null;
    }

    // Define a function to deal with pooling of game dumbbell bullets
    public GameObject GetPooledBullet()
    {
        for (int i = 0; i < bulletamount; i++)
        {
            if (!pooledbullets[i].activeInHierarchy)
            {
                return pooledbullets[i];
            }
        }
        return null;
    }

    // Define a function to deal with pooling of game objects
    public GameObject GetPooledBulletCollisionBound()
    {
        for (int i = 0; i < bulletamount; i++)
        {
            if (!pooledbulletcollisionbounds[i].activeInHierarchy)
            {
                return pooledbulletcollisionbounds[i];
            }
        }
        return null;
    }


    public void PlayBulletDeath(int bulletdeathindex)
    {
        isbulletdead[bulletdeathindex] = true; //The dumbbell bullet has been vanquished
        for (int i = 0; i < bulletamount; i++)
        {
            if (isbulletdead[i] is true)
            {
                collisionsound.Play(); //Play the collision sound
                bulletanimator[bulletdeathindex].Play("dumbbell_death"); //Play the dumbbell bullet death animation
            }
        }

    }


    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }


}
