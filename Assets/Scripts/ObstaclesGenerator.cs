using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstaclesGenerator : MonoBehaviour
{
    public GameObject [] all_obstacles ;
    public GameObject cylinder_obs = null;
    public GameObject haie_obs = null;
    public GameObject ball_obs = null;
    public bool creatingObs = false;


    void Start()
    {
        all_obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
    }

    void Update()
    {
        if (creatingObs == false)
        {
            creatingObs = true;
            StartCoroutine(GenerateObs());
        }
    }

    IEnumerator GenerateObs() 
    {
        // if (Random.Range(0f,1f) < 0.3f)
        // {
        //     GameObject new_obs1 = Instantiate(cylinder_obs) as GameObject;
        //     new_obs1.transform.parent = GameObject.Find("Roue_Arch").transform;
        //     new_obs1.transform.position = new Vector3(new_obs1.transform.position.x + Random.Range(-0.57f, -0.47f) ,new_obs1.transform.position.y + 0.35f, new_obs1.transform.position.z + Random.Range(-0.07f, 0.07f));
        // }
        if (Random.Range(0f,1f) < 0.5f)
        {
            GameObject new_obs2 = Instantiate(haie_obs) as GameObject;
            new_obs2.transform.parent = GameObject.Find("Roue_Arch").transform;
            new_obs2.transform.position = new Vector3(new_obs2.transform.position.x + Random.Range(-0.57f, -0.47f) ,new_obs2.transform.position.y + 0.25f, new_obs2.transform.position.z - Random.Range(0.17f, 0.46f));
        }
        if (Random.Range(0f,1f) > 0.6f)
        {
            GameObject new_obs3 = Instantiate(ball_obs) as GameObject;
            new_obs3.transform.parent = GameObject.Find("Roue_Arch").transform;
            new_obs3.transform.position = new Vector3(new_obs3.transform.position.x + Random.Range(-0.57f, -0.47f) ,new_obs3.transform.position.y + 0.25f, new_obs3.transform.position.z - Random.Range(-0.22f, 0.22f));
        }

        yield return new WaitForSeconds(0.5f);

        all_obstacles = GameObject.FindGameObjectsWithTag("Obstacles"); // update obstacle list
        for (int i = 0; i < all_obstacles.Length; i++)
        {
            Vector3 currentObs_pos = all_obstacles[i].transform.position;
            Quaternion currentObs_rot = all_obstacles[i].transform.rotation;
            if (currentObs_pos.x > 0.888) {
                Destroy(all_obstacles[i]);
            }
        }

        creatingObs = false;
    }

}