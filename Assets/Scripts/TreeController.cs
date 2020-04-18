using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public const int TREE_Z = 0;
    public float maxHealth = 100;
    public float health;
    public float flammability;
    public bool onFire = false;
    public SpriteRenderer spriteRenderer;
    Vector3 position;

    int MAX_CARBON_PRODUCED = 100;

    int age = 1;
    const float SCALE = 1f;
    public Sprite[] sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprites[(int)(Random.value * sprites.Length)];
       
        health = maxHealth;
        position = transform.position;
        transform.position = new Vector3(position.x, position.y, position.y/10);
        Grow();
    }

    void Update()
    {
        if (health < 0)
        {
            OnDeath();
        }
    }

    public void Attacked(float attack) {
        health -= attack;
        StartCoroutine("Shake");
        
    }

    IEnumerator Shake() {
        for (float t = 1; t >= 0; t -= 0.1f) {
            transform.position = position + Mathf.Cos(10*t)/10*Vector3.right;
            yield return null;
        }
    }

    IEnumerator Fade()
    {
        float fadeTime = 10;
        Color c = spriteRenderer.color;
        for (float t = fadeTime; t >= 0; t -= 0.1f)
        {
            spriteRenderer.color = c * t/fadeTime;
            yield return null;
        }
    }

    public void OnDeath()
    {
        GameController.instance.trees.Remove(this);
        if (onFire) {
            StartCoroutine("Fade");
            Destroy(gameObject, 5f);
            return;
        }
        
        
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
