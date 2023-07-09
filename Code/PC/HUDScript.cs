using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HUDScript : MonoBehaviour
{
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public Text ScoreText; //Declare a new Text Object that will be used to find and store the player's current score
    public Text CreatineText; //Declare a new Text Object that will be used to find and store the lives left text
    public Image DumbbellImg; //Declare a new UI Image that will be used to find and store the dumbbell icon
    public float currenttime = 0f; //Declare a float variable that will be used to store the current time since the scene loaded
    public float additionalscore = 0f; //Declare a float variable that will be used to store the additional points the player has racked up from eating chicken

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = "0"; //Initialize the score as 0
        CreatineText.text = "1"; //Initialize the number of lives as 1
        DumbbellImg.enabled = false; //Make the connected UI Image invisible on start-up
    }

    // Update is called once per frame
    void Update()
    {
        //Handle score
        if (charactermovementscript.livesleft > 0) //If the player hasn't died yet
        {
            currenttime = Time.timeSinceLevelLoad; //Store the current time, catering for the reset of the level upon death. 'Time.time' cannot be reset, but 'Time.timeSinceLevelLoad' can be used instead.
            int totalscoreint = (int)(currenttime+additionalscore); //Set the score to the number of seconds the player has survived added to any additional points
            ScoreText.text = Convert.ToString(totalscoreint); //Display the score
        }

        //Handle lives left
        CreatineText.text = Convert.ToString(charactermovementscript.livesleft); //Update the lives left text according to the number of lives the player has

        //Handle dumbbell icon
        if (charactermovementscript.canshoot is true)
        {
            DumbbellImg.enabled = true; //Make the connected item visible
        }
        else if (charactermovementscript.canshoot is false)
        {
            DumbbellImg.enabled = false; //Make the connected item invisible
        }

    }
}
