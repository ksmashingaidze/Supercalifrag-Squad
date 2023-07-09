using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MainMenuScript : MonoBehaviour
{
    public Image eulabgpanel; //Declare a new UI Image that will be used to find and store the image component of the EULA BG panel. Panels have image and gameObject components.
    public Button acceptbtn; //Declare a new UI Button that will be used to find and store the EULA accept button
    public Button declinebtn; //Declare a new UI Button that will be used to find and store the EULA decline button
    public Text eulaheadingtext; //Declare a new UI Text item to find and store the EULA heading
    public Text eulatext; //Declare a new UI Text item to find and store the EULA body text

    public Button playbtn; //Declare a new UI Button that will be used to find and store the play button
    public Button settingsbtn; //Declare a new UI Button that will be used to find and store the settings button
    public Button aboutbtn; //Declare a new UI Button that will be used to find and store the about button
    public Button exitbtn; //Declare a new UI Button that will be used to find and store the exit button
    public Text high_score_value; //Declare a new UI Text item to find and store the high score value text element

    public float musicvolume = 1f; //Declare a variable to store the music volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public float soundeffectsvolume = 1f; //Declare a variable to store the sound effects volume from the player preferences. The variable will be initialized at the original value of 1 just in case.
    public AudioSource selectsound; //Declare a new sound source

    public int isbeachunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int isabalancheunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.

    public int iseulasigned = 0; //Initialize a variable to keep track of whether the player has agreed to the EULA

    //Declare sprites
    public Sprite pureearth;
    public Sprite pureearth2;
    public Sprite beachearth;
    public Sprite ice;
    public Sprite ice2;

    public int currentmap = 0; //Declare an integer variable to keep track of which map is currently selected
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

    //Declare mouse cursor variables
    //public Texture2D cursortexture;
    //public CursorMode cursormode = CursorMode.Auto;
    //public Vector2 hotspot = new Vector2(0,0);


    // Awake allows declaration or initialization of variables before the program runs. 
    void Awake()
    {
        //Show EULA if the user has not agreed to it yet
        iseulasigned = PlayerPrefs.GetInt("iseulasigned", 0); //Check the player preferences to determine whether the player has signed the EULA
        if (iseulasigned == 0)
        {
            //Show the EULA and its elements
            eulabgpanel.gameObject.SetActive(true);
            acceptbtn.gameObject.SetActive(true);
            declinebtn.gameObject.SetActive(true);
            eulaheadingtext.gameObject.SetActive(true);
            eulatext.gameObject.SetActive(true);
       
        }
        else if (iseulasigned == 1)
        {
            //Hide the EULA and its elements
            eulabgpanel.gameObject.SetActive(false);
            acceptbtn.gameObject.SetActive(false);
            declinebtn.gameObject.SetActive(false);
            eulaheadingtext.gameObject.SetActive(false);
            eulatext.gameObject.SetActive(false);

        }


        //Load the map
        currentmap = PlayerPrefs.GetInt("currentmap", 0); //Get the current map from player preferences. '0' for Protein Park, '1' for Bicep Beach, '2' for Abalanche
        isbeachunlocked = PlayerPrefs.GetInt("isbeachunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        isabalancheunlocked = PlayerPrefs.GetInt("isabalancheunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
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

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false; //Make the connected item, 'mainground', invisible

        //Create task listeners for all the EULA buttons
        acceptbtn.onClick.AddListener(AcceptBtnClicked);
        declinebtn.onClick.AddListener(DeclineBtnClicked);

        //Create task listeners for all the main menu buttons
        playbtn.onClick.AddListener(PlayBtnClicked);
        settingsbtn.onClick.AddListener(SettingsBtnClicked);
        aboutbtn.onClick.AddListener(AboutBtnClicked);
        exitbtn.onClick.AddListener(ExitBtnClicked);

        LoadHighScore(); //Execute function that gets the high score from the player preferences and displays it
        LoadVolume(); //Execute function that gets the music volume and sound effects volume from the player preferences

        selectsound.volume = soundeffectsvolume; //Set the volume of the sound source
    }

    // Update is called once per frame
    void Update()
    {
        //If 'ESC' is pressed on the main menu
        if (Input.GetKeyDown(KeyCode.Escape)) //If the 'ESC' key is pressed. Note 'GetKeyDown' becomes true only once when you tap the key. 'GetKey' becomes true even if the key is held down.
        {
            selectsound.Play(); //Play the menu select sound
            Invoke("ExitAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play

        }

        //If any relevant navigation keys up, down, W, S, Enter, or Space are pressed, play a sound
        if ((Input.GetKeyDown(KeyCode.UpArrow)) || (Input.GetKeyDown(KeyCode.DownArrow)) || (Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Return)) || (Input.GetKeyDown(KeyCode.Space)))
        {
            selectsound.Play(); //Play the menu select sound

        }

    }

    void AcceptBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        iseulasigned = 1; //Update the EULA variable to indicate the player has signed
        PlayerPrefs.SetInt("iseulasigned", 1); //Update the player preferences to indicate the player has signed the EULA
        //Hide the EULA and its elements
        eulabgpanel.gameObject.SetActive(false);
        acceptbtn.gameObject.SetActive(false);
        declinebtn.gameObject.SetActive(false);
        eulaheadingtext.gameObject.SetActive(false);
        eulatext.gameObject.SetActive(false);

    }

    void DeclineBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        Invoke("ExitAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }

    void PlayBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        Invoke("PlayAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }

    void SettingsBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        Invoke("SettingsAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }

    void AboutBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        Invoke("AboutAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }

    void ExitBtnClicked()
    {
        selectsound.Play(); //Play the menu select sound
        Invoke("ExitAction", 0.2f); //Invoke a function once after a tiny delay to give the sound clip time to play
    }

    void LoadHighScore()
    {
        int highscore = PlayerPrefs.GetInt("highscorevalue", 0); //Get the player's high score if it exists. Get '0' if the high score hasn't been set yet
        high_score_value.text = Convert.ToString(highscore);

    }

    void LoadVolume()
    {
        musicvolume = PlayerPrefs.GetFloat("musicvolume", 1f); //Get the music volume from player preferences. Set it to the default of '1' if it hasn't been set yet.
        soundeffectsvolume = PlayerPrefs.GetFloat("soundeffectsvolume", 1f); //Get the sound effects volume from player preferences. Set it to the default of '1' if it hasn't been set yet.

    }

    private void PlayAction()
    {
        SceneManager.LoadScene("Game"); //Load the game 
    }

    private void AboutAction()
    {
        SceneManager.LoadScene("About"); //Load the about screen 
    }

    private void SettingsAction()
    {
        SceneManager.LoadScene("Settings"); //Load the settings screen 
    }

    private void ExitAction()
    {
        Application.Quit(); //Exit the game 
    }

    //void OnMouseEnter()
    //{
        
    //}

    //void OnMouseExit()
    //{
        //Cursor.SetCursor(null, Vector2.zero, cursormode);
    //}


}
