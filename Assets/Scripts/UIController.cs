using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public Text carbonText;

    void Start()
    {
        // Establish instance.
        if (instance != null)
        {
            Debug.LogError("There is already a GameController");
        }
        else
        {
            instance = this;
        }

    }

    void Update()
    {

    }

    public void UpdateCarbonText()
    {
        carbonText.text = "Carbon: " + (GameController.instance.carbon / 10000f).ToString("0.00");
    }
}
