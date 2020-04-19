using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arsonist : Enemy
{
    public GameObject fire;
    
    protected override void LookForTree()
    {
        List<PlantController> trees = GameController.instance.trees;
        target = trees[(int)(trees.Count * Random.value)];

    }
    protected override void AttackTree()
    {
        print("FIRE");
        Instantiate(fire, target.transform).GetComponent<Fire>().Spread(target);
        timeSinceAttack = 0;
        attackMode = false;
        target = null;

    }

}
