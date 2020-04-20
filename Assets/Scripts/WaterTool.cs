using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTool : Tool
{

    const int WATER_COST = 1000;

    public override bool UseTool()
    {
        if (GameController.instance.TrySubtractWater(WATER_COST))
        {
            // PLACE CODE FOR WATER HERE. THIS WILL FIRST WHEN THE USER LEFT CLICKS, NOT ON MOUSE HOLD.
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            List<PlantController> trees = GameController.instance.trees;

            foreach (PlantController t in trees)
            {
                if (t.onFire)
                {
                    print(Vector3.Distance(pos, t.transform.position).ToString());
                    if (Vector3.Distance(pos, t.transform.position) < 0.5f)
                    {
                        t.onFire = false;
                    }
                }
            }
        }

        // Don't deselect tool till the user right clicks.
        return false;
    }

    public override void UpdateInteractable()
    {
        button.interactable = GameController.instance.GetWater() >= WATER_COST;
    }
}
