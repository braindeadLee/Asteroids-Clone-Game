using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPhysics : MonoBehaviour
{
    //public PelletPhysics bullet;

    public int _asteroidType = 0;
    public float _asteroidSpeed = 1f;

    public Rigidbody2D rb;

    public float _asteroidScale = 1f;

    public GameObject projectile;


    void Start()
    {
        rb.velocity = transform.up * _asteroidSpeed;
        transform.localScale *= _asteroidScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(projectile.tag))
        {
            Manager.Instance.enemyManager.OnAsteroidDeath(_asteroidScale, transform.position,transform.rotation, _asteroidSpeed, _asteroidType);
            Destroy(gameObject);
        }
    }

}

