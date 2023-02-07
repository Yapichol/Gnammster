using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodManager : MonoBehaviour
{
    public GameObject wheel;
    public float largeurWheel;
    public float rayonWheel;
    public GameObject[] prefabsFoods;
    public float freqSpawnMin;
    public float freqSpawnMax;

    private bool canSpawn;

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            spawnFood();
            canSpawn = false;
            float timeNextSpawn = Random.Range(freqSpawnMin, freqSpawnMax);
            StartCoroutine(coolDown(timeNextSpawn));
        }
    }



    void spawnFood()
    {
        int numFood = selectFoodToSpawn();

        GameObject newFood = Instantiate(prefabsFoods[numFood]);

        Vector3 spawnPos = selectPosToSpawn();

        newFood.transform.position = spawnPos;

        newFood.transform.parent = wheel.transform;
    }



    int selectFoodToSpawn()
    {
        int number = Random.Range(0, prefabsFoods.Length - 1);
        return number;
    }


    Vector3 selectPosToSpawn()
    {
        Vector3 position = new Vector3(wheel.transform.position.x, wheel.transform.position.y + rayonWheel - 0.1f, wheel.transform.position.z + Random.Range(largeurWheel / -2, largeurWheel / 2));
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
