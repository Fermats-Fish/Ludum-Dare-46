using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantType
{

    public static List<PlantType> plantTypes = new List<PlantType>
    {
        new PlantType("Oak", Color.white, 20, 20, 100, 200, 1f),
        new PlantType("Pine", new Color(0.5f, 0.5f, 0.5f), 40, 40, 130, 100, 1.5f)
    };

    public string name;

    public int maxCarbonProduction;

    public int waterRequirement;

    public int moistureAbsorbtionRate;

    public int matureTime;

    public float terrainSpeedModifier;

    public Color color;

    public PlantType(string name, Color color, int waterRequirement, int moistureAbsorbtionRate, int maxCarbonProduction, int matureTime, float terrainSpeedModifier)
    {
        this.name = name;
        this.waterRequirement = waterRequirement;
        this.moistureAbsorbtionRate = moistureAbsorbtionRate;
        this.maxCarbonProduction = maxCarbonProduction;
        this.matureTime = matureTime;
        this.terrainSpeedModifier = terrainSpeedModifier;
        this.color = color;
    }

    public void InitSRVisuals(SpriteRenderer sr, int age)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/trees");
        int index = Mathf.FloorToInt((sprites.Length - 1) * Mathf.Clamp01((float)age / matureTime));
        sr.sprite = sprites[index];
        sr.color = color;
    }

}
