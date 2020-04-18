using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public const int TREE_Z = 0;
    float maxHealth = 100;
    public float health;

    SpriteRenderer spriteRenderer;
    Vector3 position;

    int MAX_CARBON_PRODUCED = 100;

    int age = 1;
    const float SCALE = 1f;
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
        position = transform.position;
        Grow();
    }

    void Update()
    {

    }

    public void Attacked(float attack) {
        health -= attack;
        StartCoroutine("Shake");
        if (health < 0) {
            OnDeath();
        }
    }

    IEnumerator Shake() {
        for (float t = 1; t >= 0; t -= 0.1f) {
            transform.position = position + Mathf.Cos(10*t)/10*Vector3.right;
            yield return null;
        }
    }

    public void OnDeath()
    {
        GameController.instance.trees.Remove(this);
        Destroy(gameObject, .5f);
    }

    public void Grow()
    {
        age += 1;
        long treeSize = Mathf.FloorToInt(MAX_CARBON_PRODUCED * Mathf.Pow(1 - 1f / age, 0.1f));
       
        transform.localScale = Vector3.one * treeSize / MAX_CARBON_PRODUCED;
        GameController.instance.carbon += treeSize;
    }

    
}
