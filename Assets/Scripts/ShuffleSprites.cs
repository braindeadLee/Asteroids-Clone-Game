using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuffleSprites : MonoBehaviour
{
    public Sprite[] sprites;                      
    public SpriteRenderer targetRenderer;

    void Start()
    {

        if (sprites.Length == 0 || targetRenderer == null) return;

    
        targetRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

    }

}
