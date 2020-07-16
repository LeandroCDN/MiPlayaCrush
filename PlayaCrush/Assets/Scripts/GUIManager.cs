using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{
    public static GUIManager sharedInstance;

   
    public Text movesText, scoreText;
    private int score;
    private int moveCounter;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = "Score :" + score;
        }
    }

    public int MoveCounter
    {
        get { return moveCounter; }
        set 
        {
            moveCounter = value;
            movesText.text = "Moves: " + moveCounter;
            if(moveCounter <= 0)
            {
                moveCounter = 0;
                StartCoroutine (GameOver());
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        moveCounter = 10;
        score = 0;
        movesText.text = "Moves: " + moveCounter;
        scoreText.text = "Score: " + score;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);
        yield return new WaitForSeconds(0.75f);
        ChangeSceneTo("GameOver");
    }

    public void ChangeSceneTo(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
       
    }
}
