using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType
{
    public static List<AnimalType> animalTypes;

    public int health;
    public List<AnimalType> runsFrom = new List<AnimalType>();
    public List<AnimalType> eats = new List<AnimalType>();
    public string name;
    public float moveSpeed;
    public float sightRange;

    public static void InitAnimalTypes()
    {
        var bear = new AnimalType("Bear", 300, 0.5f, 6f);
        var deer = new AnimalType("Deer", 50, 0.7f, 1f);

        animalTypes = new List<AnimalType> { bear, deer };

        bear.eats.Add(deer);

        deer.runsFrom.Add(bear);
    }

    public AnimalType(string name, int health, float moveSpeed, float sightRange)
    {
        this.name = name;
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.sightRange = sightRange;
    }
}
