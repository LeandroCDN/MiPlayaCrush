using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + GUIManager.sharedInstance.Score;
    }

    public void ChangeSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
