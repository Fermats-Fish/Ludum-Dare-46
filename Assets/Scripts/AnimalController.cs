using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public AnimalType animalType;

    SpriteRenderer spriteRenderer;

    Vector3 target;

    float timer;

    const float MAX_TIME_BETWEEN_TARGETS = 10f;

    public void Initialise(AnimalType animalType)
    {
        this.animalType = animalType;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/animals/" + animalType.name);
    }

    void Update()
    {
        // Pick new target?
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = Random.Range(0f, MAX_TIME_BETWEEN_TARGETS);
            PickNewTarget();
        }

        // Move towards target.
        Vector3 delta = (target - transform.position).normalized * animalType.moveSpeed * Time.deltaTime;
        delta.z = 0;
        transform.position += delta;
    }

    void PickNewTarget()
    {
        // First check we are near a tree.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), animalType.sightRange * 3);
        bool treeNearby = false;
        foreach (var collider in colliders)
        {
            var pc = collider.GetComponent<PlantController>();
            if (pc != null)
            {
                // There is a tree nearby, so we are fine.
                treeNearby = true;
                break;
            }
        }
        if (!treeNearby)
        {
            // Move towards a random tree.
            target = GameController.instance.trees[Random.Range(0, GameController.instance.trees.Count)].transform.position;
            return;
        }


        List<Vector3> possibleTargets = new List<Vector3>();
        // Check for anything to run away from...
        if (animalType.runsFrom.Count > 0)
        {
            colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), animalType.sightRange);
            foreach (var collider in colliders)
            {
                AnimalController ac = collider.GetComponent<AnimalController>();
                if (ac != null && animalType.runsFrom.Find(x => x == ac.animalType) != null)
                {
                    // Run away from this animal.
                    possibleTargets.Add(transform.position + (transform.position - ac.transform.position));
                }
            }
        }

        if (possibleTargets.Count > 0)
        {
            float closest = Mathf.Infinity;
            foreach (var possibleTarget in possibleTargets)
            {
                var sqrMag = (possibleTarget - transform.position).sqrMagnitude;
                if (sqrMag < closest)
                {
                    closest = sqrMag;
                    target = transform.position + (transform.position - possibleTarget).normalized * 100f;
                }
            }
            return;
        }

        // Okay, nothing to run from. Anything to run towards...
        if (animalType.eats.Count > 0)
        {
            colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), animalType.sightRange);
            foreach (var collider in colliders)
            {
                AnimalController ac = collider.GetComponent<AnimalController>();
                if (ac != null && animalType.eats.Find(x => x == ac.animalType) != null)
                {
                    // Run towards this animal.
                    possibleTargets.Add(ac.transform.position);
                }
            }
        }

        if (possibleTargets.Count > 0)
        {
            float closest = Mathf.Infinity;
            foreach (var possibleTarget in possibleTargets)
            {
                var sqrMag = (possibleTarget - transform.position).sqrMagnitude;
                if (sqrMag < closest)
                {
                    closest = sqrMag;
                    target = possibleTarget;

                    // Make the time before the next target check be at most the time it will take to reach the target's current position.
                    timer = Random.Range(0f, (target - transform.position).magnitude / animalType.moveSpeed);
                }
            }
            return;
        }

        // Nothing there either. Just move somewhere random.
        target = transform.position + Random.onUnitSphere * animalType.sightRange;
    }
}
