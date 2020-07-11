using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Enemy controls")]
    public float speed;
    public float visionDistance;
    public float VerticalVisionDistance;

    [Header("RayCast features")]
    public Transform startPoint;
    public Transform endPoint;
    public float castLenght;

    private Animator anim;
    private GameObject player;
    private Rigidbody2D rigid;

    private float tempDistance;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        tempDistance = VerticalVisionDistance;
    }

    void FixedUpdate()
    {
        if (player != null)
        {

            if (Vector2.Distance(gameObject.transform.position, player.transform.position) <= visionDistance)
            {
                float posX = transform.position.x;
                float posY = transform.position.y;
                if(player.transform.position.y - transform.position.y <= VerticalVisionDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    endPoint.position = Vector2.MoveTowards(transform.position, player.transform.position, castLenght);
                    tempDistance = VerticalVisionDistance;
                    VerticalVisionDistance = visionDistance;
                }
                Animate(posX, posY);
            }
            else if(Vector2.Distance(gameObject.transform.position, player.transform.position) > visionDistance)
            {
                rigid.velocity = new Vector2(0, 0);
                endPoint.position = transform.position;
                anim.SetBool("isRun", false);
                anim.SetBool("isWalk", false);
                VerticalVisionDistance = tempDistance;
            }
        }
        else
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isWalk", false);
        }
    }
    void Update()
    {
        if (player != null)
        {
            RaycastHit2D rayHit = Physics2D.Linecast(startPoint.position, endPoint.position, 1 << LayerMask.NameToLayer("Obstruction"));
            Debug.DrawLine(startPoint.position, endPoint.position, Color.blue);
            if (rayHit.collider != null)
            {
                if (player.transform.position.x >= transform.position.y)
                {
                    if (player.transform.position.y >= transform.position.y)
                    {
                        transform.position += new Vector3(-0.1f, -0.1f, transform.position.z);
                    }
                    else
                    {
                        transform.position += new Vector3(-0.1f, 0.1f, transform.position.z);
                    }
                }
                else
                {
                    if (player.transform.position.y >= transform.position.y)
                    {
                        transform.position += new Vector3(0.1f, -0.1f, transform.position.z);
                    }
                    else
                    {
                        transform.position += new Vector3(0.1f, 0.1f, transform.position.z);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(player);
        }
    }

    private void Animate(float posX, float posY)
    {
        if(transform.position.y < posY)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", true);
        }
        if(transform.position.y >= posY && transform.position.x > posX)
        {
            gameObject.transform.localScale = new Vector3(-6f, 6f, 6f);
            anim.SetBool("isRun", false);
            anim.SetBool("isWalk", true);
        }
        if (transform.position.y >= posY && transform.position.x < posX)
        {
            gameObject.transform.localScale = new Vector3(6f, 6f, 6f);
            anim.SetBool("isRun", false);
            anim.SetBool("isWalk", true);
        }
    }

}
