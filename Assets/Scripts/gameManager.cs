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
    private Color timing_highlightColor;
    private Color timing_originalColor;
    private float timingHighlightTime;
    private int timing_highlightTimes;

    // score management
    private int score;
    public TextMeshProUGUI textScore;
    private Color score_highlightColor;
    private Color score_originalColor;
    private float scoreHighlightTime;
    private int score_highlightTimes;

    // other data
    private int healthEquilibriumFactor;
    private float maxGramsFoodPerDay;

    // specifics diet data
    private float gnammsterDiet_maxGramsFoodPerDay;




    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        score_originalColor = new Color(textScore.color.r, textScore.color.g, textScore.color.b, 1f);
        score_highlightColor = new Color(0f, 255f, 0f, 1f);
        scoreHighlightTime = 0.3f;
        score_highlightTimes = 2;

        timing = 0;
        timing_originalColor = new Color(textTiming.color.r, textTiming.color.g, textTiming.color.b, 1f);
        timing_highlightColor = new Color(255f, 0f, 0f, 1f);
        timingHighlightTime = 0.3f;
        timing_highlightTimes = 2;

        rotator._speed = 3f;

        // food per day to correctly feed the animal
        gnammsterDiet_maxGramsFoodPerDay = 15f;

        maxGramsFoodPerDay = gnammsterDiet_maxGramsFoodPerDay;
        Nutrition.gramsSinglePortion = 15; // a modifier avec la valeur d'une portion (1-2 g)

        // The player eats the food and gets nutrients
        //GameObject[] nutritionObjects = GameObject.FindGameObjectsWithTag("Food");
        //foreach (GameObject go in nutritionObjects)
        //{
        //    go.GetComponent<Nutrition>().SetGramsSinglePortion(1);
        //}

        // Defining max values of gauges (grams)
        gaugesM.SetMaxGauges(gnammsterDiet_maxGramsFoodPerDay, gnammsterDiet_maxGramsFoodPerDay, gnammsterDiet_maxGramsFoodPerDay);

        // Threshold for each gauge (percentages) : [lower, equilibriumMin, equilibriumMax, upper]
        float[] gnammsterDietThresholds_lipids = {10f, 60f, 72f, 90f};
        float[] gnammsterDietThresholds_proteins = {5f, 12f, 18f, 50f};
        float[] gnammsterDietThresholds_carbos = {5f, 16f, 22f, 50f};

        gaugesM.SetThresholds("yellowGauge", maxGramsFoodPerDay * gnammsterDietThresholds_lipids[0] / 100, 
                                                maxGramsFoodPerDay * gnammsterDietThresholds_lipids[1] / 100, 
                                                maxGramsFoodPerDay * gnammsterDietThresholds_lipids[2] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_lipids[3] / 100);

        gaugesM.SetThresholds("redGauge", maxGramsFoodPerDay * gnammsterDietThresholds_proteins[0] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_proteins[1] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_proteins[2] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_proteins[3] / 100);

        gaugesM.SetThresholds("blueGauge", maxGramsFoodPerDay * gnammsterDietThresholds_carbos[0] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_carbos[1] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_carbos[2] / 100,
                                                maxGramsFoodPerDay * gnammsterDietThresholds_carbos[3] / 100);

        // Defining the descent pace of each gauge level (grams/second)
        gaugesM.SetDescentPace("yellowGauge", gnammsterDietThresholds_lipids[1] / 100);
        gaugesM.SetDescentPace("redGauge", gnammsterDietThresholds_proteins[1] / 100);
        gaugesM.SetDescentPace("blueGauge", gnammsterDietThresholds_carbos[1] / 100);

        // Defining initial values for gauges (grams)
        gaugesM.SetGauges(gaugesM.GetLowerThreshold("yellowGauge") * 2, gaugesM.GetLowerThreshold("redGauge") * 2, gaugesM.GetLowerThreshold("blueGauge") * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (score%1000 == 0) // test
        {
            foodM.spawnFood(1);
            foodM.spawnFood(2);
            foodM.spawnFood(3);
            StartCoroutine("scoreHighlightFeedback", 2); //test
            StartCoroutine("timingHighlightFeedback", 2); //test
        }    

        ComputeScore();
        ComputeTiming();
    }

    void ComputeScore()
    {
        healthEquilibriumFactor = gaugesM.GetEquilibriumFactor();
        score = score + (int)(rotator._speed * healthEquilibriumFactor);
        textScore.text = score.ToString();
    }

    public void EatFood(float lipidsV, float proteinsV, float carbosV)
    {
        gaugesM.AddGauges(lipidsV, proteinsV, carbosV, true);
    }

    void ComputeTiming()
    {
        timing = timing + Time.deltaTime;
        textTiming.text = timing.ToString("F2");
    }

    public void SaveScore()
    {
        //PlayerPrefs.SetInt("bestScore", score);
    }

    IEnumerator scoreHighlightFeedback(int scoreHighlightTimes)
    {
        for (int i = 0; i < scoreHighlightTimes; i++)
        {
            yield return new WaitForSeconds(scoreHighlightTime);
            textScore.color = score_highlightColor;
            yield return new WaitForSeconds(scoreHighlightTime);
            textScore.color = score_originalColor;
        }
    }

    IEnumerator timingHighlightFeedback(int scoreHighlightTimes)
    {
        for (int i = 0; i < scoreHighlightTimes; i++)
        {
            yield return new WaitForSeconds(timingHighlightTime);
            textTiming.color = timing_highlightColor;
            yield return new WaitForSeconds(timingHighlightTime);
            textTiming.color = timing_originalColor;
        }
    }
}
