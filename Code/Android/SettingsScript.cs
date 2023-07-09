using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    public Button controlsbtn; //Declare a new UI Button that will be used to find and store the controls button
    public Button resetbtn; //Declare a new UI Button that will be used to find and store the reset settings button
    public Button backbtn; //Declare a new UI Button that will be used to find and store the back button
    public Slider musicslider; //Declare a new UI Slider to find and store the music volume slider
    public Slider soundeffectsslider; //Declare a new UI Slider to find and store the music volume slider

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource selectsound; //Declare a new sound source

    public Button mapleftbtn; //Declare a new UI Button that will be used to find and store the map left button
    public Button maprightbtn; //Declare a new UI Button that will be used to find and store the map right button
    public Text mapcurrenttext; //Declare a new UI Text item that will be used to find and store the current map text
    public int numberofmaps = 3; //Declare the maximum number of maps
    public int currentmap = 0; //Declare an integer variable to keep track of which map is currently selected

    public Button skinleftbtn; //Declare a new UI Button that will be used to find and store the skin left button
    public Button skinrightbtn; //Declare a new UI Button that will be used to find and store the skin right button
    public Text skincurrenttext; //Declare a new UI Text item that will be used to find and store the current skin text
    public Text bufftext; //Declare a new UI Text item that will be used to find and store the character buff text
    public Image skincurrentimage; //Declare a new UI Image item that will be used to find and store the current skin image
    public int numberofskins = 7; //Declare the maximum number of skins
    public int currentskin = 0; //Declare an integer variable to keep track of which skin is currently selected

    public int isbeachunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int isabalancheunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int ispablounlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int islisaunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int ismmunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.

    //Declare sprites
    public Sprite pureearth;
    public Sprite pureearth2;
    public Sprite beachearth;
    public Sprite ice;
    public Sprite ice2;

    public Sprite rexsprite;
    public Sprite domsprite;
    public Sprite pablosprite;
    public Sprite pablolockedsprite;
    public Sprite nikkisprite;
    public Sprite lisasprite;
    public Sprite lisalockedsprite;
    public Sprite tumisprite;
    public Sprite mmsprite;
    public Sprite mmlockedsprite;

    //Declare the 36 visible ground tiles on the screen so maps can be changed
    public GameObject tile1;
    public GameObject tile2;
    public GameObject tile3;
    public GameObject tile4;
    public GameObject tile5;
    public GameObject tile6;
    public GameObject tile7;
    public GameObject tile8;
    public GameObject tile9;
    public GameObject tile10;
    public GameObject tile11;
    public GameObject tile12;
    public GameObject tile13;
    public GameObject tile14;
    public GameObject tile15;
    public GameObject tile16;
    public GameObject tile17;
    public GameObject tile18;
    public GameObject tile19;
    public GameObject tile20;
    public GameObject tile21;
    public GameObject tile22;
    public GameObject tile23;
    public GameObject tile24;
    public GameObject tile25;
    public GameObject tile26;
    public GameObject tile27;
    public GameObject tile28;
    public GameObject tile29;
    public GameObject tile30;
    public GameObject tile31;
    public GameObject tile32;
    public GameObject tile33;
    public GameObject tile34;
    public GameObject tile35;
    public GameObject tile36;

    //Declare the scenery tiles on the screen so maps can be changed
    public GameObject hilltile1;
    public GameObject hilltile2;
    public GameObject abtile1;
    public GameObject abtile2;

    //Declare the 20 beach ocean tiles. Visibility for these will be toggled depending on which map is selected
    public GameObject ocean1;
    public GameObject ocean2;
    public GameObject ocean3;
    public GameObject ocean4;
    public GameObject ocean5;
    public GameObject ocean6;
    public GameObject ocean7;
    public GameObject ocean8;
    public GameObject ocean9;
    public GameObject ocean10;
    public GameObject ocean11;
    public GameObject ocean12;
    public GameObject ocean13;
    public GameObject ocean14;
    public GameObject ocean15;
    public GameObject ocean16;
    public GameObject ocean17;
    public GameObject ocean18;
    public GameObject ocean19;
    public GameObject ocean20;


    // Awake allows declaration or initialization of variables before the program runs. 
    void Awake()
    {
        LoadMap(); //Call a function to load the map. A separate function is required since it is called more than once in the code.

    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false; //Make the connected item, 'mainground', invisible

        //Create task listeners for all the settings menu buttons
        controlsbtn.onClick.AddListener(ControlsBtnClicked);
        resetbtn.onClick.AddListener(ResetBtnClicked);
        backbtn.onClick.AddListener(BackBtnClicked);
        mapleftbtn.onClick.AddListener(MapLeftBtnClicked);
        maprightbtn.onClick.AddListener(MapRightBtnClicked);
        skinleftbtn.onClick.AddListener(SkinLeftBtnClicked);
        skinrightbtn.onClick.AddListener(SkinRightBtnClicked);

        LoadSettings(); //Execute function that gets all the currently saved settings from the player preferences

        selectsound.volume = soundeffectsvolume; //Set the volume of the sound source

    }

    // Update is called once per frame
    void Update()
    {
        //Update changes
        SaveSettings(); //Save settings constantly. Thus, everytime  you make a change they will be updated.
        LoadSettings(); //Load settings constantly, specifically in order to update the musicvolume and soundeffectsvolume variables within the same screen event
        selectsound.volume = soundeffectsvolume; //Update the sound source volume constantly, specifically in order to update it within the same screen event

        //If 'ESC' is pressed on the settings screen
        if (Input.GetKeyDown(KeyCode.Escape)) //If the 'ESC' key is pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
        {
            selectsound.Play(); //Play the menu select sound

            Invoke("BackAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play

        }

        //If any relevant navigation keys up, down, W, S, Enter, or Space are pressed, play a sound
        if ((Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.DownArrow)) || (Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.Space)))
        {
            selectsound.Play(); //Play the menu select sound

        }

    }

    void ControlsBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound

        Invoke("ControlsAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }

    void ResetBtnClicked()
    {
        //PlayerPrefs.DeleteAll(); //Delete all entries in player preferences. We do essentially the same thing below, but it is done again here to make sure the keys are working as intended from scratch.
        //The above line had to be edited out in order to avoid resetting the EULA when the player clears their settings

        //Reset all skin settings 
        currentskin = 0; //Reset the current skin to Rex
        PlayerPrefs.SetInt("currentskin", 0); //Reset the current skin to Rex in player preferences
        PlayerPrefs.SetInt("ispablounlocked", 0); //Reset unlockable skin 
        PlayerPrefs.SetInt("islisaunlocked", 0); //Reset unlockable skin
        PlayerPrefs.SetInt("ismmunlocked", 0); //Reset unlockable skin

        //Reset all map settings
        currentmap = 0; //Reset the current map to Protein Park
        PlayerPrefs.SetInt("currentmap", 0); //Reset the current map to Protein Park in player preferences
        PlayerPrefs.SetInt("isbeachunlocked", 0); //Reset unlockable map
        PlayerPrefs.SetInt("isabalancheunlocked", 0); //Reset unlockable map

        //Reset sound settings
        PlayerPrefs.SetFloat("musicvolume", 1f); //Reset the music volume to its default value
        musicslider.value = 1f; //Reset the music slider
        PlayerPrefs.SetFloat("soundeffectsvolume", 1f); //Reset the sound effects volume to its default value
        soundeffectsslider.value = 1f; //Reset the sound effects slider

        selectsound.volume = soundeffectsvolume; //Update the sound source volume constantly, specifically in order to update it within the same screen event

        //Reset the high score
        PlayerPrefs.SetInt("highscorevalue", 0); //Set the player preferences item 'highscorevalue' to 0

        //Play the menu sound effect after updating the volume
        selectsound.Play(); //Play the menu select sound
    }

    void BackBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound

        Invoke("BackAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }


    void SaveSettings()
    {
        //Save skin settings
        PlayerPrefs.SetInt("currentskin", currentskin); //Save the current skin to the player preferences

        //Save map settings
        PlayerPrefs.SetInt("currentmap", currentmap); //Save the current map to the player preferences

        //Save volume settings
        PlayerPrefs.SetFloat("musicvolume", musicslider.value); //Save the music slider value to the player preferences
        PlayerPrefs.SetFloat("soundeffectsvolume", soundeffectsslider.value); //Save the sound effects slider value to the player preferences

    }


    void LoadSettings()
    {
        //Load skins settings
        currentskin = PlayerPrefs.GetInt("currentskin", 0); //Get the current skin from player preferences. '0' for Protein Park, '1' for Bicep Beach, '2' for Abalanche
        ispablounlocked = PlayerPrefs.GetInt("ispablounlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        islisaunlocked = PlayerPrefs.GetInt("islisaunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        ismmunlocked = PlayerPrefs.GetInt("ismmunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.

        if (currentskin == 0)
        {
            skincurrentimage.GetComponent<Image>().sprite = rexsprite;
            skincurrenttext.text = "Rex";
            bufftext.text = ""; //Make the character buff text invisible
        }
        else if (currentskin == 1)
        {
            skincurrentimage.GetComponent<Image>().sprite = domsprite;
            skincurrenttext.text = "Dom";
            bufftext.text = ""; //Make the character buff text invisible
        }
        else if (currentskin == 2)
        {
            skincurrentimage.GetComponent<Image>().sprite = nikkisprite;
            skincurrenttext.text = "Nikki";
            bufftext.text = ""; //Make the character buff text invisible
        }
        else if (currentskin == 3)
        {
            skincurrentimage.GetComponent<Image>().sprite = tumisprite;
            skincurrenttext.text = "Tumi";
            bufftext.text = ""; //Make the character buff text invisible
        }
        else if ((currentskin == 4) && (ispablounlocked == 0)) //If Pablo is locked
        {
            skincurrentimage.GetComponent<Image>().sprite = pablolockedsprite;
            skincurrenttext.text = "???";
            bufftext.text = "(Unlock at 50 pts)"; //Set the character buff text
        }
        else if ((currentskin == 4) && (ispablounlocked == 1)) //If Pablo is unlocked
        {
            skincurrentimage.GetComponent<Image>().sprite = pablosprite;
            skincurrenttext.text = "Pablo";
            bufftext.text = "(Speed X2)"; //Set the character buff text
        }
        else if ((currentskin == 5) && (islisaunlocked == 0)) //If Lisa is locked
        {
            skincurrentimage.GetComponent<Image>().sprite = lisalockedsprite;
            skincurrenttext.text = "???";
            bufftext.text = "(Unlock at 100 pts)"; //Set the character buff text
        }
        else if ((currentskin == 5) && (islisaunlocked==1)) //If Lisa is unlocked
        {
            skincurrentimage.GetComponent<Image>().sprite = lisasprite;
            skincurrenttext.text = "Lisa";
            bufftext.text = "(Triple Jump)"; //Set the character buff text
        }
        else if ((currentskin == 6) && (ismmunlocked == 0)) //If Muscle Machine is locked
        {
            skincurrentimage.GetComponent<Image>().sprite = mmlockedsprite;
            skincurrenttext.text = "???";
            bufftext.text = "(Unlock at 150 pts)"; //Set the character buff text
        }
        else if ((currentskin == 6) && (ismmunlocked ==1)) //If Muscle Machine is unlocked
        {
            skincurrentimage.GetComponent<Image>().sprite = mmsprite;
            skincurrenttext.text = "Muscle Machine";
            bufftext.text = "(Infinite Ammo)"; //Set the character buff text
        }

        //Load map settings
        currentmap = PlayerPrefs.GetInt("currentmap", 0); //Get the current map from player preferences. '0' for Protein Park, '1' for Bicep Beach, '2' for Abalanche
        isbeachunlocked = PlayerPrefs.GetInt("isbeachunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        isabalancheunlocked = PlayerPrefs.GetInt("isabalancheunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        if (currentmap == 0)
        {
            mapcurrenttext.text = "Protein Park";
        }
        else if ((currentmap == 1) && (isbeachunlocked == 0)) //If Bicep Beach is locked
        {
            mapcurrenttext.text = "Unlock at 60 pts";
        }
        else if ((currentmap == 1) && (isbeachunlocked == 1)) //If Bicep Beach is unlocked
        {
            mapcurrenttext.text = "Bicep Beach";
        }
        else if ((currentmap == 2) && (isabalancheunlocked == 0)) //If Abalanche is locked
        {
            mapcurrenttext.text = "Unlock at 120 pts";
        }
        else if ((currentmap == 2) && (isabalancheunlocked == 1)) //If Abalanche is unlocked
        {
            mapcurrenttext.text = "Abalanche";
        }
        LoadMap(); //Call a function to load the map. We do this so the map can update immediately as the player cycles through the map options.

        //Load volume settings
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        musicslider.value = musicvolume; //Make the music slider reflect the actual value stored in player preferences

        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsslider.value = soundeffectsvolume; //Make the sound effects slider reflect the actual value stored in player preferences

    }

    private void ControlsAction()
    {
        //Handle locked skins
        if (((currentskin == 4) && (ispablounlocked == 0)) || ((currentskin == 5) && (islisaunlocked == 0)) || ((currentskin == 6) && (ismmunlocked == 0)))
        {
            currentskin = 0; //Reset the current skin to Rex
            PlayerPrefs.SetInt("currentskin", 0); //Reset the current skin to Rex in player preferences

        }
        //Handle locked maps
        if (((currentmap == 1) && (isbeachunlocked == 0)) || ((currentmap == 2) && (isabalancheunlocked == 0)))
        {
            currentmap = 0; //Reset the current map to Protein Park
            PlayerPrefs.SetInt("currentmap", 0); //Reset the current map to Protein Park in player preferences
        }

        //Navigate to new screen
        SceneManager.LoadScene("Controls"); //Load the controls screen 
    }

    private void BackAction()
    {
        //Handle locked skins
        if (((currentskin == 4) && (ispablounlocked == 0)) || ((currentskin == 5) && (islisaunlocked == 0)) || ((currentskin == 6) && (ismmunlocked == 0)))
        {
            currentskin = 0; //Reset the current skin to Rex
            PlayerPrefs.SetInt("currentskin", 0); //Reset the current skin to Rex in player preferences

        }
        //Handle locked maps
        if (((currentmap == 1) && (isbeachunlocked == 0)) || ((currentmap == 2) && (isabalancheunlocked == 0)))
        {
            currentmap = 0; //Reset the current map to Protein Park
            PlayerPrefs.SetInt("currentmap", 0); //Reset the current map to Protein Park in player preferences
        }

        //Navigate to new screen
        SceneManager.LoadScene("MainMenu"); //Load the main menu screen 
    }


    void MapLeftBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound

        if (currentmap <= 0) //If trying to go out of bounds
        {
            currentmap = numberofmaps - 1; //Wrap the map around if you are at the left-most map. Recall that for a certain 'numberofmaps', 0 <= currentmap <= numberofmaps - 1 
        }
        else if (currentmap > 0) //Else if, still operating within bounds
        {
            currentmap = currentmap - 1; //Move left through maps
        }
        
    }

    void MapRightBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound

        if (currentmap >= numberofmaps - 1) //If trying to go out of bounds
        {
            currentmap = 0; //Wrap the map around if you are at the right-most map. Recall that for a certain 'numberofmaps', 0 <= currentmap <= numberofmaps - 1 
        }
        else if (currentmap < numberofmaps - 1) //Else if, still operating within bounds
        {
            currentmap = currentmap + 1; //Move right through maps
        }

    }

    void SkinLeftBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound

        if (currentskin <= 0) //If trying to go out of bounds
        {
            currentskin = numberofskins - 1; //Wrap the skin around if you are at the left-most skin. Recall that for a certain 'numberofskins', 0 <= currentskin <= numberofskins - 1 
        }
        else if (currentskin > 0) //Else if, still operating within bounds
        {
            currentskin = currentskin - 1; //Move left through skins
        }

    }

    void SkinRightBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound

        if (currentskin >= numberofskins - 1) //If trying to go out of bounds
        {
            currentskin = 0; //Wrap the skin around if you are at the right-most skin. Recall that for a certain 'numberofskins', 0 <= currentskin <= numberofskins - 1 
        }
        else if (currentskin < numberofskins - 1) //Else if, still operating within bounds
        {
            currentskin = currentskin + 1; //Move right through skins
        }

    }

    void LoadMap()
    {
        currentmap = PlayerPrefs.GetInt("currentmap", 0); //Get the current map from player preferences. '0' for Protein Park, '1' for Bicep Beach, '2' for Abalanche
        if (currentmap == 0) //If Protein Park is selected
        {
            //Set top earth tiles to grass
            tile1.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile2.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile3.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile4.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile5.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile6.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile7.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile8.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile9.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile10.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile11.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile12.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile13.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile14.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile15.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile16.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile17.GetComponent<SpriteRenderer>().sprite = pureearth;
            tile18.GetComponent<SpriteRenderer>().sprite = pureearth;
            //Set deep earth ground tiles to dirt
            tile19.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile20.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile21.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile22.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile23.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile24.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile25.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile26.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile27.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile28.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile29.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile30.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile31.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile32.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile33.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile34.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile35.GetComponent<SpriteRenderer>().sprite = pureearth2;
            tile36.GetComponent<SpriteRenderer>().sprite = pureearth2;

            //Make Protein Park scenery tiles visible and the Abalanche scenery tiles invisible
            hilltile1.GetComponent<Renderer>().enabled = true;
            hilltile2.GetComponent<Renderer>().enabled = true;
            abtile1.GetComponent<Renderer>().enabled = false;
            abtile2.GetComponent<Renderer>().enabled = false;

            //Make ocean tiles invisible
            ocean1.GetComponent<Renderer>().enabled = false;
            ocean2.GetComponent<Renderer>().enabled = false;
            ocean3.GetComponent<Renderer>().enabled = false;
            ocean4.GetComponent<Renderer>().enabled = false;
            ocean5.GetComponent<Renderer>().enabled = false;
            ocean6.GetComponent<Renderer>().enabled = false;
            ocean7.GetComponent<Renderer>().enabled = false;
            ocean8.GetComponent<Renderer>().enabled = false;
            ocean9.GetComponent<Renderer>().enabled = false;
            ocean10.GetComponent<Renderer>().enabled = false;
            ocean11.GetComponent<Renderer>().enabled = false;
            ocean12.GetComponent<Renderer>().enabled = false;
            ocean13.GetComponent<Renderer>().enabled = false;
            ocean14.GetComponent<Renderer>().enabled = false;
            ocean15.GetComponent<Renderer>().enabled = false;
            ocean16.GetComponent<Renderer>().enabled = false;
            ocean17.GetComponent<Renderer>().enabled = false;
            ocean18.GetComponent<Renderer>().enabled = false;
            ocean19.GetComponent<Renderer>().enabled = false;
            ocean20.GetComponent<Renderer>().enabled = false;

        }
        else if ((currentmap == 1) && (isbeachunlocked == 1)) //If Bicep Beach is selected and unlocked
        {
            //Set ground tiles to sand
            tile1.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile2.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile3.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile4.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile5.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile6.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile7.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile8.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile9.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile10.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile11.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile12.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile13.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile14.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile15.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile16.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile17.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile18.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile19.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile20.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile21.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile22.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile23.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile24.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile25.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile26.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile27.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile28.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile29.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile30.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile31.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile32.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile33.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile34.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile35.GetComponent<SpriteRenderer>().sprite = beachearth;
            tile36.GetComponent<SpriteRenderer>().sprite = beachearth;

            //Make scenery tiles invisible
            hilltile1.GetComponent<Renderer>().enabled = false;
            hilltile2.GetComponent<Renderer>().enabled = false;
            abtile1.GetComponent<Renderer>().enabled = false;
            abtile2.GetComponent<Renderer>().enabled = false;

            //Make ocean tiles visible
            ocean1.GetComponent<Renderer>().enabled = true;
            ocean2.GetComponent<Renderer>().enabled = true;
            ocean3.GetComponent<Renderer>().enabled = true;
            ocean4.GetComponent<Renderer>().enabled = true;
            ocean5.GetComponent<Renderer>().enabled = true;
            ocean6.GetComponent<Renderer>().enabled = true;
            ocean7.GetComponent<Renderer>().enabled = true;
            ocean8.GetComponent<Renderer>().enabled = true;
            ocean9.GetComponent<Renderer>().enabled = true;
            ocean10.GetComponent<Renderer>().enabled = true;
            ocean11.GetComponent<Renderer>().enabled = true;
            ocean12.GetComponent<Renderer>().enabled = true;
            ocean13.GetComponent<Renderer>().enabled = true;
            ocean14.GetComponent<Renderer>().enabled = true;
            ocean15.GetComponent<Renderer>().enabled = true;
            ocean16.GetComponent<Renderer>().enabled = true;
            ocean17.GetComponent<Renderer>().enabled = true;
            ocean18.GetComponent<Renderer>().enabled = true;
            ocean19.GetComponent<Renderer>().enabled = true;
            ocean20.GetComponent<Renderer>().enabled = true;

        }
        else if ((currentmap == 2) && (isabalancheunlocked == 1)) //If Abalanche is selected and unlocked
        {
            //Set top earth tiles to snow
            tile1.GetComponent<SpriteRenderer>().sprite = ice;
            tile2.GetComponent<SpriteRenderer>().sprite = ice;
            tile3.GetComponent<SpriteRenderer>().sprite = ice;
            tile4.GetComponent<SpriteRenderer>().sprite = ice;
            tile5.GetComponent<SpriteRenderer>().sprite = ice;
            tile6.GetComponent<SpriteRenderer>().sprite = ice;
            tile7.GetComponent<SpriteRenderer>().sprite = ice;
            tile8.GetComponent<SpriteRenderer>().sprite = ice;
            tile9.GetComponent<SpriteRenderer>().sprite = ice;
            tile10.GetComponent<SpriteRenderer>().sprite = ice;
            tile11.GetComponent<SpriteRenderer>().sprite = ice;
            tile12.GetComponent<SpriteRenderer>().sprite = ice;
            tile13.GetComponent<SpriteRenderer>().sprite = ice;
            tile14.GetComponent<SpriteRenderer>().sprite = ice;
            tile15.GetComponent<SpriteRenderer>().sprite = ice;
            tile16.GetComponent<SpriteRenderer>().sprite = ice;
            tile17.GetComponent<SpriteRenderer>().sprite = ice;
            tile18.GetComponent<SpriteRenderer>().sprite = ice;
            //Set deep earth ground tiles to ice
            tile19.GetComponent<SpriteRenderer>().sprite = ice2;
            tile20.GetComponent<SpriteRenderer>().sprite = ice2;
            tile21.GetComponent<SpriteRenderer>().sprite = ice2;
            tile22.GetComponent<SpriteRenderer>().sprite = ice2;
            tile23.GetComponent<SpriteRenderer>().sprite = ice2;
            tile24.GetComponent<SpriteRenderer>().sprite = ice2;
            tile25.GetComponent<SpriteRenderer>().sprite = ice2;
            tile26.GetComponent<SpriteRenderer>().sprite = ice2;
            tile27.GetComponent<SpriteRenderer>().sprite = ice2;
            tile28.GetComponent<SpriteRenderer>().sprite = ice2;
            tile29.GetComponent<SpriteRenderer>().sprite = ice2;
            tile30.GetComponent<SpriteRenderer>().sprite = ice2;
            tile31.GetComponent<SpriteRenderer>().sprite = ice2;
            tile32.GetComponent<SpriteRenderer>().sprite = ice2;
            tile33.GetComponent<SpriteRenderer>().sprite = ice2;
            tile34.GetComponent<SpriteRenderer>().sprite = ice2;
            tile35.GetComponent<SpriteRenderer>().sprite = ice2;
            tile36.GetComponent<SpriteRenderer>().sprite = ice2;

            //Make Abalanche scenery tiles visible and the Protein Park scenery tiles invisible
            hilltile1.GetComponent<Renderer>().enabled = false;
            hilltile2.GetComponent<Renderer>().enabled = false;
            abtile1.GetComponent<Renderer>().enabled = true;
            abtile2.GetComponent<Renderer>().enabled = true;

            //Make ocean tiles invisible
            ocean1.GetComponent<Renderer>().enabled = false;
            ocean2.GetComponent<Renderer>().enabled = false;
            ocean3.GetComponent<Renderer>().enabled = false;
            ocean4.GetComponent<Renderer>().enabled = false;
            ocean5.GetComponent<Renderer>().enabled = false;
            ocean6.GetComponent<Renderer>().enabled = false;
            ocean7.GetComponent<Renderer>().enabled = false;
            ocean8.GetComponent<Renderer>().enabled = false;
            ocean9.GetComponent<Renderer>().enabled = false;
            ocean10.GetComponent<Renderer>().enabled = false;
            ocean11.GetComponent<Renderer>().enabled = false;
            ocean12.GetComponent<Renderer>().enabled = false;
            ocean13.GetComponent<Renderer>().enabled = false;
            ocean14.GetComponent<Renderer>().enabled = false;
            ocean15.GetComponent<Renderer>().enabled = false;
            ocean16.GetComponent<Renderer>().enabled = false;
            ocean17.GetComponent<Renderer>().enabled = false;
            ocean18.GetComponent<Renderer>().enabled = false;
            ocean19.GetComponent<Renderer>().enabled = false;
            ocean20.GetComponent<Renderer>().enabled = false;

        }
        else //In any other case, the map has not been unlocked so default to Protein Park
        {
            {
                //Set top earth tiles to grass
                tile1.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile2.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile3.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile4.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile5.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile6.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile7.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile8.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile9.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile10.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile11.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile12.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile13.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile14.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile15.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile16.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile17.GetComponent<SpriteRenderer>().sprite = pureearth;
                tile18.GetComponent<SpriteRenderer>().sprite = pureearth;
                //Set deep earth ground tiles to dirt
                tile19.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile20.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile21.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile22.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile23.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile24.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile25.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile26.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile27.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile28.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile29.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile30.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile31.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile32.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile33.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile34.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile35.GetComponent<SpriteRenderer>().sprite = pureearth2;
                tile36.GetComponent<SpriteRenderer>().sprite = pureearth2;

                //Make Protein Park scenery tiles visible and the Abalanche scenery tiles invisible
                hilltile1.GetComponent<Renderer>().enabled = true;
                hilltile2.GetComponent<Renderer>().enabled = true;
                abtile1.GetComponent<Renderer>().enabled = false;
                abtile2.GetComponent<Renderer>().enabled = false;

                //Make ocean tiles invisible
                ocean1.GetComponent<Renderer>().enabled = false;
                ocean2.GetComponent<Renderer>().enabled = false;
                ocean3.GetComponent<Renderer>().enabled = false;
                ocean4.GetComponent<Renderer>().enabled = false;
                ocean5.GetComponent<Renderer>().enabled = false;
                ocean6.GetComponent<Renderer>().enabled = false;
                ocean7.GetComponent<Renderer>().enabled = false;
                ocean8.GetComponent<Renderer>().enabled = false;
                ocean9.GetComponent<Renderer>().enabled = false;
                ocean10.GetComponent<Renderer>().enabled = false;
                ocean11.GetComponent<Renderer>().enabled = false;
                ocean12.GetComponent<Renderer>().enabled = false;
                ocean13.GetComponent<Renderer>().enabled = false;
                ocean14.GetComponent<Renderer>().enabled = false;
                ocean15.GetComponent<Renderer>().enabled = false;
                ocean16.GetComponent<Renderer>().enabled = false;
                ocean17.GetComponent<Renderer>().enabled = false;
                ocean18.GetComponent<Renderer>().enabled = false;
                ocean19.GetComponent<Renderer>().enabled = false;
                ocean20.GetComponent<Renderer>().enabled = false;

            }
        }

    }

}
