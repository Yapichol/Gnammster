using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class gameManager : MonoBehaviour
{

    // timing
    private float timing;
    public TextMeshProUGUI textTiming;

    // score
    private int score;
    public TextMeshProUGUI textScore;
    // healthEquilibriumFactor; to get in jauges script
    public Rotator rotator;



    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        timing = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ComputeScore();
        ComputeTiming();
    }

    void ComputeScore()
    {
        int healthEquilibriumFactor = 1; // to delete when we'll have the good value from jauges script
        score = score + (int) (rotator._speed * healthEquilibriumFactor);
        //Debug.Log(score);
        textScore.text = score.ToString();
    }

    void ComputeTiming()
    {
        timing = timing + Time.deltaTime;
        //Debug.Log("timing :" + timing.ToString()); 
        textTiming.text = timing.ToString("F2");
    }

    public void SaveScore()
    {
        //PlayerPrefs.SetInt("bestScore", score);
    }

}
