using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LanguageSelect : MonoBehaviour
{
    [Header("MainMenu")]
    public List<GameObject> languages;

    public List<Button> lang_buttons;

    public int savedScore;
    private string savedLang;
    private string filePath;

    private GameObject currentLang;
    private Camera mainCam;
    private Canvas canv;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/score.ini";

        savedScore = 0;
        if (File.Exists(filePath))
        {
            LoadFile(ref savedScore, ref savedLang);
            Checker.Instance.Language = savedLang;
        }
        else
        {
            Checker.Instance.Language = "RU";
        }
        
    }
    void Start()
    {
        if(Checker.Instance.Language == "RU")
        {
            currentLang = Instantiate(languages[0]);
        }
        else if(Checker.Instance.Language == "ENG")
        {
            currentLang = Instantiate(languages[1]);
        }
        mainCam = Camera.FindObjectOfType<Camera>();
    }

    void Update()
    {

    }

    public void Select_Russian()
    {
        if(languages[0] != currentLang)
        {
            Destroy(currentLang);
            currentLang = Instantiate(languages[0]);
            canv = currentLang.GetComponent<Canvas>();
            canv.worldCamera = mainCam;
            Checker.Instance.Language = "RU";
        }
    }

    public void Select_English()
    {
        if (languages[1] != currentLang)
        {
            Destroy(currentLang);
            currentLang = Instantiate(languages[1]);
            canv = currentLang.GetComponent<Canvas>();
            canv.worldCamera = mainCam;
            Checker.Instance.Language = "ENG";
        }
    }


    public void LoadFile(ref int savedScore, ref string savedLang)
    {
        if (!File.Exists(filePath))
        {
            return;
        }

        string[] line = File.ReadAllLines(filePath);

        savedScore = int.Parse(line[0]);
        if(line.Length >= 2)
        {
            Debug.Log(line[1]);
            savedLang = line[1];
        }
        else
        {
            savedLang = "RU";
        }
        
    }
}
