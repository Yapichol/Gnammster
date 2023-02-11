using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class GameManager : MonoBehaviour
{

    // player and wheel management
    public PlayerController player;
    public Rotator rotator;

    // managers
    public GaugesManager gaugesM;
    public FoodManager foodM;
    public ObstaclesManager obstaclesM;

    // timing management
    private float timing;
    public TextMeshProUGUI textTiming;

    // score management
    private int score;
    public TextMeshProUGUI textScore;

    // other data
    private int healthEquilibriumFactor;




    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        timing = 0;
        rotator._speed = 3f;

        // Defining max values of gauges (grams)
        gaugesM.SetMaxGauges(1000f, 1000f, 1000f); // à changer avec données reelles

        // Defining critical gauge thresholds : [lower, equilibriumMin, equilibriumMax, upper]
        gaugesM.thresholds_yellowGauge = new float[] { 100f, 400f, 600f, 900f }; 
        gaugesM.thresholds_redGauge = new float[] { 100f, 400f, 600f, 900f };
        gaugesM.thresholds_blueGauge = new float[] { 100f, 400f, 600f, 900f };
    }

    // Update is called once per frame
    void Update()
    {
        foodM.freqSpawnMin = 0;
        foodM.freqSpawnMax = 5;
        foodM.spawnFood(1);

        ComputeScore();
        ComputeTiming();
    }

    void ComputeScore()
    {
        healthEquilibriumFactor = gaugesM.GetEquilibriumFactor();
        score = score + (int)(rotator._speed * healthEquilibriumFactor);
        //Debug.Log(score);
        textScore.text = score.ToString();
    }

    public void EatFood(float lipidsV, float proteinsV, float carboV)
    {
        gaugesM.SetGauges(lipidsV, proteinsV, carboV);
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
