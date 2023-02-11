using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutriment : MonoBehaviour
{
    public float proteines;
    public float lipides;
    public float glucides;

    public GameObject[] gameManagerObjects;
   
    
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
            // The player eats the food and gets nutrients
            gameManagerObjects = GameObject.FindGameObjectsWithTag("GameManager");
            foreach (GameObject go in gameManagerObjects) {
                go.GetComponent<GameManager>().EatFood(lipides, proteines, glucides);
            }
            
            Debug.Log("eated ------> proteines :" + proteines + "     lipides :" + lipides + "     glucides :" + glucides);

            Destroy(this.gameObject);
        }
        if (other.tag == "Cleaner"){
            Destroy(this.gameObject);
        }
    }
}
