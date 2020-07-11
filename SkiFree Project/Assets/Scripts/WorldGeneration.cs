using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{

    private Spawner radius;
    private MovingPlayer mov;
    private bool getObj;

    private int count = 1;

    [Header("RayCast features")]
    public Transform startPoint;
    public float castLenght;

    [Header("Spawn features")]
    public List<GameObject> gameWorld;
    public GameObject spawner;
    public GameObject container;
    public int worldSize;

    void Start()
    {
        radius = spawner.GetComponent<Spawner>();
        mov = gameObject.GetComponent<MovingPlayer>();
        gameWorld = new List<GameObject>();

        SpawnObject(gameObject.transform.position);
    }

    void Update()
    {
        getObj = WorldExist();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Respawn" && getObj)
        {
            if(mov.mover.magnitude != 0)
            {
                Vector2 whereToSpawn = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + mov.mover * (radius.radius / 2); //+ 10

                SpawnObject(whereToSpawn);

                if(gameWorld.Count >= worldSize)
                {
                    GameObject.Destroy(gameWorld[0]);
                    gameWorld.RemoveAt(0);
                }
            }
        }
    }

    private bool WorldExist()
    {
        Vector2 endPos = startPoint.position + new Vector3(mov.mover.x, mov.mover.y) * castLenght;
        RaycastHit2D rayHit = Physics2D.Linecast(startPoint.position, endPos, 1 << LayerMask.NameToLayer("Spawner"));
        if (rayHit.collider != null)
        {
            Debug.Log("Selected object: " + rayHit.transform.name);
            Debug.DrawLine(startPoint.position, endPos, Color.blue);
            getObj = false;
        }
        else
        {
            getObj = true;
        }

        return getObj;
    }

    private void SpawnObject(Vector2 whereToSpawn)
    {
        GameObject gObj = Instantiate(spawner, whereToSpawn, Quaternion.identity);

        gObj.name = spawner.name + count;
        gObj.transform.SetParent(container.transform);
        gameWorld.Add(gObj);
        count++;
    }
}
