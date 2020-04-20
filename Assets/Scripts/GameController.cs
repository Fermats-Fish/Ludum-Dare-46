using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject treePrefab;
    public GameObject cloudPrefab;
    public GameObject animalPrefab;

    public List<PlantController> trees = new List<PlantController>();
    public List<AnimalController> animals = new List<AnimalController>();

    public List<Fire> fires = new List<Fire>();

    const int GRID_SIZE = 100;

    const float TREE_UPDATE_PERIOD = 5f, DAY_LENGTH = 180f;

    float treeTimer = 0f, dayTimer = 0f;

    int daysTillNextHunter = 0;
    const int MIN_DAYS_TILL_NEXT_HUNTER = 2;
    const int MAX_DAYS_TILL_NEXT_HUNTER = 2;

    long carbon = 0;
    long water = 0;

    public float timeOfDay;
    public int daysSurvived;

    public GameObject terrainPrefab;

    AnimalController hunter;

    float animalAdjustmentTimer = 0f;
    float MIN_TIME_FOR_ANIMAL_ADJUST = 15f;
    float MAX_TIME_FOR_ANIMAL_ADJUST = 60f;

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

        CalculateDaysTillNextHunter();

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
        for (int i = 0; i < deer.GetHabitability(trees, animals); i++)
        {
            SpawnAnimal(deer);
        }

        for (int i = 0; i < bear.GetHabitability(trees, animals); i++)
        {
            SpawnAnimal(bear);
        }
        Debug.Log(deer.GetHabitability(trees, animals));
        Debug.Log(bear.GetHabitability(trees, animals));


        UIController.instance.OnCarbonChanged();
        UIController.instance.OnWaterChanged();


    }

    AnimalController SpawnAnimal(AnimalType animalType)
    {
        return SpawnAnimal(animalType, 0f, 10f);
    }

    AnimalController SpawnAnimal(AnimalType animalType, float minDistance, float maxDistance)
    {
        Vector3 c = Random.onUnitSphere;
        Vector2 coord = new Vector2(c.x, c.y);
        coord = coord.normalized * (minDistance + Mathf.Sqrt(Random.Range(0f, 1f)) * (maxDistance - minDistance));
        GameObject animalGO = Instantiate(animalPrefab, coord, Quaternion.identity);
        var ac = animalGO.GetComponent<AnimalController>();
        ac.Initialise(animalType);
        return ac;
    }

    void CalculateDaysTillNextHunter()
    {
        daysTillNextHunter = Random.Range(MIN_DAYS_TILL_NEXT_HUNTER, MAX_DAYS_TILL_NEXT_HUNTER + 1);
    }

    void ResetAnimalAdjustTimer()
    {
        animalAdjustmentTimer = Random.Range(MIN_TIME_FOR_ANIMAL_ADJUST, MAX_TIME_FOR_ANIMAL_ADJUST);
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
            if (daysTillNextHunter == 0)
            {
                // Remove last hunter.
                if (hunter != null)
                {
                    hunter.TakeDamage(100 * hunter.GetHealth());
                }
                CalculateDaysTillNextHunter();
            }

            dayTimer = 0;
            daysSurvived++;

            daysTillNextHunter -= 1;
            if (daysTillNextHunter == 0)
            {
                // Add hunter.
                hunter = SpawnAnimal(AnimalType.animalTypes.Find(x => x.name == "Hunter"), 20f, 30f);
                Debug.Log("A hunter arrives!");
            }
        }

        animalAdjustmentTimer -= Time.deltaTime;
        if( animalAdjustmentTimer < 0f ){
            ResetAnimalAdjustTimer();
            foreach (var animalType in AnimalType.animalTypes){
                var animalsOfThisType = animals.FindAll(x => x.animalType == animalType);
                float diff = animalType.GetHabitability(trees, animals) - animalsOfThisType.Count;
                if (diff > 0 && Random.Range(0f, 3f) < diff){
                    SpawnAnimal(animalType, 20f, 30f);
                }
                else if (diff < 0 && Random.Range(0f, 3f) < -diff && animalsOfThisType.Count > 0){
                    AnimalController a = animalsOfThisType[Random.Range(0, animalsOfThisType.Count)];
                    a.TakeDamage(a.GetHealth() * 100);
                }
            }
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
