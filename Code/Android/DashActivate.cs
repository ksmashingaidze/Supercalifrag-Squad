using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class DashActivate : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    public static DashActivate SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public GameObject playerobject; //Declare a game object for the player sprite
    Rigidbody2D player; //Declare the rigidbody that will own this script
    

    //Awake allows declaration or initialization of variables before the program runs.
    void Awake()
    {
        SharedInstance = this; //This is the shared instance. Used if you want to access variables between scripts for the same owner.
    }

    // Start is called before the first frame update
    void Start()
    {
        player = playerobject.GetComponent<Rigidbody2D>(); //Get the player's rigid body component

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUpdateSelected(BaseEventData data)
    {
        //Works like void update, but is triggered whenever the button press state changes. Due to the weird way you implemented the dash ability, we need to zero the velocity immediately after applying the force.
        if ((Time.timeScale == 1) && (charactermovementscript.isdashpressed == true) && (charactermovementscript.livesleft > 0))
        {
            //IMPLEMENT DASH
            if (Time.time - charactermovementscript.lastdash >= charactermovementscript.dashconsecutivememory) //If the fixed time between consecutive dashes has elapsed
            {
                charactermovementscript.dashsound.Play(); //Play the dash sound
                charactermovementscript.lastdash = Time.time; //Save the time of the last dash
                if ((charactermovementscript.isfaceright == true) && (Time.time - charactermovementscript.lastdash < charactermovementscript.dashconsecutivememory)) //If the player is facing the right of the screen and they are still within the dash time window
                {
                    player.AddForce(new Vector2(charactermovementscript.dashmagnitudex, charactermovementscript.dashmagnitudey)); //Dash right



                }
                else if ((charactermovementscript.isfaceright == false) && (Time.time - charactermovementscript.lastdash < charactermovementscript.dashconsecutivememory)) //If the player is facing the left of the screen and they are still within the dash time window
                {
                    player.AddForce(new Vector2(-charactermovementscript.dashmagnitudex, charactermovementscript.dashmagnitudey)); //Dash left
                }

            }
        }

        charactermovementscript.isdashpressed = false;
        player.velocity = new Vector2(0f, player.velocity.y); //Zero the player's velocity in the x-direction. Whatever the y velocity is, maintain it.

    }

    //AN APPROPRIATE POINTER EVENT TRIGGER NEEDS TO BE ADDED TO THE BUTTON IN THE UNITY INSPECTOR FIRST
    public void OnPointerDown(PointerEventData data)
    {
        if (Time.timeScale == 1)
        {
            charactermovementscript.isdashpressed = true;
        }
        
        

    }

    public void OnPointerUp(PointerEventData data)
    {
        if (Time.timeScale == 1)
        {
            //Do nothing
        }

    }


}
