using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [Header("Camera features")]
    public float dumping = 1.5f; //Сглаживание при увеличении/уменьшении камеры
    public float speed = 50f;    //Скорость смещения камеры
    public float minSize;        //Минимальный размер камеры
    public float maxSize;        //Максимальный размер камеры
    public float extraSize;

    private Vector3 offset;      //Смещение камеры к позиции игрока

    //Переменные игрока
    private Transform target;       //Позиция игрока
    private Rigidbody2D playerVel;  //Физический компонент игрока
    private MovingPlayer player;    //Компонент MovingPlayer


    private GameObject currentLang;

    public List<GameObject> gameLang;

    private void Awake()
    {

        if (Checker.Instance.Language == "RU")
        {
            currentLang = Instantiate(gameLang[0]);
        }
        else if(Checker.Instance.Language == "ENG")
        {
            currentLang = Instantiate(gameLang[1]);
        }
        Canvas canv = currentLang.GetComponentInChildren<Canvas>();
        canv.worldCamera = Camera.FindObjectOfType<Camera>();
    }
    void Start()
    {
        playerVel = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovingPlayer>();
        //Текущая позиция камеры
        offset = transform.position;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if(target != null)
        {
            //Координаты смещения камеры
            offset = new Vector3(target.position.x, target.position.y, -10);

            //Плавное смещение камеры
            transform.position = Vector3.MoveTowards(transform.position, offset, speed * Time.deltaTime);
        }
        
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            if (playerVel.velocity.y == -player.maxSpeed && Camera.main.orthographicSize < maxSize)
            {
                //Плавно увеличиваем размер камеры
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, maxSize, dumping * Time.deltaTime);
            }
            else if (playerVel.velocity.y > -player.maxSpeed && Camera.main.orthographicSize > minSize)
            {
                //Плавно уменьшаем размер камеры
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, minSize, dumping * Time.deltaTime);
            }

            if(playerVel.velocity.y < -player.maxSpeed && Camera.main.orthographicSize < extraSize)
            {
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, extraSize, dumping * Time.deltaTime);
            }
            else if (playerVel.velocity.y > -player.maxAccelerate && Camera.main.orthographicSize > maxSize)
            {
                //Плавно уменьшаем размер камеры
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, maxSize, dumping * Time.deltaTime);
            }
        }
        
    }

}
