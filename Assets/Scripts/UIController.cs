using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public GameObject plantTypeButtonPrefab;

    public Text carbonText;

    public Transform plantSelectPanel;

    public Dictionary<PlantType, PlantButton> plantButtons = new Dictionary<PlantType, PlantButton>();

    public PlantType selectedPlantType;

    // The "ghost" plant you see when going to place a new plant before you actually place it.
    GameObject plantGhost;

    public GameObject plantGhostPrefab;

    const float GHOST_Z = -2;

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
            var button = Instantiate(plantTypeButtonPrefab, plantSelectPanel).GetComponent<PlantButton>();
            button.Initialise(plantType);
        }

    }

    public void SwitchToPlantType(PlantType plantType)
    {
        // Deselect selected button.
        DeselctPlantType();

        // Update selected plant type.
        selectedPlantType = plantType;

        // Instantiate a ghost plant.
        plantGhost = Instantiate(plantGhostPrefab);
        plantType.InitSRVisuals(plantGhost.GetComponent<SpriteRenderer>(), 1);

        UpdateGhostPosition();

        // Select new button.
        PlantButton button;
        plantButtons.TryGetValue(plantType, out button);
        button.Select();
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

    void DeselctPlantType()
    {
        if (selectedPlantType != null)
        {
            PlantButton button;
            plantButtons.TryGetValue(selectedPlantType, out button);
            button.Deselect();

            // If there is an old plant ghost delete it.
            if (plantGhost != null)
            {
                DeleteGhost();
            }

            selectedPlantType = null;
        }
    }

    void Update()
    {
        // If there is a plant type selected...
        if (selectedPlantType != null)
        {
            // If there is a ghost visual...
            if (plantGhost != null)
            {
                // Update its position.
                UpdateGhostPosition();
            }

            // Click to place plant.
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                GameController.instance.CreatePlant(new Vector2(pos.x, pos.y), selectedPlantType);

                // If not holding control, deselct the plant type...
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    DeselctPlantType();
                }
            }

            // Right click or esc to cancel.
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                DeselctPlantType();
            }
        }
    }

    public void UpdateCarbonText()
    {
        carbonText.text = "Carbon: " + (GameController.instance.carbon / 10000f).ToString("0.00");
    }
}
