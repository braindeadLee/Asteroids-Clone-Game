using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceshipLife : MonoBehaviour
{
    public GameObject asteroid;

    private bool _invincibility = true;
    public float InvincibilityDuration = 1.5f;
    public float InvincibilityFlickerInterval = 0.1f;

    public static event UnityAction OnDeath;

    public GameObject shipOuterLayer;


    private void Start()
    {
        StartCoroutine(InvincibilityEffect());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(asteroid.tag) && _invincibility == false)
        {
            Destroy(gameObject);
            OnDeath?.Invoke();
        }
    }
    private IEnumerator InvincibilityEffect()
    {
        _invincibility = true;
        bool flicker = true;


        float elapsed = 0f;

        while (elapsed < InvincibilityDuration)
        {

            shipOuterLayer.GetComponent<SpriteRenderer>().enabled = flicker;
            flicker = !flicker;
            yield return new WaitForSeconds(InvincibilityFlickerInterval);


            elapsed += InvincibilityFlickerInterval;
        }

        shipOuterLayer.GetComponent<SpriteRenderer>().enabled = true;
        _invincibility = false;
    }

}
