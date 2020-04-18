using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantType
{

    public static List<PlantType> plantTypes = new List<PlantType>
    {
        new PlantType("Oak", Color.white, 20, 20, 150, 200, .8f, 10, 100),
        new PlantType("Pine", new Color(0.5f, 0.5f, 0.5f), 40, 40, 100, 100, 1f, 5, 30)
    };

    public string name;

    public int maxCarbonProduction;

    public int waterRequirement;

    public int moistureAbsorbtionRate;

    public int matureTime;

    public float terrainSpeedModifier;

    public Color color;

    public int carbonBuildCost;

    public int health;

    public PlantType(string name, Color color, int waterRequirement, int moistureAbsorbtionRate, int maxCarbonProduction, int matureTime, float terrainSpeedModifier, int carbonBuildCost, int health)
    {
        this.name = name;
        this.waterRequirement = waterRequirement;
        this.moistureAbsorbtionRate = moistureAbsorbtionRate;
        this.maxCarbonProduction = maxCarbonProduction;
        this.matureTime = matureTime;
        this.terrainSpeedModifier = terrainSpeedModifier;
        this.color = color;
        this.carbonBuildCost = carbonBuildCost * 10000;
        this.health = health;
    }

    public void InitSRVisuals(SpriteRenderer sr, int age)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/trees");
        int index = Mathf.FloorToInt((sprites.Length - 1) * Mathf.Clamp01((float)age / matureTime));
        sr.sprite = sprites[index];
        sr.color = color;
    }

}
