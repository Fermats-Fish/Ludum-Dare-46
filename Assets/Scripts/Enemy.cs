﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Transform home;
    public bool attackMode;
    public Vector3 direction;
    public float speed, attack, attackPeriod;
    public PlantController target;

    protected float timeSinceAttack;
    Vector3 position;
    public float health;

    float bearSightRange = 3f;

    // Start is called before the first frame update
    public void Init()
    {
        timeSinceAttack = attackPeriod;
        health = 100;
        position = transform.position;
        attackMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check for nearby bear.
        var colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), bearSightRange);
        foreach (var collider in colliders)
        {
            AnimalController ac = collider.GetComponent<AnimalController>();
            if (ac != null && ac.animalType == AnimalType.animalTypes.Find(x => x.name == "Bear"))
            {
                // Run away from this animal.
                GoHome();
                LookForTree();
                timeSinceAttack += Time.deltaTime;
                return;
            }
        }

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
            }
        }
        else
        {
            GoHome();
            transform.up = Vector3.Normalize(home.position - transform.position);
        }
        timeSinceAttack += Time.deltaTime;

        // if (target != null)
        // {
        //     var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        //     transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // }
    }

    float TreeClosest(PlantController tree)
    {
        float closestLocal = Vector3.SqrMagnitude(tree.transform.position - transform.position) + Vector3.SqrMagnitude(tree.transform.position - home.position);
        return closestLocal;
    }

    protected virtual void LookForTree()
    {
        List<PlantController> trees = GameController.instance.trees;

        if (trees.Count == 0)
        {
            attackMode = false;
            return;
        }

        int targetIndex = 0;
        float closestDist = TreeClosest(trees[targetIndex]);

        for (int i = 0; i < trees.Count; i++)
        {
            if (!trees[i].beingChoppedDown && !trees[i].onFire)
            {
                float d = TreeClosest(trees[i]);
                if (d < closestDist)
                {

                    closestDist = d;
                    targetIndex = i;
                }
            }
        }
        target = trees[targetIndex];
        target.beingChoppedDown = true;

    }

    protected virtual void AttackTree()
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
        direction = (target.transform.position - transform.position);
       
        
        if (direction.sqrMagnitude > 0.1f)
        {
            faceMovement(direction);
        }

        transform.position += speed * direction.normalized * Time.deltaTime;
    }

    void GoHome()
    {
        // direction = (home.position - transform.position).normalized;

        // transform.position += speed * direction * Time.deltaTime;

        direction = (home.position - transform.position);
        
        if (direction.sqrMagnitude > 0.1f) {
            faceMovement(direction);
        }

        transform.position += speed * direction.normalized * Time.deltaTime;
    }

    void faceMovement(Vector3 d) {
        float bearing = Mathf.Atan2(d.y, d.x)*Mathf.Rad2Deg-90;
        Vector3 e = transform.eulerAngles;
        transform.eulerAngles = new Vector3(e.x, e.y, bearing);
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
