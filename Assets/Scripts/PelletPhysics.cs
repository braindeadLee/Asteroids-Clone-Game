using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelletPhysics : MonoBehaviour
{
    public float BulletSpeed = 10f;
    public float despawnTime = 0.1f;
    public Rigidbody2D rb;

    public GameObject asteroid;

    private void Awake()
    {
        //Ignore player collision

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        Physics2D.IgnoreCollision(manager.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }
    void Start() => Invoke(nameof(Despawn), despawnTime);

    private void FixedUpdate() => rb.velocity = transform.up * BulletSpeed;

    private void Despawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(asteroid.tag))
        {
            Destroy(gameObject);
        }
    }
}
