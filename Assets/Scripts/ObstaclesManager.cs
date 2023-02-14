using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstaclesManager : MonoBehaviour
{
    public GameObject [] all_obstacles ;
    public GameObject cylinder_obs = null;
    public GameObject haie_obs = null;
    public GameObject ball_obs = null;
    public GameObject wheel = null;
    public float largeurWheel;
    public float rayonWheel;
    public static float prob_haie = 0.5f;
    public static float prob_ball = 0.3f;
    public float wheel_r;
    public bool creatingObs = false;
    public bool create;


    void Start()
    {
        all_obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        //static float cur_p_h = 0f, cur_p_b=0f;
    }


    /// <summary>
    /// Set apparition probability
    /// p_h : probability of having a haie obstacle
    /// p_b : probability of having a ball obstacle
    /// </summary>
    ///
    public static void set_prob_apparition_obs(float p_h, float p_b){
        prob_haie = p_h;
        prob_ball = p_b;
    }

    void Update()
    {
        if (create)
        {
            if (creatingObs == false)
            {
                creatingObs = true;
                StartCoroutine(GenerateObs(prob_haie, prob_ball));
            }
        }
    }

    /// <summary>
    /// Generate obstacles according to their apparition probability
    /// prob_h : probability of having a haie obstacle
    /// prob_b : probability of having a ball obstacle
    /// </summary>
    ///
    IEnumerator GenerateObs(float prob_h, float prob_b)
    {
        if (Random.Range(0f,1f) < prob_h)
        {
            GameObject new_obs2 = Instantiate(haie_obs) as GameObject;
            new_obs2.transform.parent = GameObject.Find("Roue_Arch").transform;
            new_obs2.transform.position = new Vector3(wheel.transform.position.x, wheel.transform.position.y + rayonWheel - 0.1f, wheel.transform.position.z + Random.Range(largeurWheel / -6 - 0.2f, largeurWheel / 6 - 0.2f));
        } else if (Random.Range(0f,1f) > prob_b)
        {
            GameObject new_obs3 = Instantiate(ball_obs) as GameObject;
            new_obs3.transform.parent = GameObject.Find("Roue_Arch").transform;
            new_obs3.transform.position = new Vector3(wheel.transform.position.x, wheel.transform.position.y + rayonWheel - 0.1f, wheel.transform.position.z + Random.Range(largeurWheel / -2, largeurWheel / 2));
        }

        yield return new WaitForSeconds(0.5f);

        all_obstacles = GameObject.FindGameObjectsWithTag("Obstacles"); // update obstacle list, remove old ones
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