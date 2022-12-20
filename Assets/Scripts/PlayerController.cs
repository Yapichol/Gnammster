using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject wheel;
    public float widthWheel = 1.0f;
    public Vector3 offsetWheel = new Vector3 (0, 0, 0);
    private float limRight = 0.5f;
    private float limLeft = -0.5f;

    public float speed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        if(wheel != null)
        {
            transform.position = wheel.transform.position + offsetWheel;
        }
        limRight = transform.position.z + (widthWheel / 2);
        limLeft = transform.position.z - (widthWheel / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.RightArrow) && (transform.position.z < limRight))
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, 1), speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow) && (transform.position.z > limLeft))
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, -1), speed * Time.deltaTime);
        }
    }
}
