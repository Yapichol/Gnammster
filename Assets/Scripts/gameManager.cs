using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        // Defining critical gauge thresholds (gauges) : [lower, equilibriumMin, equilibriumMax, upper]
        gaugesM.SetThresholds("yellowGauge", 100f, 400f, 600f, 900f); 
        gaugesM.SetThresholds("redGauge", 100f, 400f, 600f, 900f);
        gaugesM.SetThresholds("blueGauge", 100f, 400f, 600f, 900f);

        // Defining the descent pace of each gauge level (grams/second)
        gaugesM.SetDescentPace("yellowGauge", 5f);
        gaugesM.SetDescentPace("redGauge", 5f);
        gaugesM.SetDescentPace("blueGauge", 5f);

        // Defining initial values for gauges (grams)
        gaugesM.SetGauges(gaugesM.GetLowerThreshold("yellowGauge") * 2, gaugesM.GetLowerThreshold("redGauge") * 2, gaugesM.GetLowerThreshold("blueGauge") * 2);
    }

    // Update is called once per frame
    void Update()
    {
        foodM.freqSpawnMin = 0;
        foodM.freqSpawnMax = 0;

        if (score%1000 == 0)
        {
            foodM.spawnFood(1);
            foodM.spawnFood(2);
            foodM.spawnFood(3);
        }
            

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
        gaugesM.AddGauges(lipidsV, proteinsV, carboV);
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
