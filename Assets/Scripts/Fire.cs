using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    Color color,treeColor;
    public PlantController tree;

    bool spread;

    float fireSize;
    // Start is called before the first frame update
    void Start()
    {
        fireSize = 0;
        color = spriteRenderer.color;
        transform.localPosition = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if (tree != null) {
            if (tree.health > 0)
            {
                spriteRenderer.color = color * (0.9f + Mathf.Cos(Time.time * 10) / 10);
                transform.localScale = Vector3.one * fireSize;
                fireSize = (1f - Mathf.Pow(tree.health / tree.maxHealth * 2 - 1, 2)) * tree.spriteRenderer.sprite.bounds.size.x;
                tree.health -= tree.flammability * Time.deltaTime;

                float h = Mathf.Max(tree.health / tree.maxHealth, 0.3f);
                tree.spriteRenderer.color = new Color(h*treeColor.r, h * treeColor.g, h * treeColor.b, 1);
                if (!spread)
                {
                    if (tree.health > 40 && tree.health < 60)
                    {
                        List<PlantController> trees = GameController.instance.trees;


                        for (int i = 0; i < trees.Count; i++)
                        {
                            if (!trees[i].onFire)
                            {
                                if (Vector3.Distance(transform.position, trees[i].transform.position) < fireSize * 2)
                                {
                                    Spread(trees[i]);
                                }
                            }
                        }

                        spread = true;
                    }
                }
            }
        }
    }

    public void Spread(PlantController plant) {
        Fire f = Instantiate(gameObject, plant.transform).GetComponent<Fire>();
        f.tree = plant;
        f.fireSize = 0;
        plant.onFire = true;
        f.treeColor = plant.spriteRenderer.color; 
    }
}
