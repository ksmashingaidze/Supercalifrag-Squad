using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;

public class ShootActivate : MonoBehaviour, IUpdateSelectedHandler, IPointerDownHandler, IPointerUpHandler
{
    public static ShootActivate SharedInstance; //Declare a new shared instance. Used if you want to access variables between scripts
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public PowerUps powerupsscript; //Reference the power-ups script
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
        if ((Time.timeScale == 1) && (charactermovementscript.isshootpressed == true) && (charactermovementscript.livesleft > 0))
        {
            //IMPLEMENT SHOOT
            if ((charactermovementscript.canshoot == true) && (Time.time - charactermovementscript.lastshoot >= charactermovementscript.shootconsecutivememory)) //If the fixed time between consecutive shoot events has elapsed and the player has the relevant power-up equipped
            {
                charactermovementscript.throwsound.Play(); //Play the throw sound
                charactermovementscript.lastshoot = Time.time; //Save the time of the last shoot
                if ((charactermovementscript.isfaceright == true) && (Time.time == charactermovementscript.lastshoot)) //If the player is facing the right of the screen and they are still within the shoot time window
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
                    float xloc = player.transform.position.x + (player.transform.localScale.x / 2);
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

                    powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().velocity = new Vector2(charactermovementscript.bulletspeed, 4f); //Give the bullet an initial velocity

                    powerupsscript.isbulletdead[powerupsscript.bulletindex] = false;

                    powerupsscript.bulletindex = powerupsscript.bulletindex + 1; //Increment the index that keeps track of all the bullets in the scene

                }
                else if ((charactermovementscript.isfaceright == false) && (Time.time == charactermovementscript.lastshoot)) //If the player is facing the left of the screen and they are still within the shoot time window
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
                    float xloc = player.transform.position.x - (player.transform.localScale.x / 2);
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

                    powerupsscript.bulletcopy[powerupsscript.bulletindex].GetComponent<Rigidbody2D>().velocity = new Vector2(-charactermovementscript.bulletspeed, 4f); //Give the bullet an initial velocity

                    powerupsscript.isbulletdead[powerupsscript.bulletindex] = false;

                    powerupsscript.bulletindex = powerupsscript.bulletindex + 1; //Increment the index that keeps track of all the bullets in the scene

                }
            }


        }

        charactermovementscript.isshootpressed = false;

    }

    //AN APPROPRIATE POINTER EVENT TRIGGER NEEDS TO BE ADDED TO THE BUTTON IN THE UNITY INSPECTOR FIRST
    public void OnPointerDown(PointerEventData data)
    {
        if (Time.timeScale == 1)
        {
            charactermovementscript.isshootpressed = true;
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
