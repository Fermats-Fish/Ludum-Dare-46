using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public int numberOfEnemies;
    public GameObject enemyPrefab;
    List<Enemy> enemies = new List<Enemy>();


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
