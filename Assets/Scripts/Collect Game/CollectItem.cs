using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectItem : MonoBehaviour
{
    public static event UnityAction OnItemCollected;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Item loaded!");
        GameManager.Instance.AddCollectibleItem();

    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnItemCollected?.Invoke();
        Debug.Log("Collision message event triggered!");
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
}
