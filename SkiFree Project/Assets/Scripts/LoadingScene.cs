using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LoadingScene : MonoBehaviour
{
    [Header("Loading")]
    public Scrollbar img;
    public string scene;
    void Start()
    {
        StartCoroutine(Loading());
    }

    IEnumerator Loading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        
        while (!operation.isDone)
        {
            img.size = operation.progress;
            yield return null;
        }
    }
}
