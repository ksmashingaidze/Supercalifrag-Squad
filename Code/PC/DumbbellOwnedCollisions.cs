using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbbellOwnedCollisions : MonoBehaviour
{
    public static DumbbellOwnedCollisions SharedInstance; //Declare a new shared instance
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public BurgerMovement burgermovementscript; //Reference the burger movement script
    public PizzaMovement pizzamovementscript; //Reference the pizza movement script
    public BirdMovement birdmovementscript; //Reference the bird movement script
    public BakerMovement bakermovementscript; //Reference the baker movement script
    public PowerUps powerupsscript; //Reference the power-up script

    // Awake allows declaration or initialization of variables before the program runs. 
    void Awake()
    {
        SharedInstance = this; //This is the shared instance
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //The 'OnTriggerEnter2D' function is a preset Unity function that will be called automatically when the owner of this script's collider makes contact with another collider.
    //For the function to work, the owner of the script must have a RigidBody2D, and a Collider2D. The other object must have otherobject.GetComponent<Collider2D>().isTrigger = true
    //The other object's tag must also already exist in the tag library, even if you assign it to randomly generated objects at runtime.
    //To facilitate player collision and ground collision, this function must exist in a script attached to the player and a separate one attached to the ground.
    //This function does not need to be called.
    void OnTriggerEnter2D(Collider2D bulletownedcollider)
    {
        if ((bulletownedcollider.tag == "pizzacollision") && (pizzamovementscript.ispizzadead is false))
        {
            if (pizzamovementscript.pizzacollisionboundcopy[pizzamovementscript.poolindex] == bulletownedcollider.gameObject) //If the baker hits a pizza
            {
                pizzamovementscript.PlayPizzaDeath(); //Call the function that plays the pizza death animation
                                                      //Execute a delay, then delete the pizza instance elements
                pizzamovementscript.pizzacopy[pizzamovementscript.poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                pizzamovementscript.pizzacopy[pizzamovementscript.poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                Invoke("HandlePizzaDeath", charactermovementscript.deathinterval); //Invoke the function 'HandlepizzaDeath' once with the same 'deathinterval' as the player character

                //Handle bullet animation and destruction
                for (int j = 0; j < powerupsscript.bulletamount; j++)
                {
                    if (powerupsscript.bulletcopy[j] == gameObject)
                    {
                        powerupsscript.PlayBulletDeath(j); //Call the function that plays the burger death animation
                                                           //Execute a delay, then delete the burger instance elements
                        powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                        powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                        Invoke("HandleBulletDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                    }

                }

                //gameObject.transform.GetChild(0).gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference its child this way.
                //gameObject.transform.GetChild(0).gameObject = null; //Clear bullet collision bound array slot
                //gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference it this way.
                //gameObject = null; //Clear array slot

            }

        }
        else if ((bulletownedcollider.tag == "birdcollision") && (birdmovementscript.isbirddead is false))
        {
            if (birdmovementscript.birdcollisionboundcopy[birdmovementscript.poolindex] == bulletownedcollider.gameObject) //If the baker hits a bird
            {
                birdmovementscript.PlayBirdDeath(); //Call the function that plays the bird death animation
                                                      //Execute a delay, then delete the bird instance elements
                birdmovementscript.birdcopy[birdmovementscript.poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                birdmovementscript.birdcopy[birdmovementscript.poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                Invoke("HandleBirdDeath", charactermovementscript.deathinterval); //Invoke the function 'HandlebirdDeath' once with the same 'deathinterval' as the player character

                //Handle bullet animation and destruction
                for (int j = 0; j < powerupsscript.bulletamount; j++)
                {
                    if (powerupsscript.bulletcopy[j] == gameObject)
                    {
                        powerupsscript.PlayBulletDeath(j); //Call the function that plays the burger death animation
                                                           //Execute a delay, then delete the burger instance elements
                        powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                        powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                        Invoke("HandleBulletDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                    }

                }
                //gameObject.transform.GetChild(0).gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference its child this way.
                //gameObject.transform.GetChild(0).gameObject = null; //Clear bullet collision bound array slot
                //gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference it this way.
                //gameObject = null; //Clear array slot

            }

        }
        else if (bulletownedcollider.tag == "burgercollision")
        {
            for (int i=0; i<burgermovementscript.poolamount; i++)
            {
                if ((burgermovementscript.burgercollisionboundcopy[i] == bulletownedcollider.gameObject) && (burgermovementscript.isburgerdead[i] is false)) //If the baker hits a burger
                {
                    burgermovementscript.PlayBurgerDeath(i); //Call the function that plays the burger death animation
                                                          //Execute a delay, then delete the burger instance elements
                    burgermovementscript.burgercopy[i].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                    burgermovementscript.burgercopy[i].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                    Invoke("HandleBurgerDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                    //Handle bullet animation and destruction
                    for (int j = 0; j < powerupsscript.bulletamount; j++)
                    {
                        if (powerupsscript.bulletcopy[j] == gameObject)
                        {
                            powerupsscript.PlayBulletDeath(j); //Call the function that plays the burger death animation
                                                               //Execute a delay, then delete the burger instance elements
                            powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                            powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                            Invoke("HandleBulletDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                        }

                    }

                    //gameObject.transform.GetChild(0).gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference its child this way.
                    //gameObject.transform.GetChild(0).gameObject = null; //Clear bullet collision bound array slot
                    //gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference it this way.
                    //gameObject = null; //Clear array slot

                }

            }


        }
        else if ((bulletownedcollider.tag == "bakercollision") && (bakermovementscript.isbakerdead is false))
        {
            if (bakermovementscript.bakercollisionboundcopy[bakermovementscript.poolindex] == bulletownedcollider.gameObject) //If the baker hits a baker
            {
                bakermovementscript.PlayBakerDeath(); //Call the function that plays the baker death animation
                                                      //Execute a delay, then delete the baker instance elements
                bakermovementscript.bakercopy[bakermovementscript.poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                bakermovementscript.bakercopy[bakermovementscript.poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                Invoke("HandleBakerDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleBakerDeath' once with the same 'deathinterval' as the player character

                //Handle bullet animation and destruction
                for (int j = 0; j < powerupsscript.bulletamount; j++)
                {
                    if (powerupsscript.bulletcopy[j] == gameObject)
                    {
                        powerupsscript.PlayBulletDeath(j); //Call the function that plays the burger death animation
                                                           //Execute a delay, then delete the burger instance elements
                        powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                        powerupsscript.bulletcopy[j].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                        Invoke("HandleBulletDeath", charactermovementscript.deathinterval); //Invoke the function 'HandleburgerDeath' once with the same 'deathinterval' as the player character

                    }

                }

                //gameObject.transform.GetChild(0).gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference its child this way.
                //gameObject.transform.GetChild(0).gameObject = null; //Clear bullet collision bound array slot
                //gameObject.SetActive(false); //Remove instance. Note the dumbbell bullet is the owner of this script, so we can reference it this way.
                //gameObject = null; //Clear array slot


            }

        }

    }


    private void HandleBakerDeath()
    {
        bakermovementscript.bakercopy[bakermovementscript.poolindex].SetActive(false); //Remove instance
        bakermovementscript.bakercopy[bakermovementscript.poolindex] = null; //Clear array slot
        bakermovementscript.bakercollisionboundcopy[bakermovementscript.poolindex].SetActive(false); //Remove instance
        bakermovementscript.bakercollisionboundcopy[bakermovementscript.poolindex] = null; //Clear array slot
        
    }

    private void HandlePizzaDeath()
    {
        pizzamovementscript.pizzacopy[pizzamovementscript.poolindex].SetActive(false); //Remove instance
        pizzamovementscript.pizzacopy[pizzamovementscript.poolindex] = null; //Clear array slot
        pizzamovementscript.pizzacollisionboundcopy[pizzamovementscript.poolindex].SetActive(false); //Remove instance
        pizzamovementscript.pizzacollisionboundcopy[pizzamovementscript.poolindex] = null; //Clear array slot
        
    }

    private void HandleBirdDeath()
    {
        birdmovementscript.birdcopy[birdmovementscript.poolindex].SetActive(false); //Remove instance
        birdmovementscript.birdcopy[birdmovementscript.poolindex] = null; //Clear array slot
        birdmovementscript.birdcollisionboundcopy[birdmovementscript.poolindex].SetActive(false); //Remove instance
        birdmovementscript.birdcollisionboundcopy[birdmovementscript.poolindex] = null; //Clear array slot
        

    }

    private void HandleBurgerDeath()
    {
        for (int i = 0; i < burgermovementscript.poolamount; i++)
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
        for (int i = 0; i < powerupsscript.bulletamount; i++)
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
