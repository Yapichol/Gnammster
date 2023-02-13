using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public GameObject wheel;
    public float largeurWheel;
    public float rayonWheel;
    public GameObject[] prefabsFoods;
    public float freqSpawnMin;
    public float freqSpawnMax;

    public bool randomSpawn;
    private bool canSpawn;

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
        randomSpawn = false;
        freqSpawnMin = 0;
        freqSpawnMax = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && randomSpawn)
        {
            spawnFood(-1);
            canSpawn = false;
            float timeNextSpawn = Random.Range(freqSpawnMin, freqSpawnMax);
            StartCoroutine(coolDown(timeNextSpawn));
        }
    }



    public void spawnFood(int foodNumber)
    {
        int numFood = foodNumber;
        if (randomSpawn || (foodNumber < 0 || foodNumber >= prefabsFoods.Length))
        {
            numFood = selectFoodToSpawn();
        }

        GameObject newFood = Instantiate(prefabsFoods[numFood]);

        Vector3 spawnPos = selectPosToSpawn();

        newFood.transform.position = spawnPos;

        newFood.transform.rotation = Quaternion.Euler(0, 0, -90);

        newFood.transform.parent = wheel.transform;

        newFood.GetComponent<Nutrition>().indexFood = numFood;
    }



    int selectFoodToSpawn()
    {
        int number = Random.Range(0, prefabsFoods.Length - 1);
        return number;
    }


    Vector3 selectPosToSpawn()
    {
        Vector3 position = new Vector3(wheel.transform.position.x -( rayonWheel - 0.1f), wheel.transform.position.y , wheel.transform.position.z + Random.Range(largeurWheel / -2, largeurWheel / 2));
        return position;
    }



    private IEnumerator coolDown(float duree)
    {
        float timer = 0;
        while (timer < duree)
        {
            timer = timer + Time.deltaTime;
            yield return null;
        }
        canSpawn = true;
    }

}
