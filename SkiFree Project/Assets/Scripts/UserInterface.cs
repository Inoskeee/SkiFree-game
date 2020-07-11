using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
    [Header("Paused game")]
    public GameObject gamePausedInterface;
    public List<Button> buttons;


    [Header("Player Score")]
    public Text score;
    public Text record;

    public int countScore;

    [Header("Player Health")]
    public List<GameObject> health;

    [Header("Saving bar")]
    public GameObject saving;

    [Header("Loading")]
    public GameObject loading;

    private float currentHealth;
    private GameObject player;
    private Transform direction;
    private bool pause;
    private float timer = 1f;

    private int savedScore;

    private string filePath;
    void Start()
    {
        direction = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = player.GetComponent<MovingPlayer>().currentHealth;

        filePath = Application.persistentDataPath + "/score.ini";

        pause = false;

        savedScore = 0;
        LoadFile(ref savedScore);

        if (Checker.Instance.Language == "RU")
        {
            record.text = $"рекорд: {savedScore}";
        }
        else if (Checker.Instance.Language == "ENG")
        {
            record.text = $"record: {savedScore}";
        }
    }

    private void FixedUpdate()
    {
        if(direction != null)
        {
            if (direction.position.y <= 0 && Mathf.Abs((int)direction.position.y) >= countScore)
            {
                countScore = Mathf.Abs((int)direction.position.y);
            }

            if (Checker.Instance.Language == "RU")
            {
                score.text = $"счет: {countScore}";
            }
            else if (Checker.Instance.Language == "ENG")
            {
                score.text = $"score: {countScore}";
            }
        }
        else
        {
            buttons[0].gameObject.SetActive(false);
            buttons[2].gameObject.SetActive(true);
            gamePausedInterface.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            score.transform.position = new Vector3(buttons[0].transform.position.x, buttons[0].transform.position.y + 5, buttons[0].transform.position.z);
            score.alignment = TextAnchor.MiddleCenter;
            score.fontSize = 80;
        }
    }
    void Update()
    {
        buttons[0].onClick.AddListener(OnPause);
        buttons[1].onClick.AddListener(OnMenu);
        buttons[2].onClick.AddListener(GameStart);
        if (player != null)
        {
            currentHealth = player.GetComponent<MovingPlayer>().currentHealth;
        }
        else
        {
            currentHealth = 0;
        }

        if (currentHealth < health.Count)
        {
            GameObject.Destroy(health[health.Count - 1]);
            health.RemoveAt(health.Count - 1);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                timer = 1f;
                gamePausedInterface.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                timer = 0f;
                gamePausedInterface.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            pause = !pause;
        }

        Time.timeScale = timer;
    }

    private void OnPause()
    {
        pause = false;
        timer = 1f;
        gamePausedInterface.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMenu()
    {
        gamePausedInterface.SetActive(false);
        score.gameObject.SetActive(false);
        saving.SetActive(true);
        SaveFile();
    }

    private void GameStart()
    {
        gamePausedInterface.SetActive(false);
        buttons[2].gameObject.SetActive(false);
        loading.SetActive(true);
        if (savedScore < countScore)
        {
            SaveFile();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void SaveFile()
    {
        string text = "";
        if(savedScore < countScore)
        {
            text = countScore + "\n" + Checker.Instance.Language;
        }
        else
        {
            text = savedScore + "\n" + Checker.Instance.Language;
        }
        File.WriteAllText(filePath, text);
    }

    public void LoadFile(ref int savedScore)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string[] line = File.ReadAllLines(filePath);

        savedScore = int.Parse(line[0]);
    }
}

