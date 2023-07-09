using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class DiveActivate : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    public static DiveActivate SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts
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
        if ((Time.timeScale == 1) && (charactermovementscript.isdivepressed == true) && (charactermovementscript.isdiving == false) && (charactermovementscript.isgrounded == false) && (charactermovementscript.livesleft > 0)) //If the player is not already diving and they are alive in mid-air
        {
            charactermovementscript.divesound.Play(); //Play the dive sound
            charactermovementscript.isdiving = true;

        }

        charactermovementscript.isdivepressed = false;

    }

    //AN APPROPRIATE POINTER EVENT TRIGGER NEEDS TO BE ADDED TO THE BUTTON IN THE UNITY INSPECTOR FIRST
    public void OnPointerDown(PointerEventData data)
    {
        if (Time.timeScale == 1)
        {
            charactermovementscript.isdivepressed = true;
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
