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



    // specifics diet data
    private float gnammsterDiet_maxGramsFoodPerDay;
    private int[] gnammsterDiet_goodFood;
    private int[] gnammsterDiet_badFood;
    private int[] gnammsterDiet_ambiguousGoodFood;
    private int[] gnammsterDiet_ambiguousBadFood;
    private float[] gnammsterDietThresholds_lipids;
    private float[] gnammsterDietThresholds_proteins;
    private float[] gnammsterDietThresholds_carbos;

    // other data
    private int healthEquilibriumFactor;
    private float maxGramsFoodPerDay;
    private float speedPace;
    private float maxSpeedWheel;

    // Adaptation
    private int nbSlidingWindowsCreated;
    private int[] slidingWindow_food;
    private int[] slidingWindow_actionPlayer;
    private int currentIndexWindow;
    private bool beginnerMode;
    private int[] foodSequence;
    private bool launchNewSlidingWindow;
    private float freqFood;

    // Start is called before the first frame update
    void Start()
    {
        beginnerMode = true;

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

        rotator._speed = 2f;
        speedPace = 0.1f;
        maxSpeedWheel = 6f;

        // food per day to correctly feed the animal
        gnammsterDiet_maxGramsFoodPerDay = 15f;

        maxGramsFoodPerDay = gnammsterDiet_maxGramsFoodPerDay;
        Nutrition.gramsSinglePortion = 1; // a modifier avec la valeur d'une portion (1-2 g)

        gnammsterDiet_goodFood = new int[] { 0, 1, 2, 3, 4, 5 };
        gnammsterDiet_badFood = new int[] { 6, 7, 8 };
        gnammsterDiet_ambiguousGoodFood = new int[] { 9, 10, 11 };
        gnammsterDiet_ambiguousBadFood = new int[] { 12, 13, 14 };

        // Initialise first Sliding Windows
        nbSlidingWindowsCreated = 0;
        ResetSlidingWindow(5);
        foodSequence = new int[] {0,0,0,0,0};
        freqFood = 2f;
        



    // The player eats the food and gets nutrients
    //GameObject[] nutritionObjects = GameObject.FindGameObjectsWithTag("Food");
    //foreach (GameObject go in nutritionObjects)
    //{
    //    go.GetComponent<Nutrition>().SetGramsSinglePortion(1);
    //}

    // Defining max values of gauges (grams)
        gaugesM.SetMaxGauges(gnammsterDiet_maxGramsFoodPerDay, gnammsterDiet_maxGramsFoodPerDay, gnammsterDiet_maxGramsFoodPerDay);

        // Threshold for each gauge (percentages) : [lower, equilibriumMin, equilibriumMax, upper]
        gnammsterDietThresholds_lipids = new float[] {21f, 60f, 72f, 90f};
        gnammsterDietThresholds_proteins = new float[] { 5f, 12f, 18f, 50f};
        gnammsterDietThresholds_carbos = new float[] { 7f, 16f, 22f, 50f};


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
        /*gaugesM.SetDescentPace("yellowGauge", gnammsterDietThresholds_lipids[1] / 100);
        gaugesM.SetDescentPace("redGauge", gnammsterDietThresholds_proteins[1] / 100);
        gaugesM.SetDescentPace("blueGauge", gnammsterDietThresholds_carbos[1] / 100);*/
        gaugesM.SetDescentPace("yellowGauge", 0f);
        gaugesM.SetDescentPace("redGauge", 0f);
        gaugesM.SetDescentPace("blueGauge", 0f);

        // Defining initial values for gauges (grams)
        gaugesM.SetGauges(gaugesM.GetLowerThreshold("yellowGauge") * 2, gaugesM.GetLowerThreshold("redGauge") * 2, gaugesM.GetLowerThreshold("blueGauge") * 2);

        launchNewSlidingWindow = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (score%1000 == 0) // test
        //{
        //    foodM.spawnFood(1);
        //    StartCoroutine("scoreHighlightFeedback", 2); //test
        //    StartCoroutine("timingHighlightFeedback", 2); //test
        //}    
        if (launchNewSlidingWindow)
        {
            launchNewSlidingWindow = false;
            StartCoroutine(CreateFoodSequenceProcess(freqFood));
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

    IEnumerator timingHighlightFeedback(int timingHighlightTimes)
    {
        for (int i = 0; i < timingHighlightTimes; i++)
        {
            yield return new WaitForSeconds(timingHighlightTime);
            textTiming.color = timing_highlightColor;
            yield return new WaitForSeconds(timingHighlightTime);
            textTiming.color = timing_originalColor;
        }
    }

    private void ResetSlidingWindow(int slidingWindowSize)
    {
        
        slidingWindow_actionPlayer = new int[slidingWindowSize];
        slidingWindow_food = new int[slidingWindowSize];
        foodSequence = new int[slidingWindowSize];
        currentIndexWindow = 0;
    }

    public void SetSlidingWindow(int indexFood, int eaten)
    {
        slidingWindow_food[currentIndexWindow] = indexFood;
        slidingWindow_actionPlayer[currentIndexWindow] = eaten * healthEquilibriumFactor;
        currentIndexWindow++;
        
        if (currentIndexWindow == slidingWindow_food.Length) { analyzer(); }
    }

    IEnumerator CreateFoodSequenceProcess(float intervalSpawn)
    {
        Debug.Log("YOUPI");
        foreach (int food in foodSequence)
        {
            foodM.spawnFood(food);
            yield return new WaitForSeconds(intervalSpawn + Random.Range(-0.1f * intervalSpawn, 0.1f * intervalSpawn));
        }
    }

    void analyzer()
    {
        Debug.Log("Analyzer");

        // First sliding window (introduction)
        if (gaugesM.GetDescentPace("yellowGauge") == 0f){
            int nbMissed = 0;
            foreach(int val in slidingWindow_actionPlayer)
            {
                if (val == 0)
                {
                    nbMissed += 1;
                }
            }
            if(nbMissed > 0)
            {
                currentIndexWindow = 0;
                ResetSlidingWindow(nbMissed);
                for (int i = 0; i < nbMissed; i++)
                {
                    foodSequence[i] = 0;
                }
                launchNewSlidingWindow = true;
                return;
            }

            nbSlidingWindowsCreated++;
            gaugesM.SetDescentPace("yellowGauge", gnammsterDietThresholds_lipids[1] / 100);
            gaugesM.SetDescentPace("redGauge", gnammsterDietThresholds_proteins[1] / 100);
            gaugesM.SetDescentPace("blueGauge", gnammsterDietThresholds_carbos[1] / 100);

            rotator._speed = Mathf.Min(rotator._speed + (rotator._speed * nbSlidingWindowsCreated * speedPace), maxSpeedWheel);
        }


        /*ResetSlidingWindow(slidingWindowSize);

        for (int i=0; i < slidingWindowSize; i++)
        {
            foodSequence[i] = 0;
        }*/
        
    }

}
