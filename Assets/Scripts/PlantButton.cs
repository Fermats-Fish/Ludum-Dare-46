using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantButton : Tool
{
    PlantType plantType;

    public AudioClip placeTreeAudioClip;

    public void Initialise(PlantType plantType)
    {
        base.Initialise();
        this.plantType = plantType;
        GetComponentInChildren<Text>().text = "Plant " + plantType.name;
    }

    public override bool UseTool()
    {
        if (GameController.instance.TrySubtractCarbon(plantType.carbonBuildCost))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameController.instance.CreatePlant(new Vector2(pos.x, pos.y), plantType);
            AudioSource.PlayClipAtPoint(placeTreeAudioClip, pos);
        }

        // If not holding control, deselct the plant type...
        return !(Input.GetKey(KeyCode.LeftControl) | Input.GetKey(KeyCode.LeftShift));
    }

    public override void UpdateInteractable()
    {
        button.interactable = GameController.instance.GetCarbon() >= plantType.carbonBuildCost;
    }

    public override void Select()
    {
        base.Select();
        UIController.instance.SetupPlantGhost(plantType);
    }

    protected override string GetMouseOverText()
    {
        return $"{plantType.name}:"
            + $"\nWater Requirement (Not Implimented): {plantType.waterRequirement}"
            + $"\nSurpless Water Production: {plantType.surplessWaterProd}"
            + $"\nCarbon Production: {plantType.maxCarbonProduction}"
            + $"\nMature Time: {plantType.matureTime}"
            + $"\nTerrain Speed Modified (Not Implimented): {plantType.terrainSpeedModifier}"
            + $"\nCarbon Build Cost: {plantType.carbonBuildCost}"
            + $"\nHealth: {plantType.health}"
            + $"\nInitial Spawn Weight: {plantType.initSpawnWeight}"
            + $"\nFlamability: {plantType.flamability}"
        ;
    }
}
