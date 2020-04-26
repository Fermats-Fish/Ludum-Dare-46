using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MouseOverObject
{
    public float maxHealth;
    public float health;
    public bool onFire = false, beingChoppedDown = false;

    public SpriteRenderer spriteRenderer;
    Vector3 position;

    int age;
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
        plantType.InitSRVisuals(spriteRenderer, age);

        // Init some things.
        maxHealth = plantType.health;
        health = maxHealth;
        position = transform.position;
        transform.position = new Vector3(position.x, position.y, position.y / 10);

        SetAge(1);

    }

    void Start()
    {
    }

    void Update()
    {
        if (health < 0)
        {
            OnDeath();
        }
    }

    public void Attacked(float attack)
    {
        health -= attack;
        UpdateMouseOverText();
        StartCoroutine("Shake");
        if (health < 0)
        {
            // Yes this will only play the very start of the clip, but the whole thing is too much anyway for how often this happens and I'm too tired to impliment now.
            // (It only plays start because it immediately dies. Would have to use AudioSource.PlayAtPoint or whatever it is, and then find a way to lower the volume on that.)
            GetComponentInChildren<AudioSource>().Play();
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

    IEnumerator Fade()
    {
        float fadeTime = 10;
        Color c = spriteRenderer.color;
        for (float t = fadeTime; t >= 0; t -= 0.1f)
        {
            spriteRenderer.color = c * t / fadeTime;
            yield return null;
        }
    }

    public void OnDeath()
    {
        RemoveMouseOverGO();

        GameController.instance.trees.Remove(this);
        if (onFire)
        {
            StartCoroutine("Fade");
            Destroy(gameObject, 10f);
            return;
        }

        Destroy(gameObject, .5f);
    }

    public void Grow()
    {
        if (!(onFire || beingChoppedDown))
        {
            SetAge(age + 1);

            GameController.instance.AddToCarbon(GetCarbonProd());
            GameController.instance.AddToWater(GetWaterProd());
        }
    }

    long GetCarbonProd()
    {
        return Mathf.FloorToInt(plantType.maxCarbonProduction * Mathf.Pow(1 - 100f / (100f + plantType.matureTime * age), 10));
    }

    long GetWaterProd()
    {
        return Mathf.FloorToInt(plantType.surplessWaterProd * Mathf.Pow(1 - 100f / (100f + plantType.matureTime * age), 10));
    }

    public void SetAge(int newAge)
    {
        age = newAge;

        // Visuals might have changed.
        plantType.InitSRVisuals(spriteRenderer, age);

        // Change mouse over.
        UpdateMouseOverText();
    }

    protected override string GetMouseOverText()
    {
        return $"{plantType.name}:"
            + $"\nCarbon Production: {GetCarbonProd()}/{plantType.maxCarbonProduction}"
            + $"\nWater Production: {GetWaterProd()}/{plantType.surplessWaterProd}"
            + $"\nHealth: {health}/{plantType.health}"
            + $"\nMaturity: {(100 * age) / plantType.matureTime}%"
            + $"\nFlamability: {(100 * age) / plantType.matureTime}%"
        ;
    }

}
