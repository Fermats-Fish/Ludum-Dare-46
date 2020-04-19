using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject treePrefab;

    public List<PlantController> trees = new List<PlantController>();

    const float TREE_UPDATE_PERIOD = 5f;

    const int GRID_SIZE = 200;

    float treeTimer = 0f;

    public long carbon = 0;

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
                        var marshGameObject = Instantiate(terrainPrefab, m.location, Quaternion.Euler(new Vector3(0, 0, (int)(m.rotation*360))));
                        var sr = marshGameObject.GetComponent<SpriteRenderer>();
                        sr.sprite = m.sprite;
                        sr.flipX = m.flipX;
                        sr.flipY = m.flipY;
                        sr.material.color = new Color(0.0f, 0.05f, 0.4f);
                    }
                }
            }
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
        var newTreeController = newTreeGO.GetComponent<PlantController>();

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
