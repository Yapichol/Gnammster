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


    private void OnTriggerStay2D(Collider2D collision)
    {
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        if (collision.tag == "gaugesZone") 
        {
            //Debug.Log(collision.tag);
            gm.lookedAtGauges();
        } 
        
        else if (collision.tag == "scoreZone")
        {
            //Debug.Log(collision.tag);
        }
        
        else if (collision.tag == "timingZone")
        {
            //Debug.Log(collision.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("exit");
    }



}
