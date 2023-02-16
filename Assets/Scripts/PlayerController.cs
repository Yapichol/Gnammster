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

    public float jumpHeight = 1.0f;
    public float speedJump = 1.0f;
    private float groundHeight = 0.0f;
    private bool jumping = false;
    private float targetJump = 1.0f;

    public bool useJumpAcceleration = true;
    public float accelerationSpeed = 0.1f;
    private float acceleration = 1.0f;

    public bool activeHover = false;
    public float coefHover = 1.0f;

    public bool useProgressiveJump = false;
    public float maxProgJump = 1.0f;

    private Animator m_animator;
    private string[] m_animations = new string[] { "Idle", "Run", "Dead" };

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {

        GameObject [] test =  GameObject.FindGameObjectsWithTag("Obstacles");

        if(wheel != null)
        {
            transform.position = wheel.transform.position + offsetWheel;
        }
        limRight = transform.position.z + (widthWheel / 2);
        limLeft = transform.position.z - (widthWheel / 2);
        groundHeight = transform.position.y;
        m_animator = GetComponent<Animator>(); // Handle animations through animator state machine
        ObstaclesManager.set_prob_apparition_obs(0.5f, 0.5f);


    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < groundHeight)
        {
            transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
        }
        if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ) && (transform.position.z < limRight))
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, 1), speed * Time.deltaTime);
        }
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q)) && (transform.position.z > limLeft))
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0, -1), speed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space) && (transform.position.y <= groundHeight))
        {
            //Debug.Log("Peut sauter");
            jumping = true;
            targetJump = jumpHeight;
            if (anim != null)
            {
                anim.SetBool("Jump", true);
            }
            //transform.position.y = transform.position.y + 0.1f;
        }
        if (jumping)
        {
            if(useProgressiveJump && Input.GetKey(KeyCode.Space) && (targetJump < jumpHeight + maxProgJump))
            {
                targetJump = targetJump + 0.7f * Time.deltaTime;
            }
            if (transform.position.y < groundHeight + targetJump)
            {
                acceleration = acceleration - accelerationSpeed * Time.deltaTime;
                if ((transform.position.y < groundHeight + targetJump - jumpHeight / 2) || (useJumpAcceleration == false))
                {
                    acceleration = 1.0f;
                }
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 1, 0), acceleration * speedJump * Time.deltaTime);
            }
            else
            {
                jumping = false;
                acceleration = 0.0f;
            }
        }
        else
        {
            if(transform.position.y > groundHeight)
            {
                acceleration = acceleration + accelerationSpeed * Time.deltaTime;
                if((transform.position.y < groundHeight + targetJump - jumpHeight / 2) || (useJumpAcceleration == false))
                {
                    acceleration = 1.0f;
                }
                if(activeHover && Input.GetKey(KeyCode.Space))
                {
                    acceleration = 1 / coefHover;
                }
                //Debug.Log(acceleration);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, -1, 0), acceleration * speedJump * Time.deltaTime);
            }
            else
            {
                if (anim != null)
                {
                    anim.SetBool("Jump", false);
                }
                //acceleration = 1.0f;
            }
        }
        if (anim != null) {
            anim.SetFloat("SpeedRunning", wheel.GetComponentInChildren<Rotator>()._speed * 0.75f);
        }
    }

    // Handle collisions & effects here
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Obstacles"))
        {
            //Debug.Log("Collision  !!!");
            other.gameObject.SetActive(false);
            this.gameObject.GetComponent<PlayerHealth>().getHurt();
            //other.GetComponent<Renderer>().enabled = false;
        }
    }
}
