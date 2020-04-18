using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
 
    public Transform home;
    public bool attackMode;
    public Vector3 direction;
    public float speed, attack, attackPeriod;
    public TreeController target;

    float timeSinceAttack;

    public float health;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceAttack = attackPeriod;
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackMode)
        {

            if (target != null)
            {

                if (Vector3.SqrMagnitude(target.transform.position - transform.position) > 0.1f)
                {
                    GoToTree();
                }
                else if (timeSinceAttack > attackPeriod)
                {
                    AttackTree();
                }

            }
            else
            {

                LookForTree();

                //print("looking for tree");


            }
        }
        else {
            GoHome();
        }
        timeSinceAttack += Time.deltaTime;
    }

    float TreeClosest(TreeController tree) {
        float closestLocal = Vector3.SqrMagnitude(tree.transform.position - transform.position) + Vector3.SqrMagnitude(tree.transform.position - home.position);
            return closestLocal;
    }

  

    void LookForTree() {
        List<TreeController> trees = GameController.instance.trees;
        int targetIndex = 0;
        float closestDist = TreeClosest(trees[targetIndex]);
        
        for (int i = 0; i < trees.Count; i++)
        {
            float d = TreeClosest(trees[i]);
            print(d);
            if (d < closestDist) {
                
                closestDist = d;
                targetIndex = i;
            }
        }
        target = trees[targetIndex];
        //print("found tree at " + target.transform.position);

    }
    void AttackTree() {
        
        target.Attacked(attack);
        timeSinceAttack = 0;
        if (target.health < 0) {
            
            target = null;
            
        }

    }

    void GoToTree() {
        direction = (target.transform.position - transform.position).normalized;
        transform.position += speed * direction * Time.deltaTime;
    }
    void GoHome()
    {
        direction = (home.position - transform.position).normalized;
        transform.position += speed * direction * Time.deltaTime;
    }

}
