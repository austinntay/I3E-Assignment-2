/*
 * Author: AustinTayReiChong
 * Date: 
 * Description: This is a simple script on the collection of the crystals
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    /// <summary>
    /// Stores the score that each crystal is worth.
    /// </summary>
    public int cystalScore;

    /// <summary>
    /// The function to use when the cystal is collected.
    /// </summary>
    public void Unlocked()
    {
        // Disable the collider after being collected.
        GetComponent<Collider>().enabled = false;


        // Play the open animation.
        GetComponent<Animator>().SetTrigger("Unlocked");



    }



   
}
