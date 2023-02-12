using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nutrition : MonoBehaviour
{
    // Nutrition values for 100 grams
    public float lipids;
    public float proteins;
    public float carbos;

    // Weight of one portion of food used for the game (grams) 
    private int gramsSinglePortion;

    public GameObject[] gameManagerObjects;
   
    
    // Start is called before the first frame update
    void Start()
    {
        gramsSinglePortion = 15; // je n'arrive pas à le set depuis game manager, il faudrait

        // gramsSinglePortion is set in the GameManager script, in Start()
        lipids = lipids * gramsSinglePortion / 100;
        proteins = proteins * gramsSinglePortion / 100;
        carbos = carbos * gramsSinglePortion / 100;         
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // The player eats the food and gets nutrients
            gameManagerObjects = GameObject.FindGameObjectsWithTag("GameManager");

            foreach (GameObject go in gameManagerObjects) {
                go.GetComponent<GameManager>().EatFood(lipids, proteins, carbos);
            }
            Debug.Log("eated ------> lipids :" + lipids + "     proteins: " + proteins + "carbos :" + carbos);
            Destroy(this.gameObject);
        }
        if (other.tag == "Cleaner"){
            Destroy(this.gameObject);
        }
    }

    public void SetGramsSinglePortion(int weight)
    {
        gramsSinglePortion = weight;
    }
}
