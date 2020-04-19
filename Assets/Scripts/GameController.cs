using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject treePrefab;

    public GameObject animalPrefab;

    public List<PlantController> trees = new List<PlantController>();

    const float TREE_UPDATE_PERIOD = 5f;

    float treeTimer = 0f;

    long carbon = 0;

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

        // Init the animal types.
        AnimalType.InitAnimalTypes();

        // Place some inital trees.
        for (int i = 0; i < 100; i++)
        {
            Vector3 c = Random.onUnitSphere;
            Vector2 coord = new Vector2(c.x, c.y);
            coord = coord.normalized * Mathf.Sqrt(Random.Range(0f, 1f)) * 10f;
            PlantController newPlant = CreatePlant(coord, PlantType.plantTypes[Random.Range(0, PlantType.plantTypes.Count)]);
            newPlant.SetAge(Random.Range(1, newPlant.plantType.matureTime));
        }

        var deer = AnimalType.animalTypes.Find(x => x.name == "Deer");
        var bear = AnimalType.animalTypes.Find(x => x.name == "Bear");


        // Place some initial animals.
        for (int i = 0; i < 20; i++)
        {
            Vector3 c = Random.onUnitSphere;
            Vector2 coord = new Vector2(c.x, c.y);
            coord = coord.normalized * Mathf.Sqrt(Random.Range(0f, 1f)) * 10f;
            GameObject animalGO = Instantiate(animalPrefab, coord, Quaternion.identity);
            animalGO.GetComponent<AnimalController>().Initialise(deer);
        }

        for (int i = 0; i < 2; i++)
        {
            Vector3 c = Random.onUnitSphere;
            Vector2 coord = new Vector2(c.x, c.y);
            coord = coord.normalized * Mathf.Sqrt(Random.Range(0f, 1f)) * 10f;
            GameObject animalGO = Instantiate(animalPrefab, coord, Quaternion.identity);
            animalGO.GetComponent<AnimalController>().Initialise(bear);
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

        while (treeTimer > TREE_UPDATE_PERIOD)
        {
            foreach (var tree in trees)
            {
                tree.Grow();
            }
            treeTimer -= TREE_UPDATE_PERIOD;
        }
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
