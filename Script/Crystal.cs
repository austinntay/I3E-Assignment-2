/*
 * Author: AustinTayReiChong
 * Date: 
 * Description: This is a simple script on the collection of the crystals
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    /// <summary>
    /// Stores the score that each crystal is worth.
    /// </summary>
    public int cystalScore;

    /// <summary>
    /// The function to use when the cystal is collected.
    /// </summary>
    public void Collected()
    {
        // Disable the collider after being collected.
        GetComponent<Collider>().enabled = false;


        // Play the collected animation.
        GetComponent<Animator>().SetTrigger("Collected");

  

    }

   

    /// <summary>
    /// The function used to destroy the crystal.
    /// </summary>
    public virtual void DestroyCrystal()
    {
        Destroy(gameObject);
        Debug.Log("destroyed");
    }
}
