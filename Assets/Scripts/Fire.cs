using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    Color color,treeColor;
    public PlantController tree;
    public float fireHealth = 1;
    bool spread;

    public float fireSize;
    // Start is called before the first frame update
    void Start()
    {
        fireSize = 0.1f;
        color = spriteRenderer.color;

        GameController.instance.fires.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (fireHealth < 0)
        {
            print("Fire put out");
            tree.onFire = false;
            GameController.instance.fires.Remove(this);
            Destroy(gameObject, 0.1f);
        }

        if (spread && fireSize <0.01f)
        {
            print("Fire old age");
            GameController.instance.fires.Remove(this);
            Destroy(gameObject, 0.1f);
        }

        if (tree != null)
        {

                transform.localScale = Vector3.one * tree.spriteRenderer.sprite.bounds.size.x;
                if (tree.health > 0)
                {

                    spriteRenderer.color = color * (0.9f + Mathf.Cos(Time.time * 10) / 10) * fireSize * fireHealth;
                    //stransform.localScale = Vector3.one * fireSize;
                    fireSize = (1f - Mathf.Pow(tree.health / tree.maxHealth * 2 - 1, 2)) * tree.spriteRenderer.sprite.bounds.size.x;
                    tree.health -= tree.plantType.flamability * Time.deltaTime;

                    float h = Mathf.Max(tree.health / tree.maxHealth, 0.3f);
                    tree.spriteRenderer.color = new Color(h * treeColor.r, h * treeColor.g, h * treeColor.b, 1);
                    if (!spread)
                    {
                        if (tree.health < 80 && !spread)
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
        Fire f = Instantiate(gameObject).GetComponent<Fire>();
        f.tree = plant;
        f.fireSize = 0;
        f.transform.position = plant.transform.position;
        plant.onFire = true;
        f.treeColor = plant.spriteRenderer.color;
        
    }
}
