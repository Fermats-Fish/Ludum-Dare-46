using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType
{
    public static List<AnimalType> animalTypes;

    public int health;
    public List<AnimalType> runsFrom = new List<AnimalType>();
    public List<AnimalType> eats = new List<AnimalType>();
    public List<PlantType> attractedToPlants = new List<PlantType>();
    public string name;
    public float moveSpeed;
    public float sightRange;
    public float attackRange;
    public int attackStrength;
    public float attackCooldown;

    public static void InitAnimalTypes()
    {
        var bear = new AnimalType("Bear", 300, 0.5f, 2f, 0.5f, 10, 1f);
        var deer = new AnimalType("Deer", 50, 0.7f, 1f, 0f, 0, 1f);

        animalTypes = new List<AnimalType> { bear, deer };

        bear.eats.Add(deer);
        bear.attractedToPlants.Add(PlantType.plantTypes.Find(x => x.name == "Fruit Tree"));

        deer.runsFrom.Add(bear);
    }

    public AnimalType(string name, int health, float moveSpeed, float sightRange, float attackRange, int attackStrength, float attackCooldown)
    {
        this.name = name;
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.sightRange = sightRange;
        this.attackRange = attackRange;
        this.attackStrength = attackStrength;
        this.attackCooldown = attackCooldown;
    }
}
