using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GroundScript : MonoBehaviour
{
    public static GroundScript SharedInstance; //Declare a new shared instance
    public BurgerMovement burgermovementscript; //Reference the burger movement script
    public PowerUps powerupsscript; //Reference the power-ups script to find the bullet instances
    public CharacterMovement charactermovementscript; //Reference the character movement script

    public float bgupdatetime = 1f; //Declare the update time period which will be used with 'InvokeRepeating'
    public Color cameracol; //Declare a Color variable to set the value of the camera background
    public int colindex; //Declare an index to keep track of the intermediate color values
    public int colamount = 81; //Declare a variable to store the total number of intermediate color values
    public float[] rvalues; //Declare an array of floats for the intermediate red primary color values
    public float[] gvalues; //Declare an array of floats for the intermediate green primary color values
    public float[] bvalues; //Declare an array of floats for the intermediate blue primary color values

    public int isbeachunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    public int isabalancheunlocked = 0; //Initialize a variable to keep track of the state of unlockable items. '0' for locked, '1' for unlocked.
    
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


    // Awake allows declaration or initialization of variables before the program runs. 
    void Awake()
    {
        SharedInstance = this; //This is the shared instance

        //Load the map
        currentmap = PlayerPrefs.GetInt("currentmap", 0); //Get the current map from player preferences. '0' for Protein Park, '1' for Bicep Beach, '2' for Abalanche
        isbeachunlocked = PlayerPrefs.GetInt("isbeachunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        isabalancheunlocked = PlayerPrefs.GetInt("isabalancheunlocked", 0); //Check if an item has been unlocked. '0' for locked, '1' for unlocked.
        if (currentmap == 0) //If Protein Park is selected
        {
            //Set on screen controls to white
            charactermovementscript.leftmovebtn.GetComponent<Image>().color = new Color(1,1,1);
            charactermovementscript.rightmovebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.jumpbtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.divebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.dashbtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.shootbtn.GetComponent<Image>().color = new Color(1, 1, 1);

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
            //Set on screen controls to white
            charactermovementscript.leftmovebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.rightmovebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.jumpbtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.divebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.dashbtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.shootbtn.GetComponent<Image>().color = new Color(1, 1, 1);

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
            //Set on screen controls to black
            charactermovementscript.leftmovebtn.GetComponent<Image>().color = new Color(0, 0, 0);
            charactermovementscript.rightmovebtn.GetComponent<Image>().color = new Color(0, 0, 0);
            charactermovementscript.jumpbtn.GetComponent<Image>().color = new Color(0, 0, 0);
            charactermovementscript.divebtn.GetComponent<Image>().color = new Color(0, 0, 0);
            charactermovementscript.dashbtn.GetComponent<Image>().color = new Color(0, 0, 0);
            charactermovementscript.shootbtn.GetComponent<Image>().color = new Color(0, 0, 0);

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
            //Set on screen controls to white
            charactermovementscript.leftmovebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.rightmovebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.jumpbtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.divebtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.dashbtn.GetComponent<Image>().color = new Color(1, 1, 1);
            charactermovementscript.shootbtn.GetComponent<Image>().color = new Color(1, 1, 1);

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


        //Populate primary color intermediate values
        rvalues = new float[colamount];
        gvalues = new float[colamount];
        bvalues = new float[colamount];
        rvalues = new float[]{ 0.749f, 0.755f, 0.762f, 0.768f, 0.774f, 0.780f, 0.787f, 0.793f, 0.799f, 0.805f, 0.812f, 0.818f, 0.824f, 0.831f, 0.837f, 0.843f, 0.849f, 0.856f, 0.862f, 0.868f, 0.875f, 0.881f, 0.887f, 0.893f, 0.900f, 0.906f, 0.912f, 0.918f, 0.925f, 0.931f, 0.937f, 0.944f, 0.950f, 0.956f, 0.962f, 0.969f, 0.975f, 0.981f, 0.987f, 0.994f, 1.000f, 0.978f, 0.957f, 0.935f, 0.913f, 0.892f, 0.870f, 0.848f, 0.827f, 0.805f, 0.783f, 0.762f, 0.740f, 0.718f, 0.697f, 0.675f, 0.653f, 0.632f, 0.610f, 0.588f, 0.567f, 0.545f, 0.523f, 0.502f, 0.480f, 0.458f, 0.437f, 0.415f, 0.393f, 0.372f, 0.350f, 0.328f, 0.307f, 0.285f, 0.263f, 0.242f, 0.220f, 0.198f, 0.177f, 0.155f, 0.133f}; //Populate array of red primary color values
        gvalues = new float[]{ 0.847f, 0.839f, 0.832f, 0.824f, 0.816f, 0.808f, 0.801f, 0.793f, 0.785f, 0.777f, 0.770f, 0.762f, 0.754f, 0.746f, 0.739f, 0.731f, 0.723f, 0.715f, 0.708f, 0.700f, 0.692f, 0.684f, 0.677f, 0.669f, 0.661f, 0.653f, 0.646f, 0.638f, 0.630f, 0.622f, 0.615f, 0.607f, 0.599f, 0.591f, 0.584f, 0.576f, 0.568f, 0.560f, 0.553f, 0.545f, 0.537f, 0.524f, 0.510f, 0.497f, 0.484f, 0.470f, 0.457f, 0.443f, 0.430f, 0.416f, 0.403f, 0.390f, 0.376f, 0.363f, 0.349f, 0.336f, 0.322f, 0.309f, 0.295f, 0.282f, 0.269f, 0.255f, 0.242f, 0.228f, 0.215f, 0.201f, 0.188f, 0.175f, 0.161f, 0.148f, 0.134f, 0.121f, 0.107f, 0.094f, 0.081f, 0.067f, 0.054f, 0.040f, 0.027f, 0.013f, 0.000f}; //Populate array of green primary color values
        bvalues = new float[]{ 1.000f, 0.981f, 0.962f, 0.942f, 0.923f, 0.904f, 0.885f, 0.865f, 0.846f, 0.827f, 0.808f, 0.789f, 0.769f, 0.750f, 0.731f, 0.712f, 0.693f, 0.673f, 0.654f, 0.635f, 0.616f, 0.596f, 0.577f, 0.558f, 0.539f, 0.520f, 0.500f, 0.481f, 0.462f, 0.443f, 0.424f, 0.404f, 0.385f, 0.366f, 0.347f, 0.327f, 0.308f, 0.289f, 0.270f, 0.251f, 0.231f, 0.232f, 0.232f, 0.232f, 0.232f, 0.232f, 0.233f, 0.233f, 0.233f, 0.233f, 0.233f, 0.234f, 0.234f, 0.234f, 0.234f, 0.234f, 0.235f, 0.235f, 0.235f, 0.235f, 0.235f, 0.235f, 0.236f, 0.236f, 0.236f, 0.236f, 0.236f, 0.237f, 0.237f, 0.237f, 0.237f, 0.237f, 0.238f, 0.238f, 0.238f, 0.238f, 0.238f, 0.239f, 0.239f, 0.239f, 0.239f}; //Populate array of blue primary color values

        //Set the startup background color
        cameracol = new Color(rvalues[0], gvalues[0], bvalues[0]); //Set the initial background color to sky blue

        InvokeRepeating("DayNightCycle", bgupdatetime, bgupdatetime); //Repeat the function 'DayNightCycle' after initial time 'bgupdatetime', and every 'bgupdatetime' thereafter

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Function to change the background color with time to implement a day and night cycle
    private void DayNightCycle()
    {
        if (colindex < colamount)
        {
            cameracol = new Color(rvalues[colindex], gvalues[colindex], bvalues[colindex]); //Pull the new camera background color from the arrays

        }
        else if ((colindex >= colamount) && (colindex < 2*colamount))
        {
            cameracol = new Color(rvalues[((colamount-1)-(colindex-colamount))], gvalues[((colamount - 1) - (colindex - colamount))], bvalues[((colamount - 1) - (colindex - colamount))]); //Pull the new camera background color from the arrays

        }
        else if (colindex >= 2*colamount)
        {
            colindex = 0; //Reset the color picking index
            cameracol = new Color(rvalues[colindex], gvalues[colindex], bvalues[colindex]); //Pull the new camera background color from the arrays
        }

        colindex = colindex + 1; //Increment the color picking index
        
        Camera.main.backgroundColor = cameracol; //Set the new camera background color

    }


    //The 'OnTriggerEnter2D' function is a preset Unity function that will be called automatically when the owner of this script's collider makes contact with another collider.
    //For the function to work, the owner of the script must have a RigidBody2D, and a Collider2D. The other object must have otherobject.GetComponent<Collider2D>().isTrigger = true
    //The other object's tag must also already exist in the tag library, even if you assign it to randomly generated objects at runtime.
    //To facilitate player collision and ground collision, this function must exist in a script attached to the player and a separate one attached to the ground.
    //This function does not need to be called.
    void OnTriggerEnter2D(Collider2D groundcollider)
    {
        if (groundcollider.tag == "burgercollision")
        {
            for (int i = 0; i < burgermovementscript.poolamount; i++)
            {
                if (burgermovementscript.burgercollisionboundcopy[i] == groundcollider.gameObject)
                {
                    burgermovementscript.PlayBurgerDeath(i); //Call the function that plays the burger death animation
                                                            //Execute a delay, then delete the burger instance elements
                    burgermovementscript.burgercopy[i].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                    burgermovementscript.burgercopy[i].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                    Invoke("HandleBurgerDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                }

            }

        }
        else if (groundcollider.tag == "bulletcollision")
        {
            for (int i = 0; i < powerupsscript.bulletamount; i++)
            {
                if (powerupsscript.bulletcollisionboundcopy[i] == groundcollider.gameObject)
                {
                    powerupsscript.PlayBulletDeath(i); //Call the function that plays the burger death animation
                                                        //Execute a delay, then delete the burger instance elements
                    powerupsscript.bulletcopy[i].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                    powerupsscript.bulletcopy[i].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                    Invoke("HandleBulletDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                }

            }

        }

    }

    private void HandleBurgerDeath()
    {
        for (int i=0; i<burgermovementscript.poolamount; i++)
        {
            if ((burgermovementscript.isburgerdead[i] is true) && (burgermovementscript.burgercopy[i] != null))
            {
                burgermovementscript.burgercopy[i].SetActive(false); //Remove instance
                burgermovementscript.burgercopy[i] = null; //Clear array slot
                burgermovementscript.burgercollisionboundcopy[i].SetActive(false); //Remove instance
                burgermovementscript.burgercollisionboundcopy[i] = null; //Clear array slot
            }
        }
    }


    private void HandleBulletDeath()
    {
        for (int i=0; i<powerupsscript.bulletamount; i++)
        {
            if ((powerupsscript.isbulletdead[i] is true) && (powerupsscript.bulletcopy[i] != null))
            {
                powerupsscript.bulletcopy[i].SetActive(false); //Remove instance
                powerupsscript.bulletcopy[i] = null; //Clear array slot
                powerupsscript.bulletcollisionboundcopy[i].SetActive(false); //Remove instance
                powerupsscript.bulletcollisionboundcopy[i] = null; //Clear array slot
            }
        }
    }


}
