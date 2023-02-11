using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackerCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "gaugesZone") 
        {
            Debug.Log(collision.tag);
        } 
        
        else if (collision.tag == "scoreZone")
        {
            Debug.Log(collision.tag);
        }
        
        else if (collision.tag == "timingZone")
        {
            Debug.Log(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
    }



}
