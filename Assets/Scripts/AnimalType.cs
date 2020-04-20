using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType
{
    public static List<AnimalType> animalTypes = new List<AnimalType>();

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
    public float runAfterHitDistance;
    
    public delegate float GetHabitabilityDelegate(List<PlantController> plants, List<AnimalController> animals);
    public GetHabitabilityDelegate GetHabitability = (x,y) => 0;

    public static void InitAnimalTypes()
    {
        //                             Name    Health  Speed  Sight  ARange AStrength ACooldown RunAfterHitDist
        var bear    = new AnimalType("Bear",    100,    0.5f,  2f,    0.5f,    10,      1f,            0f);
        var deer    = new AnimalType("Deer",     50,    0.7f,  1f,      0f,     0,      1f,            0f);
        var hunter  = new AnimalType("Hunter",   70,    0.6f, 60f,      1f,  1000,  20000f,        10000f);

        bear.eats.Add(deer);
        bear.eats.Add(hunter);
        bear.attractedToPlants.Add(PlantType.plantTypes.Find(x => x.name == "Fruit Tree"));
        bear.GetHabitability = (plants, animals) => {
            float fruitWeight = 0;
            float deerWeight = 0;
            var fruitTree = PlantType.plantTypes.Find(x => x.name == "Fruit Tree");
            foreach (var plant in plants)
            {
                if(plant.plantType == fruitTree){
                    fruitWeight += 1;
                }
            }
            foreach (var animal in animals){
                if (animal.animalType == deer){
                    deerWeight += 1;
                }
            }

            return fruitWeight * deerWeight * 0.04f;
        };

        deer.runsFrom.Add(bear);
        deer.GetHabitability = (plants, animals) => plants.Count * 0.15f;

        hunter.eats.Add(bear);
        hunter.eats.Add(deer);
    }

    public AnimalType(string name, int health, float moveSpeed, float sightRange, float attackRange, int attackStrength, float attackCooldown, float runAfterHitDistance)
    {
        this.name = name;
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.sightRange = sightRange;
        this.attackRange = attackRange;
        this.attackStrength = attackStrength;
        this.attackCooldown = attackCooldown;
        this.runAfterHitDistance = runAfterHitDistance;

        animalTypes.Add(this);
    }
}
