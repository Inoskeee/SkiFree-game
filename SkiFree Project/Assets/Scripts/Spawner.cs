using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public List<GameObject> Enemies;
    public List<GameObject> Prefabs;

    private GameObject player;

    public float maxCubes;
    public float maxEnemies;
    public float radius;


    private float numCubes = 0;
    private float numEmemies = 0;
    private Vector2 center;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        center = gameObject.transform.position;
        gameObjects = new List<GameObject>();

        while (maxCubes > gameObjects.Count)
        {
            Vector3 pos = center + new Vector2(Random.Range(-radius / 2, radius / 2), Random.Range(-radius / 2, radius / 2));
            bool check = pos.x < player.transform.position.x + 30 && pos.x > player.transform.position.x - 30 && pos.y < player.transform.position.y + 25 && pos.y > player.transform.position.y - 25;
            if (!check)
            {
                numCubes++;
                GameObject gObj = Instantiate(Prefabs[Random.Range(0, Prefabs.Count - 1)], pos, Quaternion.identity);
                gObj.name = "Cube " + numCubes;
                gObj.transform.SetParent(gameObject.transform);
                gameObjects.Add(gObj);
            }
        }

        if(gameObject.transform.position.y < -200)
        {
            while (maxEnemies > Enemies.Count)
            {
                Vector3 pos = center + new Vector2(Random.Range(-radius / 2, radius / 2), Random.Range(-radius / 2, radius / 2));
                bool check = pos.x < player.transform.position.x + 30 && pos.x > player.transform.position.x - 30 && pos.y < player.transform.position.y + 25 && pos.y > player.transform.position.y - 25;
                if (!check)
                {
                    numEmemies++;
                    GameObject gObj = Instantiate(Prefabs[3], pos, Quaternion.identity);
                    gObj.name = "Cube " + numEmemies;
                    gObj.transform.SetParent(gameObject.transform);
                    Enemies.Add(gObj);
                }
            }
        }
    }

    void Update()
    {

        
    }


}
