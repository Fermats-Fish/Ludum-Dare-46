using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject treePrefab;

    public List<TreeController> trees = new List<TreeController>();

    const float TREE_UPDATE_PERIOD = 5f;

    float treeTimer = 0f;

    public long carbon = 0;

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
            CreateTree(coord, PlantType.plantTypes[Random.Range(0, PlantType.plantTypes.Count)]);
        }


        UIController.instance.UpdateCarbonText();

    }

    public void CreateTree(Vector2 position, PlantType selectedPlantType)
    {
        // Instantiate the new tree.
        var newTreeGO = Instantiate(treePrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
        var newTreeController = newTreeGO.GetComponent<TreeController>();

        // Init the tree.
        newTreeController.Initialise(selectedPlantType);
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

            UIController.instance.UpdateCarbonText();
        }
    }
}
