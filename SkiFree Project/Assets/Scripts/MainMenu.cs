using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Menu")]
    public List<Button> buttons;
    

    [Header("Record")]
    public GameObject record;
    public Text userScore;

    [Header("Help")]
    public Text userHelp;

    [Header("Loading")]
    public GameObject loading;

    private int savedScore;
    private string filePath;
    void Start()
    {
        Canvas canv = gameObject.GetComponent<Canvas>();
        canv.worldCamera = FindObjectOfType<Camera>();

        filePath = Application.persistentDataPath + "/score.ini";

        savedScore = GameObject.FindGameObjectWithTag("Canvas").GetComponent<LanguageSelect>().savedScore;     
    }

    void Update()
    {
        buttons[0].onClick.AddListener(GameStart);
        buttons[1].onClick.AddListener(Raiting);
        buttons[2].onClick.AddListener(ExitGame);
        buttons[3].onClick.AddListener(Back);
        buttons[4].onClick.AddListener(Help);
    }

    private void GameStart()
    {
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        buttons[2].gameObject.SetActive(false);
        buttons[4].gameObject.SetActive(false);
        loading.SetActive(true);

    }

    private void ExitGame()
    {
        SaveFile();
        Application.Quit();
    }

    private void Raiting()
    {
        if (Checker.Instance.Language == "RU")
            userScore.text = "Рекорд: " + savedScore;
        else if(Checker.Instance.Language == "ENG")
            userScore.text = "Record: " + savedScore;
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        buttons[2].gameObject.SetActive(false);
        buttons[4].gameObject.SetActive(false);
        userScore.gameObject.SetActive(true);
        userHelp.gameObject.SetActive(false);
        record.SetActive(true);
    }

    private void Back()
    {
        record.SetActive(false);
        buttons[0].gameObject.SetActive(true);
        buttons[1].gameObject.SetActive(true);
        buttons[2].gameObject.SetActive(true);
        buttons[4].gameObject.SetActive(true);
    }

    private void Help()
    {
        if (Checker.Instance.Language == "RU")
            userHelp.text = "> Для передвижения используйте клавиши A, S, W, D или стрелки на клавиатуре\n" +
                "> Для ускорения нажмите Shift\n" +
                "> Для торможения зажмите Backspace";
        else if (Checker.Instance.Language == "ENG")
            userHelp.text = "> Use the A, S, W, D keys or the arrow keys on the keyboard to move around.\n" +
                "> Press Shift for accelerating\n" +
                "> Press Backspace for braking.";
        buttons[0].gameObject.SetActive(false);
        buttons[1].gameObject.SetActive(false);
        buttons[2].gameObject.SetActive(false);
        buttons[4].gameObject.SetActive(false);
        userScore.gameObject.SetActive(false);
        userHelp.gameObject.SetActive(true);
        record.SetActive(true);
    }
    public void SaveFile()
    {
        string text = savedScore + "\n" + Checker.Instance.Language;
        File.WriteAllText(filePath, text);
    }


    
}
