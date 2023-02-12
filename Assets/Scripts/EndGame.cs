using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public GameObject player;

    public GameObject wheel;

    public GameManager gameManager;

    public GameObject[] ToDeactivateAtEnd;

    public TextMeshProUGUI inGameScore;
    public TextMeshProUGUI inGameTimer;

    public TextMeshProUGUI endScore;
    public TextMeshProUGUI endTimer;

    public GameObject[] ToActivateAtEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in ToActivateAtEnd)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void endinGame()
    {
        foreach (GameObject go in ToActivateAtEnd)
        {
            go.SetActive(true);
        }
        if (endScore != null && inGameScore != null && endTimer != null && inGameTimer != null)
        {
            endScore.text = inGameScore.text;
            endTimer.text = inGameTimer.text;
        }
        foreach (GameObject go in ToDeactivateAtEnd)
        {
            Destroy(go);
            //go.SetActive(false);
        }
        Debug.Log("END GAME");
    }


    public void playAgain()
    {
        Debug.Log("PlayAgain");
        SceneManager.LoadScene("GameScene");
    }


    public void backToMenu()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene("MenuScene");
        SceneManager.UnloadSceneAsync("GameScene");
    }
}
