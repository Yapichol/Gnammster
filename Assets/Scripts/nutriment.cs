using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutriment : MonoBehaviour
{
    public float proteines;
    public float lipides;
    public float glucides;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            print("proteines :" + proteines + "     lipides :" + lipides + "     glucides :" + glucides);

            Destroy(this.gameObject);
        }
        if (other.tag == "Cleaner"){
            Destroy(this.gameObject);
        }
    }
}
