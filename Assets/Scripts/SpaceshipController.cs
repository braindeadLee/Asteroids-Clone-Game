using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;
public class SpaceshipController : MonoBehaviour
{
    public Rigidbody2D Rb;

    private bool _shouldMoveForward;
    private bool _shouldRotateLeft;
    private bool _shouldRotateRight;

    //public float MoveSpeed = 3f;
    public float RotationSpeed = 1f;
    public float AccelerationSpeed = 1.5f;
    //public float DeaccelerationSpeed = 2f;
    //public float CurrentSpeed = 0f;
    public float MaxSpeed = 3f;

    private Vector3 LastUpDirection;

    void Start()
    {
        Debug.Log("Screen Width: " + Screen.width);
        Debug.Log("Screen Height: " + Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.wKey.IsPressed())
        {
            _shouldMoveForward = true;
        }
            
        else if (keyboard.wKey.wasReleasedThisFrame)
            _shouldMoveForward = false;

        if (keyboard.aKey.IsPressed())
            _shouldRotateLeft = true;
        else if (keyboard.aKey.wasReleasedThisFrame)
            _shouldRotateLeft = false;

        if (keyboard.dKey.IsPressed())
            _shouldRotateRight = true;
        else if (keyboard.dKey.wasReleasedThisFrame)
            _shouldRotateRight = false;

    }

    private void FixedUpdate()
    {
        if (_shouldMoveForward)
        {
            Rb.AddForce(transform.up * AccelerationSpeed);
        }
        
        if(Rb.velocity.magnitude > MaxSpeed)
        {
            Rb.velocity = Rb.velocity.normalized * MaxSpeed;
        }

        if (_shouldRotateLeft)
            Rb.rotation += RotationSpeed * Time.deltaTime;
            
        if (_shouldRotateRight)
            Rb.rotation -= RotationSpeed * Time.deltaTime;
    }
}

//old way
//private void FixedUpdate()
//{
//    if (_shouldMoveForward)
//    {
//        CurrentSpeed += AccelerationSpeed * Time.deltaTime;
//        LastUpDirection = transform.up;
//    }
//    else
//    {
//        CurrentSpeed -= DeaccelerationSpeed * Time.deltaTime;
//    }

//    CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0, MaxSpeed);

//    Rb.velocity = (Vector2)LastUpDirection * CurrentSpeed;

//    if (_shouldRotateLeft)
//        Rb.rotation += RotationSpeed * Time.deltaTime;

//    if (_shouldRotateRight)
//        Rb.rotation -= RotationSpeed * Time.deltaTime;
//}
//}