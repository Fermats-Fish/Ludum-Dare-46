using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public int numberOfEnemies;
    public GameObject enemyPrefab;
    List<Enemy> enemies = new List<Enemy>();
    public float HOME_TIME =0.75f, ATTACK_TIME=0.25f;
    bool waveSent = false;
    int currentDay = 0;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Enemy newEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
            newEnemy.home = transform;
            newEnemy.transform.position = transform.position;
            enemies.Add(newEnemy);
            newEnemy.Init();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!waveSent)
        {
            if (GameController.instance.timeOfDay > ATTACK_TIME)
            {
                SendOutAllEnemies();
                waveSent = true;
            }
        }
        else
        {
            if (GameController.instance.timeOfDay > HOME_TIME)
            {
                RecallEnemies();
            }
        }
        if (currentDay < GameController.instance.daysSurvived) {
            currentDay = GameController.instance.daysSurvived;
            waveSent = false;
        }
    }
    public void SendOutAnEnemy()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].attackMode == false)
            {
                enemies[i].attackMode = true;
                return;
            }
        }
        
    }

    public void SendOutAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].attackMode = true;
        }
    }
    public void RecallEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].attackMode = false;
        }
    }
}
