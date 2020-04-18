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
            coord = coord.normalized * Random.Range(0f, 10f);
            CreateTree(coord);
        }

        UIController.instance.UpdateCarbonText();

    }

    void CreateTree(Vector2 position)
    {
        // Instantiate the new tree.
        var newTreeGO = Instantiate(treePrefab, new Vector3(position.x, position.y, TreeController.TREE_Z), Quaternion.identity);
        var newTreeController = newTreeGO.GetComponent<TreeController>();

        // Add to the list of trees.
        trees.Add(newTreeController);
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
