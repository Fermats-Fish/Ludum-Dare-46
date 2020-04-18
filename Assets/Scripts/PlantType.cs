using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantType
{

    public static List<PlantType> plantTypes = new List<PlantType>
    {
        new PlantType("Oak", 20, 20, 100, 20, 1f),
        new PlantType("Pine", 40, 40, 130, 10, 1.5f)
    };

    public string name;

    public int maxCarbonProduction;

    public int waterRequirement;

    public int moistureAbsorbtionRate;

    public int matureTime;

    public float terrainSpeedModifier;

    public PlantType(string name, int waterRequirement, int moistureAbsorbtionRate, int maxCarbonProduction, int matureTime, float terrainSpeedModifier)
    {
        this.name = name;
        this.waterRequirement = waterRequirement;
        this.moistureAbsorbtionRate = moistureAbsorbtionRate;
        this.maxCarbonProduction = maxCarbonProduction;
        this.matureTime = matureTime;
        this.terrainSpeedModifier = terrainSpeedModifier;
    }

    public void InitSRVisuals(SpriteRenderer sr)
    {
        sr.sprite = Resources.Load<Sprite>("Sprites/" + name);
    }

}
