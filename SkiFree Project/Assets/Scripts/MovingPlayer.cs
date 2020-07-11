using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MovingPlayer : MonoBehaviour
{
    [Header("Characteristics")]
    public float maxSpeed;
    public float maxAccelerate;
    public float currentHealth;


    [Header("Coordinates")]
    public Vector2 mover;

    [Header("Trail")]
    public GameObject step;

    [Header("Accelerate")]
    public float timeToSpeed;

    private TrailRenderer trail;

    private Rigidbody2D rigid;
    private float speed;
    private float checker;

    private Animator anim;

    private bool time;
    private float isTimer;

    private int col = 0;

    void Start()
    {
        trail = step.GetComponent<TrailRenderer>();
        rigid = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        time = true;
    }

    void FixedUpdate()
    {
        rigid.velocity = mover * speed;
    }
    void Update()
    {
        if (time == true)
        {
            speed = maxSpeed;
            
            if (mover.y < 0 && checker <= 0)
            {
                mover.Set(Input.GetAxis("Horizontal"), -1);
                anim.SetBool("MoveDown", true);
                trail.numCornerVertices = 90;
                trail.numCapVertices = 90;

                ChangeCoordinates();

                if (mover.x == 0)
                {
                    anim.SetBool("Turn", false);
                    anim.SetBool("Move", false);
                }
                else if (mover.x != 0)
                {
                    anim.SetBool("Turn", true);
                    anim.SetBool("Move", true);
                }

                if (Input.GetButton("Accelerate"))
                {
                    speed = maxAccelerate;
                }
                else
                {
                    if (speed != maxSpeed)
                    {
                        speed -= 0.5f;
                    }
                }
            }
            else if (rigid.velocity.y == 0 && Input.GetAxis("Vertical") <= 0)
            {
                mover.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                speed /= 2f;
                trail.numCornerVertices = 0;
                trail.numCapVertices = 0;

                ChangeCoordinates();

                if (rigid.velocity.x != 0)
                {
                    anim.SetBool("Walk", true);
                }
                else if (mover.x == 0)
                {
                    anim.SetBool("Walk", false);
                }
            }
            else if (rigid.velocity.y >= 0)
            {
                mover.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                speed /= 5f;

                ChangeCoordinates();

                if (mover.y > 0)
                {
                    if (mover.x != 0)
                    {
                        anim.SetBool("Walk", true);
                    }
                    else
                    {
                        anim.SetBool("Walk", false);
                    }
                    anim.SetBool("WalkUp", true);
                }
                else if (mover.y == 0)
                {
                    anim.SetBool("WalkUp", false);
                }

            }

            if (Input.GetKey(KeyCode.Space) && rigid.velocity.y < 0)
            {
                checker = 1;

                mover.Set(Input.GetAxis("Horizontal"), mover.y + Time.deltaTime);
                if (mover.y > 0)
                {
                    mover.y = 0;
                    anim.SetBool("MoveDown", false);
                }
            }
            else
            {
                checker = 0;
            }
        }
        else
        {
            switch (col)
            {
                case 0:
                    transform.position += new Vector3(0.02f, 0.01f, transform.position.z);
                    break;
                case 1:
                    transform.position += new Vector3(-0.02f, 0.01f, transform.position.z);
                    break;
                case 2:
                    transform.position += new Vector3(-0.01f, -0.01f, transform.position.z);
                    break;
            }
            isTimer += Time.deltaTime;
            if (isTimer >= 2)
            {
                time = true;
                isTimer = 0;
                anim.SetBool("isFall", false);
                anim.SetBool("Move", false);
                anim.SetBool("MoveDown", false);
                anim.SetBool("Walk", false);
                anim.SetBool("WalkUp", false);
                anim.SetBool("Turn", false);
            }
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void ChangeCoordinates()
    {
        if (mover.x > 0)
        {
            gameObject.transform.localScale = new Vector3(-5f, 5f, 5f);
        }
        else if (mover.x < 0)
        {
            gameObject.transform.localScale = new Vector3(5f, 5f, 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstruction")
        {
            anim.SetBool("isFall", true);

            currentHealth--;
            if (rigid.velocity.y <= 0)
            {
                if (rigid.velocity.x < 0)
                {
                    col = 0;
                }
                else if(rigid.velocity.x > 0)
                {
                    col = 1;
                }
            }
            else if(rigid.velocity.y>0)
            {
                col = 2;
            }
            rigid.velocity = new Vector2(0, 0);
            mover.x = 0;
            mover.y = 0;
            time = false;
        }
    }



}
