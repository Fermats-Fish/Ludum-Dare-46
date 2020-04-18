using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public const int TREE_Z = 0;

    SpriteRenderer spriteRenderer;

    float size = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public void Grow()
    {
        size = (size + 1) / size;
        spriteRenderer.size = new Vector2(size, size);
    }
}
