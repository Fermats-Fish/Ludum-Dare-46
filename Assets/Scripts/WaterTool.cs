using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTool : Tool
{

    const int WATER_COST = 1000;
    public GameObject cloud;

    private void Start()
    {
        cloud = Instantiate(GameController.instance.cloudPrefab);
    }
    public override bool UseTool()
    {
        
        if (GameController.instance.TrySubtractWater(WATER_COST))
        {
            // PLACE CODE FOR WATER HERE. THIS WILL FIRST WHEN THE USER LEFT CLICKS, NOT ON MOUSE HOLD.
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            cloud.GetComponent<SpriteRenderer>().color = Color.white;
             List <Fire> fires = GameController.instance.fires;
            cloud.transform.position = pos;
            foreach (Fire t in fires)
            {
                if (t != null)
                {
                   
                    pos.z = t.transform.position.z;
                    print(t);
                    if (Vector3.Distance(pos, t.transform.position) < 1f)
                    {
                        t.fireHealth -= 0.1f;
                        print(t + "water");

                    }
                }
            }
        }

        // Don't deselect tool till the user right clicks.
        return false;
    }

    private void Update()
    {
        cloud.GetComponent<SpriteRenderer>().color = cloud.GetComponent<SpriteRenderer>().color*0.99f;
    }

    public override void UpdateInteractable()
    {
        button.interactable = GameController.instance.GetWater() >= WATER_COST;
    }
}
