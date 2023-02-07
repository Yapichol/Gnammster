using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nutriment : MonoBehaviour
{
    public float sugar;
    public float salt;
    public float fat;
    
    
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
            print("Sugar :" + sugar + "     Salt :" + salt + "     Fat :" + fat);

            Destroy(this.gameObject);
        }
        if (other.tag == "Cleaner"){
            Destroy(this.gameObject);
        }
    }
}
