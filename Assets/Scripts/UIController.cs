using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public GameObject plantTypeButtonPrefab;

    public GameObject waterToolButtonPrefab;

    public Text carbonText;

    public Text waterText;

    public Text daysSurvivedText;

    public Transform toolSelectPanel;

    public List<Tool> tools;

    public Tool selectedTool;

    // The "ghost" plant you see when going to place a new plant before you actually place it.
    GameObject plantGhost;

    public GameObject plantGhostPrefab;

    public Slider timeSlider;
    public Text timeScaleText;

    const float GHOST_Z = -2;


   
   

    public static bool muted = true;

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

        // Generate a button for each plant type.
        foreach (var plantType in PlantType.plantTypes)
        {
            var button = Instantiate(plantTypeButtonPrefab, toolSelectPanel).GetComponent<PlantButton>();
            button.Initialise(plantType);
        }

        // Add the water button.
        Instantiate(waterToolButtonPrefab, toolSelectPanel).GetComponent<WaterTool>().Initialise();

    }

    public void UpdateAudioOn(bool audioOn){
        if (audioOn){
            AudioListener.volume = 1f;
        } else {
            AudioListener.volume = 0f;
        }
    }

    public void SetupPlantGhost(PlantType plantType)
    {

        // Instantiate a ghost plant.
        plantGhost = Instantiate(plantGhostPrefab);
        plantType.InitSRVisuals(plantGhost.GetComponent<SpriteRenderer>(), 1);

        UpdateGhostPosition();
    }

    public void SwitchToTool(Tool newTool)
    {
        // Deselect selected button.
        DeselectTool();

        // Update selected tool.
        selectedTool = newTool;

        // Select new button.
        selectedTool.Select();
    }

    void UpdateGhostPosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = GHOST_Z;
        plantGhost.transform.position = pos;
    }

    void DeleteGhost()
    {
        Destroy(plantGhost);
        plantGhost = null;
    }

    void DeselectTool()
    {
        if (selectedTool != null)
        {
            selectedTool.Deselect();

            // If there is an old plant ghost delete it.
            if (plantGhost != null)
            {
                DeleteGhost();
            }

            selectedTool = null;
        }
    }

    void Update()
    {
        // If there is a tool selected...
        if (selectedTool != null)
        {
            // If there is a ghost visual...
            if (plantGhost != null)
            {
                // Update its position.
                UpdateGhostPosition();
            }

            // Click uses the tool if not hovering on ui.
            if (Input.GetMouseButtonDown(0) && !MouseOverUI())
            {
                bool deselect = selectedTool.UseTool();

                if (deselect)
                {
                    DeselectTool();
                }
            }

            // Right click or esc to cancel.
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectTool();
            }
        }
        Time.timeScale = (int)(timeSlider.value*29 + 1);
        timeScaleText.text = "Time x" + Time.timeScale;
    }

    public static bool MouseOverUI()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }

    public void OnCarbonChanged()
    {
        carbonText.text = "Carbon: " + (GameController.instance.GetCarbon() / 10000f).ToString("0.00");
        foreach (Tool tool in tools)
        {
            tool.UpdateInteractable();
        }
    }

    public void OnDaysSurvived()
    {
        daysSurvivedText.text = "Days: " + (GameController.instance.daysSurvived+GameController.instance.timeOfDay).ToString("0.00");
      
    }

    public void OnWaterChanged()
    {
        foreach (Tool tool in tools)
        {
            tool.UpdateInteractable();
        }
        waterText.text = "Water: " + (GameController.instance.GetWater() / 10000f).ToString("0.00");
    }
}
