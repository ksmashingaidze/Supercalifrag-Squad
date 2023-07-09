using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class CharacterMovement : MonoBehaviour
{
    public static CharacterMovement SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts for the same owner.
    public PowerUps powerupsscript; //Reference the power-ups script
    public BakerMovement bakermovementscript; //Reference the baker movement script
    public HUDScript hudscript; //Reference the HUD script
    Rigidbody2D player; //Declare the rigidbody that will own this script
    public Animator playeranimator; //Declare the animator component of the player
    public float playermargin; //Declare a variable that will hold the player's inscribed circle radius for handling out-of-camera-bound cases
    public float playerspeed; //Declare a variable that will hold the player's movement speed
    public float defaultplayerspeed = 7f; //Declare a variable that will hold the player's default movement speed
    public float dashmagnitudex = 4000f; //Declare a variable that will hold the player's horizontal dash impulse force
    public float dashmagnitudey = 0f; //Declare a variable that will hold the player's vertical dash impulse force
    public float jumpspeed = 8f; //Declare a variable that will hold the player's jump speed
    public int extrajumps; //Declare a variable to allow extra mid-air jumps
    public int livesleft = 1; //Declare number of lives left
    public float deathinterval = 1f; //Declare amount of time to delay after death, before resetting the scene
    public int defaultextrajumps = 1; //Declare a variable to store the number of extra mid-air jumps allowed
    public bool isgrounded; //Declare a boolean variable to declare whether the player is touching the ground
    public bool isfaceright = true; //Declare a boolean variable to check whether the player is facing right or not. Initialize it as true in the beginning.
    public bool isdiving = false; //Declare a boolean variable to declare whether the player is diving
    public Transform groundsensor; //Declare a sensor to check whether the player is touching the ground
    public float groundradius = 0.05f; //Declare the distance from the ground required to facilitate a jump
    public float lastground; //Declare a variable to hold last time grounded.
    public float lastdash; //Declare a variable to hold the last time you performed a dash
    public float lastshoot; //Declare a variable to hold the last time you performed a shoot event
    public float timer; //Declare a timer variable to implement delays
    public float groundmemory = 0.1f; //Declare a variable to hold the amount of time you want to remember grounding. Caters for jumping right at the edge of a lip.
    public float dashconsecutivememory = 0.5f; //Declare a variable to hold the amount of time you want to set as a restriction between consecutive dashes.
    public float shootconsecutivememory = 0.1f; //Declare a variable to hold the amount of time you want to set as a restriction between consecutive shoot events.
    public LayerMask groundlayer; //Declare the ground layer
    public LayerMask bakerlayer; //Declare the baker layer
    public float jumpmultiplier = 2f; //Declare a gravity force multiplier for jumping
    public float fallmultiplier = 2.5f; //Declare a gravity force multiplier for falling
    public float divemultiplier = 10f; //Declare a gravity force multiplier for diving
    public bool canshoot = false; //Declare a boolean variable which governs whether or not the player can throw dumbbells
    public float bulletspeed = 10f; //Declare a variable to store the bullet speed
    public int currscore; //Declare a variable to store the player's current score for the purposes of high score handling

    public Button resumebtn; //Declare a new UI Button that will be used to find and store the resume button
    public Button restartbtn; //Declare a new UI Button that will be used to find and store the restart button
    public Button mainmenubtn; //Declare a new UI Button that will be used to find and store the main menu button
    public Image pausemenupanel; //Declare a new UI Image that will be used to find and store the image component of the pause menu panel. Panels have image and gameObject components.
    public Image pausemenulogo; //Declare a new UI Image that will be used to find and store the pause menu logo
    public Image unlockpanel; //Declare a new UI Image that will be used to find and store the image component of the item unlock panel. Panels have image and gameObject components.
    public Text unlocktext; //Declare a new UI Text item that will be used to find and store the item unlock text

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource musicsound; //Declare a new sound source for the music
    public AudioSource selectsound; //Declare a new sound source
    public AudioSource dashsound; //Declare a new sound source
    public AudioSource diesound; //Declare a new sound source
    public AudioSource divesound; //Declare a new sound source
    public AudioSource powerupssound; //Declare a new sound source
    public AudioSource unlocksound; //Declare a new sound source
    public AudioSource jumpsound; //Declare a new sound source
    public AudioSource throwsound; //Declare a new sound source

    public bool hasdieplayed; //Declare a boolean variable to keep track of whether the death sound has played

    public int currentskin = 0; //Declare an integer variable to keep track of which skin is currently selected

    public int isbeachunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int isabalancheunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int ispablounlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int islisaunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int ismmunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.

    // Awake allows declaration or initialization of variables before the program runs. 

    void Awake()
    {
        SharedInstance = this; //This is the shared instance. Used if you want to access variables between scripts for the same owner.

        //Disable the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Load the skins
        currentskin = PlayerPrefs.GetInt("currentskin", 0); //Get the current skin from player preferences.

        //Load the unlockable item states
        isbeachunlocked = PlayerPrefs.GetInt("isbeachunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        isabalancheunlocked = PlayerPrefs.GetInt("isabalancheunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        ispablounlocked = PlayerPrefs.GetInt("ispablounlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        islisaunlocked = PlayerPrefs.GetInt("islisaunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        ismmunlocked = PlayerPrefs.GetInt("ismmunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.

        //Handle Pablo and Lisa buffs
        if ((currentskin == 4) && (ispablounlocked ==1)) //Pablo
        {
            defaultplayerspeed = 14f; //Pablo runs faster than other characters
            defaultextrajumps = 1; //Pablo is only capable of double jump
        }
        else if ((currentskin == 5) && (islisaunlocked ==1)) //Lisa
        {
            defaultextrajumps = 2; //Lisa is capable of triple jump
            defaultplayerspeed = 7f; //Lisa can only run at normal speed
        }
        else
        {
            //Set defaults
            defaultplayerspeed = 7f;
            defaultextrajumps = 1;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<Rigidbody2D>(); //Also note that if the player sprite has a 'Box Collider' the player can get caught in the tiny gap between tiles. Use a 'Capsule Collider' instead.
        
        playeranimator = gameObject.GetComponent<Animator>(); //Bind animator component of the owner to the specified variable
        
        playerspeed = defaultplayerspeed; //Initialize player speed to its default value
        extrajumps = defaultextrajumps; //Initialize player extra jumps to their default values
        playermargin = gameObject.transform.localScale.x / 2; //Initialize the variable that will hold the player's inscribed circle radius for handling out-of-camera-bound cases


        //Initialize pause menu variables
        Time.timeScale = 1; //Allow the game to run
        //Disable pause menu elements on startup
        pausemenupanel.gameObject.SetActive(false);
        pausemenulogo.gameObject.SetActive(false);
        resumebtn.gameObject.SetActive(false);
        restartbtn.gameObject.SetActive(false);
        mainmenubtn.gameObject.SetActive(false);

        //Disable unlock popup elements on startup
        unlockpanel.gameObject.SetActive(false);
        unlocktext.text = "";

        //Create task listeners for all the pause buttons
        resumebtn.onClick.AddListener(ResumeBtnClicked);
        restartbtn.onClick.AddListener(RestartBtnClicked);
        mainmenubtn.onClick.AddListener(MainMenuBtnClicked);

        //Initialize sound
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences
        musicsound.volume = musicvolume; //Set the volume of the sound source
        selectsound.volume = soundeffectsvolume; //Set the volume of the sound source
        dashsound.volume = soundeffectsvolume; //Set the volume of the sound source
        diesound.volume = soundeffectsvolume; //Set the volume of the sound source
        divesound.volume = soundeffectsvolume; //Set the volume of the sound source
        powerupssound.volume = soundeffectsvolume; //Set the volume of the sound source
        unlocksound.volume = soundeffectsvolume; //Set the volume of the sound source
        jumpsound.volume = soundeffectsvolume; //Set the volume of the sound source
        throwsound.volume = soundeffectsvolume; //Set the volume of the sound source

        musicsound.Play(); //Play the supercalisoundtrack
        hasdieplayed = false; //Initialize a value that keeps track of whether the death sound has played to false
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.timeScale ==1) && (livesleft > 0))
        {
            HandleAnimation(); //Call function to handle the player character's animations
            PlayerMove(); //Call function to facilitate left-right movement
            PlayerDash(); //Call function to facilitate dash
            PlayerShoot(); //Call function to facilitate shooting
            PlayerJump(); //Call function to facilitate jumping
        }
        
        
        CheckGrounding(); //Call function to check if you are touching the ground
        HandleBounding(); //Call function to wrap player around if they go outside the camera width bounds
        HandleHits(); //Call function to handle hits to the player
        HandlePause(); //Call function to handle the pause menu. Placed here so I don't forget it 
        HandleHighScore(); //Call function to handle the high score constantly. This is done in order to preserve unlockables without incident
        HandleUnlockables(); //Unlock special items when the user surpasses their limits, and display the relevant popup

        if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
        {
            canshoot = true; //Muscle Machine has infinite dumbbell throw ability
        }

        //livesleft = 100;
    }

    void PlayerMove()
    {
        float xaxis = Input.GetAxisRaw("Horizontal"); //Get x-axis
        float xvelocity = xaxis * playerspeed; //Calculate velocity in the x direction
        player.velocity = new Vector2(xvelocity, player.velocity.y); //Update the player's velocity in the x-direction. Whatever the y velocity is, maintain it.

    }

    void PlayerDash()
    {
        //GET THE DIRECTION THE CHARACTER IS FACING
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) //If left-binded inputs are pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
        {
            isfaceright = false; //The character is facing left
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) //If right-binded inputs are pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
        {
            isfaceright = true; //The character is facing right
        }

        //IMPLEMENT DASH
        if (Input.GetKeyDown(KeyCode.Z) && (Time.time - lastdash >= dashconsecutivememory)) //If you press 'Z' and the fixed time between consecutive dashes has elapsed
        {
            dashsound.Play(); //Play the dash sound
            lastdash = Time.time; //Save the time of the last dash
            if ((isfaceright == true) && (Time.time - lastdash < dashconsecutivememory)) //If the player is facing the right of the screen and they are still within the dash time window
            {
                player.AddForce(new Vector2(dashmagnitudex, dashmagnitudey)); //Dash right
            }
            else if ((isfaceright == false) && (Time.time - lastdash < dashconsecutivememory)) //If the player is facing the left of the screen and they are still within the dash time window
            {
                player.AddForce(new Vector2(-dashmagnitudex, dashmagnitudey)); //Dash left
            }
        }

    }

    void PlayerShoot()
    {
        //IMPLEMENT SHOOT
        if (Input.GetKeyDown(KeyCode.C) && (canshoot == true) && (Time.time - lastshoot >= shootconsecutivememory)) //If you press 'Z' and the fixed time between consecutive shoot events has elapsed
        {
            throwsound.Play(); //Play the throw sound
            lastshoot = Time.time; //Save the time of the last shoot
            if ((isfaceright == true) && (Time.time == lastshoot)) //If the player is facing the right of the screen and they are still within the shoot time window
            {
                //Shoot right
                //If the index that keeps track of all the bullets in the scene goes out of bounds
                if (powerupsscript.bulletindex >= powerupsscript.bulletamount)
                {
                    powerupsscript.bulletindex = 0; //Reset the index that keeps track of all the bullets in the scene
                }

                //Spawn a new bullet instance
                powerupsscript.bulletcopy[powerupsscript.bulletindex] = PowerUps.SharedInstance.GetPooledBullet(); //Get pooled bullets
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex] = PowerUps.SharedInstance.GetPooledBulletCollisionBound(); //Get pooled bullet collision bounds
                //Set bullet spawn position
                float xloc = player.transform.position.x + (player.transform.localScale.x/2);
                float yloc = player.transform.position.y;
                Vector2 spawnpos = new Vector2(xloc, yloc);
                powerupsscript.bulletcopy[powerupsscript.bulletindex].transform.position = spawnpos;
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].transform.position = spawnpos;

                powerupsscript.bulletcopy[powerupsscript.bulletindex].SetActive(true); //Spawn the bullet
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].SetActive(true); //Spawn the bullet collision bound

                powerupsscript.bulletanimator[powerupsscript.bulletindex] = powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Animator>(); //Bind animator component of the owner to the specified variable

                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].transform.parent = powerupsscript.bulletcopy[powerupsscript.bulletindex].transform; //Set the object instance as the parent of the collision bound instance

                //Re-enable physics. This has to be done explicitly since we disable physics for each instance when we destroy it to allow time for the animation.
                powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().simulated = true; //Enable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().WakeUp(); //Enable rigidbody of the instance

                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].GetComponent<Collider2D>().isTrigger = true;
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].tag = "bulletcollision";

                powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().velocity = new Vector2(bulletspeed, 4f); //Give the bullet an initial velocity

                powerupsscript.isbulletdead[powerupsscript.bulletindex] = false;

                powerupsscript.bulletindex = powerupsscript.bulletindex + 1; //Increment the index that keeps track of all the bullets in the scene

            }
            else if ((isfaceright == false) && (Time.time == lastshoot)) //If the player is facing the left of the screen and they are still within the shoot time window
            {
                //Shoot left
                //If the index that keeps track of all the bullets in the scene goes out of bounds
                if (powerupsscript.bulletindex >= powerupsscript.bulletamount)
                {
                    powerupsscript.bulletindex = 0; //Reset the index that keeps track of all the bullets in the scene
                }

                //Spawn a new bullet instance
                powerupsscript.bulletcopy[powerupsscript.bulletindex] = PowerUps.SharedInstance.GetPooledBullet(); //Get pooled bullets
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex] = PowerUps.SharedInstance.GetPooledBulletCollisionBound(); //Get pooled bullet collision bounds
                //Set bullet spawn position
                float xloc = player.transform.position.x - (player.transform.localScale.x/2);
                float yloc = player.transform.position.y;
                Vector2 spawnpos = new Vector2(xloc, yloc);
                powerupsscript.bulletcopy[powerupsscript.bulletindex].transform.position = spawnpos;
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].transform.position = spawnpos;

                powerupsscript.bulletcopy[powerupsscript.bulletindex].SetActive(true); //Spawn the bullet
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].SetActive(true); //Spawn the bullet collision bound

                powerupsscript.bulletanimator[powerupsscript.bulletindex] = powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Animator>(); //Bind animator component of the owner to the specified variable

                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].transform.parent = powerupsscript.bulletcopy[powerupsscript.bulletindex].transform; //Set the object instance as the parent of the collision bound instance

                //Re-enable physics. This has to be done explicitly since we disable physics for each instance when we destroy it to allow time for the animation.
                powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().simulated = true; //Enable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().WakeUp(); //Enable rigidbody of the instance

                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].GetComponent<Collider2D>().isTrigger = true;
                powerupsscript.bulletcollisionboundcopy[powerupsscript.bulletindex].tag = "bulletcollision";

                powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletspeed, 4f); //Give the bullet an initial velocity

                powerupsscript.isbulletdead[powerupsscript.bulletindex] = false;

                powerupsscript.bulletindex = powerupsscript.bulletindex + 1; //Increment the index that keeps track of all the bullets in the scene

            }
        }

    }

    void PlayerJump()
    {
        //SIMPLE JUMP IMPLEMENTATION
        if (Input.GetKeyDown(KeyCode.Space) && (isgrounded==true || (Time.time - lastground <= groundmemory) || (extrajumps > 0))) //If 'SPACE' is pressed and the player is grounded
        {
            jumpsound.Play(); //Play the jump sound
            player.velocity = new Vector2(player.velocity.x, jumpspeed); //When 'SPACE' is pressed, jump
            extrajumps = extrajumps - 1; //Decrement the number of extra jumps
        }

        //CHECK IF THE PLAYER WANTS TO DIVE
        if (Input.GetKeyDown(KeyCode.X) && (isdiving == false) && (isgrounded == false)) //If the 'X' button is pressed, and the player is not already diving, and they are in mid-air
        {
            divesound.Play(); //Play the dive sound
            isdiving = true; 
        }

        //IMPLEMENT DIVE PHYSICS
        if (isdiving == true) //If you are diving.
        {
            player.velocity += (Physics2D.gravity * (divemultiplier - 1) * Time.deltaTime) * Vector2.up;

            Collider2D bakerdivecollider = Physics2D.OverlapCircle(groundsensor.position, groundradius, bakerlayer);
            if (bakerdivecollider != null) //If the collider at the bottom of the player's bounds detects the baker
            {
                bakermovementscript.PlayBakerDeath(); //Call the function that plays the baker death animation
                                                      //Execute a delay, then delete the baker instance elements
                bakermovementscript.bakercopy[bakermovementscript.poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                bakermovementscript.bakercopy[bakermovementscript.poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance
                Invoke("HandleBakerDeath", deathinterval); //Invoke the function 'HandleBakerDeath' once with the same 'deathinterval' as the player character

            }
            
        }
        //FACILITATE SMOOTHING OF JUMP PHYSICS. YOU WANT TO FALL SLIGHTLY FASTER THAN YOU JUMPED, TO IMPROVE MANOEAUVERABILITY
        else if (player.velocity.y < 0) //If you are falling. When the y-velocity vector is negative, you are falling.
        {
            player.velocity += (Physics2D.gravity * (fallmultiplier - 1) * Time.deltaTime) * Vector2.up;
        }
        else if (player.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) //If you have jumped. When the y-velocity vector is positive and the space button has been released, you have jumped.
        {
            player.velocity += (Physics2D.gravity * (jumpmultiplier - 1) * Time.deltaTime) * Vector2.up;
        }

    }

    void CheckGrounding()
    {
        Collider2D collider = Physics2D.OverlapCircle(groundsensor.position, groundradius, groundlayer);
        if (collider != null) //If the collider detects ground
        {
            isgrounded = true; //Set the boolean value of 'isgrounded' to true
            isdiving = false; //Set the boolean value of 'isdiving' to false
            extrajumps = defaultextrajumps; //Reset the multi-jump variable
        }
        else //If technically no longer grounded
        {
            if (isgrounded == true)
            {
                lastground = Time.time; //Save the time of the last time the player touched the ground
            }
            isgrounded = false;
        }

    }

    void HandleBounding()
    {
        if (player.transform.position.x <= Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - playermargin) //If the player goes past the left-most camera bound
        {
            float xloc = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + playermargin - 0.1f; //Set the respawn x-location off-screen to the right, offset by a safety margin to prevent pinballing between camera-bound respawn positions
            float yloc = player.transform.position.y; //Maintain y-value of the player
            Vector2 spawnpos = new Vector2(xloc, yloc);
            player.transform.position = spawnpos;
        }
        else if (player.transform.position.x >= Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + playermargin) //If the player goes past the right-most camera bound
        {
            float xloc = Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x - playermargin + 0.1f; //Set the respawn x-location off-screen to the left, offset by a safety margin to prevent pinballing between camera-bound respawn positions
            float yloc = player.transform.position.y; //Maintain y-value of the player
            Vector2 spawnpos = new Vector2(xloc, yloc);
            player.transform.position = spawnpos;

        }

    }

    void HandleHits()
    {
        if (livesleft <= 0) //If the player is out of lives
        {
            //HandleHighScore(); //Handle the player's high score upon death
            livesleft = 0; //Maintain 'livesleft' at zero

            //Play the death sound only once
            if (hasdieplayed is false)
            {
                diesound.Play(); //Play the player death sound
                hasdieplayed = true; //Set a value that keeps track of whether the death sound has played to true
            }

            //Play the skin specific death animation
            if (currentskin == 0) //Rex
            {
                playeranimator.Play("rex_death"); //Play the death animation
            }
            else if (currentskin == 1) //Dom
            {
                playeranimator.Play("dom_death"); //Play the death animation
            }
            else if (currentskin == 2) //Nikki
            {
                playeranimator.Play("nikki_death"); //Play the death animation
            }
            else if (currentskin == 3) //Tumi
            {
                playeranimator.Play("tumi_death"); //Play the death animation
            }
            else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
            {
                playeranimator.Play("pablo_death"); //Play the death animation
            }
            else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
            {
                playeranimator.Play("lisa_death"); //Play the death animation
            }
            else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
            {
                playeranimator.Play("mm_death"); //Play the death animation
            }
            else
            {
                playeranimator.Play("rex_death"); //Play the death animation
            }

            player.simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
            player.Sleep(); //Disable rigidbody of the player
            //player.isKinematic = true; //Disable physics of the player
            Invoke("ResetScene", deathinterval); //Invoke the function 'ResetScene' once after initial time 'deathinterval'
            //Play Death Animation

        }
    }

    void HandleAnimation()
    {
        if (livesleft > 0) //If the player hasn't died yet
        {
            //Running
            if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && (isgrounded is true)) //If right-binded inputs are pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_run_right"); //Play the run right animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_run_right"); //Play the run right animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_run_right"); //Play the run right animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_run_right"); //Play the run right animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_run_right"); //Play the run right animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_run_right"); //Play the run right animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_run_right"); //Play the run right animation
                }
                else //Handle locked characters by defaulting to Rex
                {
                    playeranimator.Play("rex_run_right"); //Play the run right animation
                }

            }
            else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && (isgrounded is true)) //If left-binded inputs are pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_run_left"); //Play the run left animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_run_left"); //Play the run left animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_run_left"); //Play the run left animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_run_left"); //Play the run left animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_run_left"); //Play the run left animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_run_left"); //Play the run left animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_run_left"); //Play the run left animation
                }
                else
                {
                    playeranimator.Play("rex_run_left"); //Play the run left animation
                }
            }
            else if ((isdiving is true) && (isfaceright is true)) //If the player is diving right
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_dive_right"); //Play the dive right animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_dive_right"); //Play the dive right animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_dive_right"); //Play the dive right animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_dive_right"); //Play the dive right animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_dive_right"); //Play the dive right animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_dive_right"); //Play the dive right animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_dive_right"); //Play the dive right animation
                }
                else
                {
                    playeranimator.Play("rex_dive_right"); //Play the dive right animation
                }
            }
            else if ((isdiving is true) && (isfaceright is false)) //If the player is diving left
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_dive_left"); //Play the dive left animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_dive_left"); //Play the dive left animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_dive_left"); //Play the dive left animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_dive_left"); //Play the dive left animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_dive_left"); //Play the dive left animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_dive_left"); //Play the dive left animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_dive_left"); //Play the dive left animation
                }
                else
                {
                    playeranimator.Play("rex_dive_left"); //Play the dive left animation
                }
            }
            else if ((isgrounded is false) && (isfaceright is true)) //If the player is simply falling right
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_jump_right"); //Play the jump right animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_jump_right"); //Play the jump right animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_jump_right"); //Play the jump right animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_jump_right"); //Play the jump right animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_jump_right"); //Play the jump right animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_jump_right"); //Play the jump right animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_jump_right"); //Play the jump right animation
                }
                else
                {
                    playeranimator.Play("rex_jump_right"); //Play the jump right animation
                }

            }
            else if ((isgrounded is false) && (isfaceright is false)) //If the player is simply falling left
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_jump_left"); //Play the jump left animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_jump_left"); //Play the jump left animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_jump_left"); //Play the jump left animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_jump_left"); //Play the jump left animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_jump_left"); //Play the jump left animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_jump_left"); //Play the jump left animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_jump_left"); //Play the jump left animation
                }
                else
                {
                    playeranimator.Play("rex_jump_left"); //Play the jump left animation
                }

            }
            else if (isfaceright is true) //If the player is facing right
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_idle_right"); //Play the idle right animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_idle_right"); //Play the idle right animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_idle_right"); //Play the idle right animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_idle_right"); //Play the idle right animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_idle_right"); //Play the idle right animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_idle_right"); //Play the idle right animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_idle_right"); //Play the idle right animation
                }
                else
                {
                    playeranimator.Play("rex_idle_right"); //Play the idle right animation
                }

            }
            else if (isfaceright is false) //If the player is facing left
            {
                if (currentskin == 0) //Rex
                {
                    playeranimator.Play("rex_idle_left"); //Play the idle left animation
                }
                else if (currentskin == 1) //Dom
                {
                    playeranimator.Play("dom_idle_left"); //Play the idle left animation
                }
                else if (currentskin == 2) //Nikki
                {
                    playeranimator.Play("nikki_idle_left"); //Play the idle left animation
                }
                else if (currentskin == 3) //Tumi
                {
                    playeranimator.Play("tumi_idle_left"); //Play the idle left animation
                }
                else if ((currentskin == 4) && (ispablounlocked == 1)) //Pablo
                {
                    playeranimator.Play("pablo_idle_left"); //Play the idle left animation
                }
                else if ((currentskin == 5) && (islisaunlocked == 1)) //Lisa
                {
                    playeranimator.Play("lisa_idle_left"); //Play the idle left animation
                }
                else if ((currentskin == 6) && (ismmunlocked == 1)) //Muscle Machine
                {
                    playeranimator.Play("mm_idle_left"); //Play the idle left animation
                }
                else
                {
                    playeranimator.Play("rex_idle_left"); //Play the idle left animation
                }

            }
        }


    }


    void HandlePause()
    {
        //HANDLE PAUSE MENU IF THE 'ESC' KEY IS PRESSED
        if (Input.GetKeyDown(KeyCode.Escape)) //If the 'ESC' key is pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
        {
            selectsound.Play(); //Play the menu select sound
            if (Time.timeScale == 0) //If the game is already paused, resume it
            {
                //Update pause menu variables
                Time.timeScale = 1; //Resume the game
                musicsound.UnPause(); //Resume the music
                //Disable the mouse cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //Disable pause menu elements
                pausemenupanel.gameObject.SetActive(false);
                pausemenulogo.gameObject.SetActive(false);
                resumebtn.gameObject.SetActive(false);
                restartbtn.gameObject.SetActive(false);
                mainmenubtn.gameObject.SetActive(false);
            }
            else if (Time.timeScale == 1) //If the game is running, pause it
            {
                //Update pause menu variables
                Time.timeScale = 0; //Pause the game
                musicsound.Pause(); //Pause the music
                //Enable the mouse cursor
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                //Enable pause menu elements
                pausemenupanel.gameObject.SetActive(true);
                pausemenulogo.gameObject.SetActive(true);
                resumebtn.gameObject.SetActive(true);
                restartbtn.gameObject.SetActive(true);
                mainmenubtn.gameObject.SetActive(true);
            }

        }

        if (Time.timeScale == 0) //If the game is paused
        {
            //If any relevant navigation keys up, down, W, S, Enter, or Space are pressed, play a sound
            if ((Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.DownArrow)) || (Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.Space)))
            {
                selectsound.Play(); //Play the menu select sound

            }
        }
        
    }


    // Pause menu item, Resume the scene
    private void ResumeBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        //Update pause menu variables
        Time.timeScale = 1; //Resume the game
        musicsound.UnPause(); //Resume the music
        //Disable the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Disable pause menu elements
        pausemenupanel.gameObject.SetActive(false);
        pausemenulogo.gameObject.SetActive(false);
        resumebtn.gameObject.SetActive(false);
        restartbtn.gameObject.SetActive(false);
        mainmenubtn.gameObject.SetActive(false);

    }


    // Pause menu item, Reset the scene
    private void RestartBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        //Update pause menu variables
        Time.timeScale = 1; //Resume the game
        musicsound.Play(); //Restart the music
        //Disable the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Disable pause menu elements
        pausemenupanel.gameObject.SetActive(false);
        pausemenulogo.gameObject.SetActive(false);
        resumebtn.gameObject.SetActive(false);
        restartbtn.gameObject.SetActive(false);
        mainmenubtn.gameObject.SetActive(false);
        Invoke("RestartAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
        
    }


    // Pause menu item, Navigate to main menu. The high score will not be handled in the case of the player exiting manually instead of dying
    private void MainMenuBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        Time.timeScale = 1; //We do this not to resume the game, but to allow scene changing again once a return to the main menu is executed.
        //Enable the mouse cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Invoke("MainMenuAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play

    }


    // Reset the scene after death
    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }

    // Handle the player's high score upon death
    void HandleHighScore()
    {
        //Get the current score of the player and assign it to the variable 'currscore'
        if (hudscript.ScoreText.text != null) //This 'if' statement was required to cover up for a null string error being received
        {
            currscore = Int32.Parse(hudscript.ScoreText.text); //Get the player's current score
        }
        

        if (PlayerPrefs.GetInt("highscorevalue") != 0) //If the high score has been set, and the player preferences item is not empty
        {
            int highscore = PlayerPrefs.GetInt("highscorevalue");
            if (currscore > highscore) //If the player's current score is greater than the currently saved high score
            {
                PlayerPrefs.SetInt("highscorevalue", currscore); //Save the new high score to player preferences
            }
            else
            {
                //DO NOTHING if the current score is less than the high score
            }

        }
        else //If the high score has not yet been set
        {
            PlayerPrefs.SetInt("highscorevalue", currscore); //Save the new high score to player preferences
        }

    }

    void HandleUnlockables()
    {
        //Get the current score of the player and assign it to the variable 'currscore'
        if (hudscript.ScoreText.text != null) //This 'if' statement was required to cover up for a null string error being received
        {
            currscore = Int32.Parse(hudscript.ScoreText.text); //Get the player's current score
        }

        //Unlockable skins
        if ((currscore >= 50) && (ispablounlocked == 0)) //If the player surpasses their limits and the item has not yet been unlocked
        {
            unlocksound.Play(); //Play the unlock item sound
            ispablounlocked = 1; //Update the state of an unlockable item
            PlayerPrefs.SetInt("ispablounlocked", ispablounlocked); //Update player preferences
            unlockpanel.gameObject.SetActive(true); //Make the unlock panel visible
            unlocktext.text = "New Character Unlocked";
            Invoke("HideUnlockPanel", (deathinterval/1.3f)); //Invoke the function 'HideUnlockPanel' once after initial time 'deathinterval/2'
        }
        if ((currscore >= 100) && (islisaunlocked == 0)) //If the player surpasses their limits and the item has not yet been unlocked
        {
            unlocksound.Play(); //Play the unlock item sound
            islisaunlocked = 1; //Update the state of an unlockable item
            PlayerPrefs.SetInt("islisaunlocked", islisaunlocked); //Update player preferences
            unlockpanel.gameObject.SetActive(true); //Make the unlock panel visible
            unlocktext.text = "New Character Unlocked";
            Invoke("HideUnlockPanel", (deathinterval / 1.3f)); //Invoke the function 'HideUnlockPanel' once after initial time 'deathinterval/2'
        }
        if ((currscore >= 150) && (ismmunlocked == 0)) //If the player surpasses their limits and the item has not yet been unlocked
        {
            unlocksound.Play(); //Play the unlock item sound
            ismmunlocked = 1; //Update the state of an unlockable item
            PlayerPrefs.SetInt("ismmunlocked", ismmunlocked); //Update player preferences
            unlockpanel.gameObject.SetActive(true); //Make the unlock panel visible
            unlocktext.text = "New Character Unlocked";
            Invoke("HideUnlockPanel", (deathinterval / 1.3f)); //Invoke the function 'HideUnlockPanel' once after initial time 'deathinterval/2'
        }

        //Unlockable maps
        if ((currscore >= 60) && (isbeachunlocked == 0)) //If the player surpasses their limits and the item has not yet been unlocked
        {
            unlocksound.Play(); //Play the unlock item sound
            isbeachunlocked = 1; //Update the state of an unlockable item
            PlayerPrefs.SetInt("isbeachunlocked", isbeachunlocked); //Update player preferences
            unlockpanel.gameObject.SetActive(true); //Make the unlock panel visible
            unlocktext.text = "New Map Unlocked";
            Invoke("HideUnlockPanel", (deathinterval / 1.3f)); //Invoke the function 'HideUnlockPanel' once after initial time 'deathinterval/2'
        }
        if ((currscore >= 120) && (isabalancheunlocked == 0)) //If the player surpasses their limits and the item has not yet been unlocked
        {
            unlocksound.Play(); //Play the unlock item sound
            isabalancheunlocked = 1; //Update the state of an unlockable item
            PlayerPrefs.SetInt("isabalancheunlocked", isabalancheunlocked); //Update player preferences
            unlockpanel.gameObject.SetActive(true); //Make the unlock panel visible
            unlocktext.text = "New Map Unlocked";
            Invoke("HideUnlockPanel", (deathinterval / 1.3f)); //Invoke the function 'HideUnlockPanel' once after initial time 'deathinterval/2'
        }

    }

    private void HideUnlockPanel()
    {
        //Disable unlock panel elements
        unlockpanel.gameObject.SetActive(false);
        unlocktext.text = "";
    }


    private void HandleBakerDeath()
    {
        bakermovementscript.bakercopy[bakermovementscript.poolindex].SetActive(false); //Remove instance
        bakermovementscript.bakercopy[bakermovementscript.poolindex] = null; //Clear array slot
        bakermovementscript.bakercollisionboundcopy[bakermovementscript.poolindex].SetActive(false); //Remove instance
        bakermovementscript.bakercollisionboundcopy[bakermovementscript.poolindex] = null; //Clear array slot

    }


    private void RestartAction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reset the scene

    }

    private void MainMenuAction()
    {
        SceneManager.LoadScene("MainMenu"); //Load the main menu

    }


    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }




}
