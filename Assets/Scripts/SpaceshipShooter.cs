using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipShooter : MonoBehaviour
{

    private bool _cooldownFinished = true;

    private float _cooldownSeconds = 0.1f;

    public GameObject projectile;
    public Transform gunbarrel;

    //public GameObject bullet;
    private void Awake()
    {
        //GameObject bullet = GameObject.FindGameObjectWithTag("Bullet");
        Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
    }
    void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        if (keyboard.spaceKey.wasPressedThisFrame && _cooldownFinished)
        {
            Shoot();

            //Activate cooldowns
            _cooldownFinished = false;
            Invoke(nameof(DeactivateCooldown), _cooldownSeconds);


        }

    }
    private void Shoot() => Instantiate(projectile, gunbarrel.position, gunbarrel.rotation);

    private void DeactivateCooldown()
    {
        _cooldownFinished = true;

    }

}

