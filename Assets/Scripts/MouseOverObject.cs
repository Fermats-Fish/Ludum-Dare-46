using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MouseOverObject : MonoBehaviour
{

    protected abstract string GetMouseOverText();

    GameObject mouseOverGO;
    Text text;

    public void OnMouseEnter()
    {
        if (mouseOverGO == null && GetMouseOverText() != null)
        {
            mouseOverGO = UIController.instance.CreateMouseOverObj();
            text = mouseOverGO.GetComponentInChildren<Text>();
            UpdateMouseOverText();
        }
    }

    public void OnMouseExit()
    {
        RemoveMouseOverGO();
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
            text.text = GetMouseOverText();
        }
    }
}
