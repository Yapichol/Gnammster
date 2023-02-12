using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    public GameObject player;

    public GameObject wheel;

    public GameManager gm;

    public GameObject[] inGameUI;

    public TextMeshProUGUI endScore;
    public TextMeshProUGUI endTimer;

    public GameObject[] endGameUI;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject go in endGameUI)
        {
            go.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void endinGame()
    {
        foreach (GameObject go in inGameUI)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in endGameUI)
        {
            go.SetActive(true);
        }
    }
}
