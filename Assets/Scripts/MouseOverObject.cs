using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MouseOverObject : MonoBehaviour
{

    protected abstract string GetMouseOverText();

    GameObject mouseOverGO;
    Text mouseOverText;

    void OnMouseEnter()
    {
        if (mouseOverGO == null && GetMouseOverText() != null)
        {
            mouseOverGO = UIController.instance.CreateMouseOverObj();
            mouseOverText = mouseOverGO.GetComponentInChildren<Text>();
            UpdateMouseOverText();
        }
    }

    void OnMouseExit()
    {
        RemoveMouseOverGO();
    }

    // These have to be manually hooked up by adding an event trigget component to the ui element.
    public void UIOnMouseEnter()
    {
        OnMouseEnter();
    }
    public void UIOnMouseExit()
    {
        OnMouseExit();
    }

    protected void RemoveMouseOverGO()
    {
        if (mouseOverGO != null)
        {
            Destroy(mouseOverGO);
            mouseOverGO = null;
        }
    }

    protected void UpdateMouseOverText()
    {
        if (mouseOverGO != null)
        {
            mouseOverText.text = GetMouseOverText();
        }
    }

    void OnDestroy()
    {
        RemoveMouseOverGO();
    }
}
