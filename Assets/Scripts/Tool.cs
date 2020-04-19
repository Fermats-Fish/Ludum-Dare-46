using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tool : MonoBehaviour
{
    PlantType plantType;
    Image image;
    Text text;

    static readonly Color DESELECTED_COLOR = new Color(1f, 1f, 1f, 43f / 255f);
    static readonly Color SELECTED_COLOR = new Color(1f, 1f, 1f, 100f / 255f);

    public Button button;

    public virtual void Initialise()
    {
        UIController.instance.tools.Add(this);
        text = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
        this.button = GetComponent<Button>();
    }

    public void Press()
    {
        UIController.instance.SwitchToTool(this);
    }

    public void Deselect()
    {
        image.color = DESELECTED_COLOR;
    }

    public virtual void Select()
    {
        image.color = SELECTED_COLOR;
    }

    public virtual bool UseTool()
    {
        return true;
    }

    public virtual void UpdateInteractable()
    {

    }
}
