using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject treePrefab;

    public GameObject animalPrefab;

    public List<PlantController> trees = new List<PlantController>();

    const int GRID_SIZE = 100;

    const float TREE_UPDATE_PERIOD = 5f, DAY_LENGTH = 180f;

    float treeTimer = 0f, dayTimer = 0f;

    long carbon = 0;
    long water = 0;

    public float timeOfDay;
    public int daysSurvived;

    public GameObject terrainPrefab;

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

        // Generate terrain
        TileController tc = new TileController(GRID_SIZE);
        tc.InitGrid();
        foreach (List<Tile> gridX in tc.grid)
        {
            foreach (Tile t in gridX)
            {
                var terrainGameObject = Instantiate(terrainPrefab, t.location, Quaternion.identity);
                terrainGameObject.GetComponent<SpriteRenderer>().sprite = t.sprite;
                terrainGameObject.GetComponent<SpriteRenderer>().material.color = t.color;

                // Add marsh ponds if required
                if (t.isMarshy)
                {
                    foreach (MarshSubTile m in t.marsh)
                    {
                        var marshGameObject = Instantiate(terrainPrefab, m.location, Quaternion.Euler(new Vector3(0, 0, (int)(m.rotation * 360))));
                        var sr = marshGameObject.GetComponent<SpriteRenderer>();
                        sr.sprite = m.sprite;
                        sr.flipX = m.flipX;
                        sr.flipY = m.flipY;
                        sr.material.color = new Color(0.0f, 0.05f, 0.4f);
                    }
                }
            }
        }

        // Init the animal types.
        AnimalType.InitAnimalTypes();

        // Place some inital trees.
        for (int i = 0; i < 100; i++)
        {
            Vector3 c = Random.onUnitSphere;
            Vector2 coord = new Vector2(c.x, c.y);
            coord = coord.normalized * Mathf.Sqrt(Random.Range(0f, 1f)) * 10f;
            PlantController newPlant = CreatePlant(coord, PlantType.GetRandomPlantType());
            newPlant.SetAge(Random.Range(1, newPlant.plantType.matureTime));
        }

        var deer = AnimalType.animalTypes.Find(x => x.name == "Deer");
        var bear = AnimalType.animalTypes.Find(x => x.name == "Bear");


        // Place some initial animals.
        for (int i = 0; i < 10; i++)
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
        UIController.instance.OnWaterChanged();
       

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

        if (dayTimer > DAY_LENGTH)
        {
            dayTimer = 0;
            daysSurvived++;
        }

        timeOfDay = dayTimer / DAY_LENGTH;
        UIController.instance.OnDaysSurvived();
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

    public long GetWater()
    {
        return water;
    }

    public bool TrySubtractWater(long delta)
    {
        if (GetWater() < delta)
        {
            return false;
        }
        else
        {
            SetWater(water - delta);
            return true;
        }
    }

    void SetWater(long newWater)
    {
        water = newWater;

        // Tell the UIController to update which buttons are enabled.
        UIController.instance.OnWaterChanged();
    }

    public void AddToWater(long delta)
    {
        if (delta < 0)
        {
            Debug.LogError("Trying to add a negative amount of water. Use TrySubtractWater instead.");
        }

        SetWater(water + delta);
    }
}
