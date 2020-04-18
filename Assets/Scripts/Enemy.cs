using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform home;
    public bool attackMode;
    public Vector3 direction;
    public float speed, attack, attackPeriod;
    public PlantController target;

    float timeSinceAttack;
    Vector3 position;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceAttack = attackPeriod;
        health = 100;
        position = transform.position;
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
        else
        {
            GoHome();
        }
        timeSinceAttack += Time.deltaTime;
    }

    float TreeClosest(PlantController tree)
    {
        float closestLocal = Vector3.SqrMagnitude(tree.transform.position - transform.position) + Vector3.SqrMagnitude(tree.transform.position - home.position);
        return closestLocal;
    }



    void LookForTree()
    {
        List<PlantController> trees = GameController.instance.trees;
        int targetIndex = 0;
        float closestDist = TreeClosest(trees[targetIndex]);

        for (int i = 0; i < trees.Count; i++)
        {
            float d = TreeClosest(trees[i]);
            if (d < closestDist)
            {

                closestDist = d;
                targetIndex = i;
            }
        }
        target = trees[targetIndex];
        //print("found tree at " + target.transform.position);

    }
    void AttackTree()
    {

        target.Attacked(attack);
        timeSinceAttack = 0;
        if (target.health < 0)
        {

            target = null;

        }

    }

    void GoToTree()
    {
        direction = (target.transform.position - transform.position).normalized;
        transform.position += speed * direction * Time.deltaTime;
    }
    void GoHome()
    {
        direction = (home.position - transform.position).normalized;
        transform.position += speed * direction * Time.deltaTime;
    }


    public void Attacked(float attack)
    {
        health -= attack;
        StartCoroutine("Shake");
        if (health < 0)
        {
            OnDeath();
        }
    }

    IEnumerator Shake()
    {
        for (float t = 1; t >= 0; t -= 0.1f)
        {
            transform.position = position + Mathf.Cos(10 * t) / 10 * Vector3.right;
            yield return null;
        }
    }

    public void OnDeath()
    {
        attackMode = false;
        transform.position = home.position;
        health = 100;
    }
}
