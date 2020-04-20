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

    int health;

    AnimalController targetAnimal;

    float attackTimer = 0f;

    public void Initialise(AnimalType animalType)
    {
        this.animalType = animalType;
        health = animalType.health;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/animals/" + animalType.name);
    }

    void Update()
    {
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
        timer -= Time.deltaTime;

        spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, Time.deltaTime * 5f);

        // Pick new target?
        if (timer <= 0f)
        {
            timer = Random.Range(0f, MAX_TIME_BETWEEN_TARGETS);
            PickNewTarget();
        }

        if (targetAnimal != null && attackTimer <= 0 && (targetAnimal.transform.position - transform.position).sqrMagnitude < animalType.attackRange * animalType.attackRange)
        {
            // Can attack.
            attackTimer = animalType.attackCooldown;
            targetAnimal.TakeDamage(animalType.attackStrength);
        }

        // Move towards target.
        // Vector3 delta = (target - transform.position).normalized * animalType.moveSpeed * Time.deltaTime;
        // delta.z = 0;
        // transform.position += delta;
        
        // var direction = target - transform.position;
        // var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (target != null)
        {
            Vector3 delta = (target - transform.position).normalized * animalType.moveSpeed * Time.deltaTime;
            delta.z = 0;
            transform.position += delta;

            Vector3 direction = (target - transform.position);
            if (direction.sqrMagnitude > 0.1f)
            {
                faceMovement(direction.normalized);
            }
        }
    }

    void faceMovement(Vector3 d)
    {
        float bearing = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg-90;
        Vector3 e = transform.eulerAngles;
        transform.eulerAngles = new Vector3(e.x, e.y, bearing);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        spriteRenderer.color = Color.red;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
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
        List<AnimalController> possibleAnimals = new List<AnimalController>();
        if (animalType.eats.Count > 0)
        {
            colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), animalType.sightRange);
            foreach (var collider in colliders)
            {
                AnimalController ac = collider.GetComponent<AnimalController>();
                if (ac != null && animalType.eats.Find(x => x == ac.animalType) != null)
                {
                    // Run towards this animal.
                    possibleAnimals.Add(ac);
                }
            }
        }

        if (possibleAnimals.Count > 0)
        {
            float closest = Mathf.Infinity;
            foreach (var animal in possibleAnimals)
            {
                var possibleTarget = animal.transform.position;
                var sqrMag = (possibleTarget - transform.position).sqrMagnitude;
                if (sqrMag < closest)
                {
                    closest = sqrMag;
                    target = possibleTarget;
                    targetAnimal = animal;

                    // Make the time before the next target check be at most the time it will take to reach the target's current position.
                    timer = Random.Range(0f, (target - transform.position).magnitude / animalType.moveSpeed);
                }
            }
            return;
        }

        // Move towards a plant?
        if (animalType.attractedToPlants.Count > 0)
        {
            // Whether we move to a plant.
            bool moveToPlant = true;

            // If we aren't in eye sight of one of our favourite plants, move to the closest one.
            colliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), animalType.sightRange);
            foreach (var collider in colliders)
            {
                PlantController pc = collider.GetComponent<PlantController>();
                if (pc != null && animalType.attractedToPlants.Find(x => x == pc.plantType) != null)
                {
                    // We are close to one.
                    moveToPlant = false;
                    break;
                }
            }

            if (moveToPlant)
            {
                float closest = Mathf.Infinity;
                possibleTargets = new List<Vector3>();
                foreach (var plant in GameController.instance.trees)
                {
                    if (animalType.attractedToPlants.Find(x => x == plant.plantType) != null)
                    {
                        possibleTargets.Add(plant.transform.position);
                    }
                }
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
        }

        // Just move somewhere random.
        target = transform.position + Random.onUnitSphere * animalType.sightRange;
    }
}
