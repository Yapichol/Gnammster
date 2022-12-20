using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{

    public GameObject player;
    public Vector3 offsetPlayer = new Vector3(0, 0, 0);
    public float distanceMin = 0.2f;
    public float followSpeed = 1.0f;
    public bool useAcceleration = false;

    // Start is called before the first frame update
    void Start()
    {
        if(player != null)
        {
            transform.position = player.transform.position + offsetPlayer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float acceleration = 1.0f;
        if (useAcceleration)
        {
            acceleration = Mathf.Abs(player.transform.position.z - transform.position.z) - distanceMin;
        }
        if(player.transform.position.z - transform.position.z > distanceMin)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, 1), acceleration * followSpeed * Time.deltaTime);
        }
        if (transform.position.z - player.transform.position.z > distanceMin)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, -1), acceleration * followSpeed * Time.deltaTime);
        }
    }
}
