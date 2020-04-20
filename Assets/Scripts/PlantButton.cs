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
        return !Input.GetKey(KeyCode.LeftControl);
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
}
