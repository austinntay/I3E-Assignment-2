using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{

    public GameObject CrystalOnPlayer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                this.gameObject.SetActive(false);
                
            }
        }
    }
}
