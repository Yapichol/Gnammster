using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private float _speed = 25f;
    private Animator m_animator;

    void Start(){
        // Handle animations through animator state machine
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.UpArrow)) _rotation = Vector3.up;
        else if (Input.GetKey(KeyCode.DownArrow)) _rotation = Vector3.down;
        else _rotation = Vector3.zero;
        */
        transform.Rotate(_rotation * _speed * Time.deltaTime);
    }
}
