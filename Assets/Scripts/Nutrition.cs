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
    public static int gramsSinglePortion;

    public GameObject[] gameManagerObjects;

    public int indexFood;
   
    
    // Start is called before the first frame update
    void Start()
    {
        // gramsSinglePortion is set in the GameManager script, in Start()
        lipids = lipids * gramsSinglePortion / 100;
        proteins = proteins * gramsSinglePortion / 100;
        carbos = carbos * gramsSinglePortion / 100;         
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManagerObjects = GameObject.FindGameObjectsWithTag("GameManager");
        if (other.tag == "Player")
        {
            // The player eats the food and gets nutrients
            

            foreach (GameObject go in gameManagerObjects) {
                go.GetComponent<GameManager>().EatFood(lipids, proteins, carbos);
                go.GetComponent<GameManager>().SetSlidingWindow(indexFood, 1);
            }
            Debug.Log("eated ------> lipids :" + lipids + "     proteins: " + proteins + "carbos :" + carbos);
            Destroy(this.gameObject);
        }
        if (other.tag == "Cleaner"){
            foreach (GameObject go in gameManagerObjects)
            {
                go.GetComponent<GameManager>().SetSlidingWindow(indexFood, 0);
            }
            Destroy(this.gameObject);
        }
    }

    public void SetGramsSinglePortion(int weight)
    {
        gramsSinglePortion = weight;
    }
}
