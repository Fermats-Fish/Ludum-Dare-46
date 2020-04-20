using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalType
{
    public static List<AnimalType> animalTypes = new List<AnimalType>();

    public int health;
    public List<AnimalType> runsFrom = new List<AnimalType>();
    public List<AnimalType> eats = new List<AnimalType>();
    public List<AnimalType> alwaysAttacks = new List<AnimalType>();
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

    public float maxHunger;
    public float foodValue;

    public static void InitAnimalTypes()
    {
        //                             Name    Health  Speed  Sight  ARange AStrength ACooldown RunAfterHitDist MaxHunger FoodValue
        var bear    = new AnimalType("Bear",    100,    0.5f,  2f,    0.5f,    10,      1f,            0f,        10f,       5f);
        var deer    = new AnimalType("Deer",     50,    0.7f,  1f,      0f,     0,      1f,            0f,         1f,       1f);
        var rabbit  = new AnimalType("Rabbit",   10,    1f,    0.75f,   0f,     0,      1f,            0f,         1f,       1f);
        var hunter  = new AnimalType("Hunter",   70,    0.6f, 60f,      1f,  1000,     20f,           10f,     10000f,       3f);
        var wolf    = new AnimalType("Wolf",     50,    1f,    5f,    0.7f,     5,     0.7f,           3f,        10f,       1f);

        bear.eats.Add(deer);
        bear.eats.Add(wolf);
        bear.alwaysAttacks.Add(hunter);
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

        wolf.eats.Add(bear);
        wolf.eats.Add(deer);
        wolf.alwaysAttacks.Add(hunter);
        wolf.GetHabitability = (plants, animals) => {
            return animals.FindAll(x => x.animalType == deer).Count * 0.07f;
        };

        deer.runsFrom.Add(bear);
        deer.runsFrom.Add(wolf);
        deer.GetHabitability = (plants, animals) => (1.5f * plants.FindAll(x => x.plantType.name == "Pine").Count + 0.5f * plants.FindAll(x => x.plantType.name == "Oak").Count) * 0.15f;

        rabbit.runsFrom.Add(bear);
        rabbit.runsFrom.Add(wolf);
        rabbit.GetHabitability = (plants, animals) => (1.5f * plants.FindAll(x => x.plantType.name == "Pine").Count + 0.5f * plants.FindAll(x => x.plantType.name == "Oak").Count) * 0.15f;

        hunter.alwaysAttacks.Add(bear);
        hunter.alwaysAttacks.Add(deer);
    }

    public AnimalType(string name, int health, float moveSpeed, float sightRange, float attackRange, int attackStrength, float attackCooldown, float runAfterHitDistance, float maxHunger, float foodValue)
    {
        this.name = name;
        this.health = health;
        this.moveSpeed = moveSpeed;
        this.sightRange = sightRange;
        this.attackRange = attackRange;
        this.attackStrength = attackStrength;
        this.attackCooldown = attackCooldown;
        this.runAfterHitDistance = runAfterHitDistance;
        this.maxHunger = maxHunger;
        this.foodValue = foodValue;

        animalTypes.Add(this);
    }
}
