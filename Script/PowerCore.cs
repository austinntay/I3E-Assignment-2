/*
 * Author: Austin Tay Rei Chong
 * Date:  30/6/2023
 * Description: This is a script for the power core which the player picks up
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerCore : MonoBehaviour
{
    /// <summary>
    /// Stores the score that each crystal is worth.
    /// </summary>
    public int cystalScore;
    public AudioSource audioPlayer;

    /// <summary>
    /// The function to use when the cystal is collected.
    /// </summary>
    public void Taken()
    {
        // Disable the collider after being collected.
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject);




    }

}
