using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class LeftMoveActivate : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    public static LeftMoveActivate SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts
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
        //Do nothing, since we've implemented some code elsewhere in this script. Works like void update, but is triggered whenever the button press state changes

    }

    //A POINTER DOWN EVENT TRIGGER NEEDS TO BE ADDED TO THE BUTTON IN THE UNITY INSPECTOR FIRST
    public void OnPointerDown(PointerEventData data)
    {
        if ((Time.timeScale == 1) && (charactermovementscript.livesleft > 0))
        {
            charactermovementscript.isrunningleft = true; //The character is running
            charactermovementscript.isfaceright = false; //The character is facing left
            //float xaxis = Input.GetAxisRaw("Horizontal"); //Get x-axis
            //float xvelocity = xaxis * playerspeed; //Calculate velocity in the x direction
            //player.velocity = new Vector2(xvelocity, player.velocity.y); //Update the player's velocity in the x-direction. Whatever the y velocity is, maintain it.
            player.velocity = new Vector2(-charactermovementscript.playerspeed, player.velocity.y); //Update the player's velocity in the x-direction. Whatever the y velocity is, maintain it.


        }
    }


    public void OnPointerUp(PointerEventData data)
    {
        charactermovementscript.isrunningleft = false; //The character has stopped running
        player.velocity = new Vector2(0f, player.velocity.y); //Zero the player's velocity in the x-direction. Whatever the y velocity is, maintain it.

    }


}
