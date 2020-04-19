using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject treePrefab;

    public List<PlantController> trees = new List<PlantController>();
   

    const float TREE_UPDATE_PERIOD = 5f, DAY_LENGTH = 60f;


    float treeTimer = 0f, dayTimer = 0f;

    long carbon = 0;

    public float timeOfDay;
    public int daysSurvived;

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

        // Place some inital trees.
        for (int i = 0; i < 100; i++)
        {
            Vector3 x = Random.onUnitSphere;
            Vector2 coord = new Vector2(x.x, x.y);
            coord = coord.normalized * Mathf.Sqrt(Random.Range(0f, 1f)) * 10f;
            PlantController newPlant = CreatePlant(coord, PlantType.plantTypes[Random.Range(0, PlantType.plantTypes.Count)]);
            newPlant.SetAge(Random.Range(1, newPlant.plantType.matureTime));
        }


        UIController.instance.OnCarbonChanged();

    }

    public PlantController CreatePlant(Vector2 position, PlantType selectedPlantType)
    {
        // Instantiate the new tree.
        var newPlantGO = Instantiate(treePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        var newPlantController = newPlantGO.GetComponent<PlantController>();

        // Init the tree.
        newPlantController.Initialise(selectedPlantType);

        return newPlantController;
    }

    void Update()
    {
        treeTimer += Time.deltaTime;
        dayTimer += Time.deltaTime;

        while (treeTimer > TREE_UPDATE_PERIOD)
        {
            foreach (var tree in trees)
            {
                tree.Grow();
            }
            treeTimer -= TREE_UPDATE_PERIOD;
        }

        if (dayTimer > DAY_LENGTH) {
            dayTimer = 0;
            daysSurvived++;
        }

        timeOfDay = dayTimer / DAY_LENGTH;
    }

    public long GetCarbon()
    {
        return carbon;
    }

    public bool TrySubtractCarbon(long delta)
    {
        if (GetCarbon() < delta)
        {
            return false;
        }
        else
        {
            SetCarbon(carbon - delta);
            return true;
        }
    }

    public void AddToCarbon(long delta)
    {
        if (delta < 0)
        {
            Debug.LogError("Trying to add a negative amount of carbon. Use TrySubtractCarbon instead.");
        }

        SetCarbon(carbon + delta);
    }

    void SetCarbon(long newCarbon)
    {
        carbon = newCarbon;

        // Tell the UIController to update which buttons are enabled.
        UIController.instance.OnCarbonChanged();
    }
}
