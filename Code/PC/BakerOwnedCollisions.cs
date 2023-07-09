using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakerOwnedCollisions : MonoBehaviour
{
    public CharacterMovement charactermovementscript; //Reference the character movement script
    public PizzaMovement pizzamovementscript; //Reference the pizza movement script
    public BirdMovement birdmovementscript; //Reference the bird movement script
    public BakerMovement bakermovementscript; //Reference the baker movement script

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
    void OnTriggerEnter2D(Collider2D bakerownedcollider)
    {
        if ((bakerownedcollider.tag == "pizzacollision") && (bakermovementscript.isbakerdead is false) && (pizzamovementscript.ispizzadead is false))
        {
            if (pizzamovementscript.pizzacollisionboundcopy[pizzamovementscript.poolindex] == bakerownedcollider.gameObject) //If the baker hits a pizza
            {
                pizzamovementscript.PlayPizzaDeath(); //Call the function that plays the pizza death animation
                                                      //Execute a delay, then delete the pizza instance elements
                pizzamovementscript.pizzacopy[pizzamovementscript.poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                pizzamovementscript.pizzacopy[pizzamovementscript.poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                Invoke("HandlePizzaDeath", charactermovementscript.deathinterval); //Invoke the function 'HandlepizzaDeath' once with the same 'deathinterval' as the player character


            }

        }
        else if ((bakerownedcollider.tag == "birdcollision") && (bakermovementscript.isbakerdead is false) && (birdmovementscript.isbirddead is false))
        {
            if (birdmovementscript.birdcollisionboundcopy[birdmovementscript.poolindex] == bakerownedcollider.gameObject) //If the baker hits a bird
            {
                birdmovementscript.PlayBirdDeath(); //Call the function that plays the bird death animation
                                                      //Execute a delay, then delete the bird instance elements
                birdmovementscript.birdcopy[birdmovementscript.poolindex].GetComponent<Rigidbody2D>().simulated = false; //Disable the RigidBody2D's ability to simulate physics, making it a ghost other objects pass through
                birdmovementscript.birdcopy[birdmovementscript.poolindex].GetComponent<Rigidbody2D>().Sleep(); //Disable rigidbody of the instance

                Invoke("HandleBirdDeath", charactermovementscript.deathinterval); //Invoke the function 'HandlebirdDeath' once with the same 'deathinterval' as the player character


            }

        }

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

}
