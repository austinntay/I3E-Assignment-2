/*
 * Author: Austin Tay Rei Chong
 * Date:  30/6/2023
 * Description: This is a script which contains all interactions and movements of the player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/// <summary>
/// The class responsible for the player mechanics.
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// The Vector3 used to store the WASD input of the user.
    /// </summary>
    Vector3 movementInput = Vector3.zero;

    /// <summary>
    /// The Vector3 used to store the left/right mouse input of the user.
    /// </summary>
    Vector3 rotationInput = Vector3.zero;

    /// <summary>
    /// The movement speed of the player per second.
    /// </summary>
    float moveSpeed = 7f;

    /// <summary>
    /// The speed at which the player rotates
    /// </summary>
    float rotationSpeed = 30f;

    /// <summary>
    /// The text object used to display the current score, dialog , and the player's inner thoughts.
    /// </summary>
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI Dialog;
    public TextMeshProUGUI PlayerSpeech;
   


    /// <summary>
    /// The text object used to hold multiple prompts for the player.
    /// The Object for the power core that the player is holding
    /// The photos for the staion master and the background for his text
    /// The trigger for which the player steps in at the start to trigger the intro dialog
    /// </summary>
    public GameObject congratsMessage;
    public GameObject Prompt;
    public GameObject Station;
    public GameObject BackGround;
    public GameObject CrystalsOnDoor;
    public GameObject DoorPrompt;
    public GameObject NotEnoughCrystalsPrompt;
    public GameObject PlayerEnemy;
    public GameObject PlayerDeath;
    public GameObject PowerCoreOnPlayer;
    public GameObject StartTrigger;
    public GameObject PowerCoreStatusMissing;
    public GameObject PowerCoreStatusActive;
    public GameObject PowerCoreLightMissing;
    public GameObject PowerCoreLightActive;




    //Audio source for the player//
    public AudioSource audioPlayer;

    /// <summary>
    /// The current score of the player.
    /// </summary>
    public int playerScore = 0;

    /// <summary>
    /// Tracks the number of crystal collected.
    /// </summary>
    int crystalCollected = 0;

    /// <summary>
    /// The door that will be unlocked after some coins are collected.
    /// </summary>
    public Rigidbody lockedDoor;

    /// <summary>
    /// Tracks whether the player is dead or not.
    /// </summary>
    bool isDead = false;
    /// <summary>
    /// sets the distance for which the player can interact with objects
    /// </summary>
    float interactionDistance = 4f;
    /// <summary>
    /// the player isnt interacting on default unless he presses E
    /// </summary>
    bool interact = false;
    /// <summary>
    /// A bool for when the player collects the power core at the end of the game
    /// </summary>
    bool PowerCoreCollected = false; 

    Vector3 headRotationInput;

    public GameObject playerCamera;

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            // Create a new Vector3
            Vector3 movementVector = Vector3.zero;

            // Add the forward direction of the player multiplied by the user's up/down input.
            movementVector += transform.forward * movementInput.y;

            // Add the right direction of the player multiplied by the user's right/left input.
            movementVector += transform.right * movementInput.x;

            // Apply the movement vector multiplied by movement speed to the player's position.
            transform.position += movementVector * moveSpeed * Time.deltaTime;

            // Apply the rotation multiplied by the rotation speed.
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationInput * rotationSpeed * Time.deltaTime);
            playerCamera.transform.rotation = Quaternion.Euler(playerCamera.transform.rotation.eulerAngles + headRotationInput * rotationSpeed * Time.deltaTime);

            Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + (playerCamera.transform.forward * interactionDistance));
            RaycastHit hitInfo;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hitInfo, interactionDistance))
            {
               Debug.Log(hitInfo.transform.name);
                if (hitInfo.transform.tag == "Cryst1")
                {
                    Prompt.SetActive(true);
                    if (interact)
                    {
                        Prompt.SetActive(false);
                        //tells the crysyal to be collected//
                        hitInfo.transform.GetComponent<Crystal>().Collected();

                        // Increase the player score.
                        playerScore += 1;

                        // Increase the number of coins collected.//
                        ++crystalCollected;

                        // Update the displayed score
                        displayText.text = "Crystals           " + crystalCollected + "/4";

                        //play collected sound//
                        audioPlayer.Play();

                        if (crystalCollected >= 4)
                        {
                            StartCoroutine(CongratsPrompt());
                        }
                    }
                }

                //when player hits the final door, if they have all 4 crystals , play door open anim//
                else if (hitInfo.transform.tag == "FinalDoor")
                {
                    if (crystalCollected >= 4)
                    {
                        DoorPrompt.SetActive(true);
                        if (interact)
                        {
                            CrystalsOnDoor.SetActive(true);
                            hitInfo.transform.GetComponent<DoorOpen>().Unlocked();
                            audioPlayer.Play();
                        }

                    }
                    //when player hits the final door, if they do not have 4 crystals, the player will be prompted to collect all 4 first//
                    else if (crystalCollected < 4)
                    {
                        NotEnoughCrystalsPrompt.SetActive(true);
                    }
                }
                else if (hitInfo.transform.tag == "PowerCore")
                {
                    Prompt.SetActive(true);

                    if (interact)
                    {
                        hitInfo.transform.GetComponent<PowerCore>().Taken();
                        Prompt.SetActive(false);
                        PowerCoreOnPlayer.SetActive(true);
                        StartCoroutine(PowerCore());
                        PowerCore();
                        PowerCoreCollected = true;
                    }
                }

                
            }

            else if (PowerCoreCollected)
            {
                PowerCoreStatusMissing.SetActive(false);
                PowerCoreStatusActive.SetActive(true);
                PowerCoreLightMissing.SetActive(false);
                PowerCoreLightActive.SetActive(true);  
            }


            else if (PowerCoreCollected)
            {
                StartTrigger.SetActive(false);
            }
            else
            {
                Prompt.SetActive(false);
                DoorPrompt.SetActive(false);
                NotEnoughCrystalsPrompt.SetActive(false);
            }
        }

      

        interact = false;
    }

    void Start()
    {
        isDead = false;
    }


//use to show the intro dialog when the player steps in the trigger//

    public IEnumerator IntroDialog()
    {
        Dialog.text = "This is control, your spaceship's power core was lost when entering this planet's atmosphere.";
        Station.SetActive(true);
        BackGround.SetActive(true);
        yield return new WaitForSeconds(4f);
        Dialog.text = "scanners show a functional power core behind a locked door in that cave.";
        yield return new WaitForSeconds(4f);
        Dialog.text = "you're going to need to find 4 gems to open that door, good luck.";
        yield return new WaitForSeconds(6f);
        Dialog.text = "";
        Station.SetActive(false);
        BackGround.SetActive(false);
    }

    //use to show the final dialog when the player is on his way out of the cave//

    public IEnumerator FinalDialog()
    {
        Dialog.text = "You found the core, bring it back to the spaceship and you can finally go home.";
        Station.SetActive(true);
        BackGround.SetActive(true);
        yield return new WaitForSeconds(5f);
        Dialog.text = "";
        Station.SetActive(false);
        BackGround.SetActive(false);
    }

    /// <summary>
    /// used to show after the player picks up the power core//
    /// </summary>
    /// <returns></returns>
    public IEnumerator PowerCore()
    {
        PlayerSpeech.text = "Got the power core, time to return to my spaceship.";
        yield return new WaitForSeconds(5f);
        PlayerSpeech.text = "";

    }


    /// <summary>
    /// shows the congrats message, waits for a few seconds then removes it//
    /// </summary>
    /// <returns></returns>
    public IEnumerator CongratsPrompt()
    {
        congratsMessage.SetActive(true);
        yield return new WaitForSeconds(3f);
        congratsMessage.SetActive(false);

    }
    /// <summary>
    /// plays when the player steps on the prompt for being near an enemy//
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemyPrompt()
    {
        PlayerEnemy.SetActive(true);
        yield return new WaitForSeconds(4f);
        PlayerEnemy.SetActive(false);

    }

    /// <summary>
    /// Called when the object collides with another object.
    /// </summary>
    /// <param name="collision">Holds the information of the collision.</param>
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "InstaKill")
        {
            KillPlayer();
        }

    }

    
    //if player exits the range of the certain promt triggers, remove those triggers//
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        Prompt.SetActive(false);

    }

    /// <summary>
    /// used to jump//
    /// </summary>
    void OnEKeyboard()
    {
        GetComponent<Rigidbody>().
            AddForce(new Vector3(0, 7, 0), ForceMode.Impulse);
    }

   
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "InstaKill")
        {
            KillPlayer();
        }

    }

    private void OnTriggerStay(Collider collision)
    {
       
        
        //used to show the intro dialog and remove the trigger so it only plays once//
        if (collision.gameObject.tag == "Starting")
        {
            StartCoroutine(IntroDialog());
            IntroDialog();
            collision.gameObject.SetActive(false);
        }

        //used to show the prompt when the player goes near an enemy//
        else if (collision.gameObject.tag == "NearEnemy")
        {
            StartCoroutine(EnemyPrompt());
            EnemyPrompt();
            collision.gameObject.SetActive(false);

        }
        //when the player goes near the blocked path//
        else if (collision.gameObject.tag == "BlockedOff")
        {
            PlayerSpeech.text = "Dang rocks , can't go there.";
        }

        else if (collision.gameObject.tag == "Final")
        {
            StartCoroutine(FinalDialog());
            FinalDialog();
            collision.gameObject.SetActive(false);
        }

       

    }

       
    

    //remove any prompts when the player exits the range of the triggers//
    private void OnTriggerExit(Collider collision)
    {
        Prompt.SetActive(false);
        PlayerSpeech.text = "";
      

    }



    /// <summary>
    /// Used to kill the player.
    /// </summary>
    void KillPlayer()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("Death");
        PlayerDeath.SetActive(true);
    }
    
 

    /// <summary>
    /// Called when the Look action is detected.
    /// </summary>
    /// <param name="value"></param>
    void OnLook(InputValue value)
    {
        if (!isDead)
        {
            rotationInput.y = value.Get<Vector2>().x;
            headRotationInput.x = value.Get<Vector2>().y * -1;

        }
    }

    /// <summary>
    /// Called when the Move action is detected.
    /// </summary>
    /// <param name="value"></param>
    void OnMove(InputValue value)
    {
        if (!isDead)
        {
            movementInput = value.Get<Vector2>();
        }
    }
    /// <summary>
    /// when the player presses E
    /// </summary>
    void OnInteract()
    {
      interact = true;
    }
    
}