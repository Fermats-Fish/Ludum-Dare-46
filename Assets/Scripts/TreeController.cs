using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    float maxHealth = 100;
    public float health;

    SpriteRenderer spriteRenderer;
    Vector3 position;

    int age = 1;
    const float SCALE = 1f;

    public PlantType plantType;

    public void Initialise(PlantType plantType)
    {
        // Add to the list of trees.
        GameController.instance.trees.Add(this);

        // Set the plant type.
        this.plantType = plantType;

        // Set the sprite.
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantType.InitSRVisuals(spriteRenderer);

        // Init some things.
        health = maxHealth;
        position = transform.position;
        transform.position = new Vector3(position.x, position.y, position.y / 10);
        Grow();

    }

    void Start()
    {
    }

    void Update()
    {

    }

    public void Attacked(float attack)
    {
        health -= attack;
        StartCoroutine("Shake");
        if (health < 0)
        {
            OnDeath();
        }
    }

    IEnumerator Shake()
    {
        for (float t = 1; t >= 0; t -= 0.1f)
        {
            transform.position = position + Mathf.Cos(10 * t) / 10 * Vector3.right;
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
        long treeSize = Mathf.FloorToInt(plantType.maxCarbonProduction * Mathf.Pow(1 - 1f / age, 0.1f));

        transform.localScale = Vector3.one * treeSize / plantType.maxCarbonProduction;
        GameController.instance.carbon += treeSize;
    }


}
