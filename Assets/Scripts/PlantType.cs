using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantType
{

    public static List<PlantType> plantTypes = new List<PlantType>
    {
        //            Name                Color,                   Water Requirement, Surpless Water Prod, Carbon Prod, Mature Time, Speed Mod, Build Cost, Helath, Init Spawn Weight
        new PlantType("Oak",              Color.white,             20,                100,                 150,         200,          0.8f,     20,         300,      1f),
        new PlantType("Pine",         new Color(0.5f, 0.5f, 0.5f), 40,                  0,                 100,         100,          1f,       10,          90,      1f),
        new PlantType("Fruit Tree",   new Color(0.7f, 0f, 0.7f),   50,                 20,                  50,         300,          0.9f,     35,         600,      0.1f)
    };

    public string name;

    public int maxCarbonProduction;

    public int surplessWaterProd;

    public int waterRequirement;

    public int matureTime;

    public float terrainSpeedModifier;

    public Color color;

    public int carbonBuildCost;

    public int health;

    public float initSpawnWeight;

    public PlantType(string name, Color color, int waterRequirement, int surplessWaterProd, int maxCarbonProduction, int matureTime, float terrainSpeedModifier, int carbonBuildCost, int health, float initSpawnWeight)
    {
        this.name = name;
        this.surplessWaterProd = surplessWaterProd;
        this.waterRequirement = waterRequirement;
        this.maxCarbonProduction = maxCarbonProduction;
        this.matureTime = matureTime;
        this.terrainSpeedModifier = terrainSpeedModifier;
        this.color = color;
        this.carbonBuildCost = carbonBuildCost * 1000;
        this.health = health;
        this.initSpawnWeight = initSpawnWeight;
    }

    public void InitSRVisuals(SpriteRenderer sr, int age)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/trees");
        int index = Mathf.FloorToInt((sprites.Length - 1) * Mathf.Clamp01((float)age / matureTime));
        sr.sprite = sprites[index];
        sr.color = color;
    }

    public static PlantType GetRandomPlantType()
    {
        if (plantTypes.Count == 0)
        {
            Debug.LogError("Trying to get a random plant type when there are none.");
            return null;
        }

        // Sum the spawn weights.
        var totalWeight = 0f;
        foreach (var plantType in plantTypes)
        {
            totalWeight += plantType.initSpawnWeight;
        }

        // Get random number.
        var x = Random.Range(0f, totalWeight);

        // Loop again.
        foreach (var plantType in plantTypes)
        {
            x -= plantType.initSpawnWeight;
            if (x <= 0)
            {
                return plantType;
            }
        }

        // Should never get here.
        Debug.LogError("Something weird happened when getting a random plant type");
        return plantTypes[plantTypes.Count - 1];

    }

}
