using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public const int TREE_Z = 0;

    SpriteRenderer spriteRenderer;

    int MAX_CARBON_PRODUCED = 100;

    int age = 1;
    const float SCALE = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    public void Grow()
    {
        age += 1;
        GameController.instance.carbon += Mathf.FloorToInt(MAX_CARBON_PRODUCED * Mathf.Pow(1 - 1f / age, 0.1f));
    }
}
