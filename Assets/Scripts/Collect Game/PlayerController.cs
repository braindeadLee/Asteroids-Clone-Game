using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{ 
    private bool _shouldMoveForward;

    public Rigidbody2D Rb;
    public float MoveSpeed = 10f;
    public float SpriteRotationOffset = -90f;
    public float LookAtSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;

        //Keyboard connected?
        if (keyboard == null || mouse == null)
            return; // No - stop running code.

        if (keyboard.spaceKey.IsPressed() || mouse.leftButton.IsPressed())
        {
            // Move while holding spacebar key or left mouse button down.
            _shouldMoveForward = true;
            Debug.Log("Im being pressed!");
        }
        else if (keyboard.spaceKey.wasReleasedThisFrame || mouse.leftButton.wasReleasedThisFrame)
        {
            // The spacebar key was released - stop moving
            _shouldMoveForward = false;
        }

        LookAtMousePointer();
    }

    private void FixedUpdate()
    {
        if (_shouldMoveForward)
        {
            // Process physics movement.
            // Up is the direction the object sprite is currently facing.
            Rb.velocity = transform.up * MoveSpeed;
        }
        else
        {
            //Stop movement.
            Rb.velocity = Vector2.zero;
        }
    }

    private void LookAtMousePointer()
    {
        var mouse = Mouse.current;
        if (mouse == null) 
            return;

        var mousePos = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
        
        var direction = (Vector2)mousePos - Rb.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + SpriteRotationOffset;

        //Direct rotation.
        //Rb.rotation = angle;

        // Interpolated rotation - smoothed.
        // Forward (Z-axis) is what we want to rotate on.
        var q = Quaternion.AngleAxis(angle, Vector3.forward);
        Rb.transform.rotation = Quaternion.Slerp(Rb.transform.rotation, q, Time.deltaTime * LookAtSpeed);


        //Debug.Log($"mouse={mouse}, mousePos={mousePos} direction={direction}, angle={angle}, q={q}, Rb.transform.rotation={Rb.transform.rotation}");
    }

    [Header("Hit Hazard Speed")]

    public float SlowedSpeed = 2f;
    public float SlowedTime = 5f;

    private float _speedStartValue;

    private void Start()
    {
        _speedStartValue = MoveSpeed;
    }

    public void SlowPlayerSpeed()
    {
        MoveSpeed = SlowedSpeed;
        Invoke(nameof(RestoreSpeed), SlowedTime);
    }

    private void RestoreSpeed()
    {
        MoveSpeed = _speedStartValue;
    }

}
