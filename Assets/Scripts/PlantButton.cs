using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantButton : MonoBehaviour
{
    PlantType plantType;
    Image image;

    static readonly Color DESELECTED_COLOR = new Color(1f, 1f, 1f, 43f / 255f);
    static readonly Color SELECTED_COLOR = new Color(1f, 1f, 1f, 100f / 255f);

    public void Initialise(PlantType plantType)
    {
        UIController.instance.plantButtons.Add(plantType, this);
        this.plantType = plantType;
        GetComponentInChildren<Text>().text = "Plant " + plantType.name;
        image = GetComponent<Image>();
    }

    public void Press()
    {
        UIController.instance.SwitchToPlantType(plantType);
    }

    public void Deselect()
    {
        image.color = DESELECTED_COLOR;
    }

    public void Select()
    {
        image.color = SELECTED_COLOR;
    }
}
