using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Tobii;



public class GameManager : MonoBehaviour
{
    // Audioclips
    public AudioSource audioSource;
    public AudioClip soundEat;
    public AudioClip soundDisgust;
    public AudioClip soundObstacles;

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
    public Tobii.Gaming.Examples.GazePointData.EyeTracker eyeTrack;
    private int nbSlidingWindowsCreated;
    private int[] slidingWindow_food;
    private int[] slidingWindow_statePlayer;
    private int[] slidingWindow_eaten;
    private float[] slidingWindow_lookedGauges;
    private int currentIndexWindow;
    private bool beginnerMode;
    private int[] foodSequence;
    private bool launchNewSlidingWindow;
    private float freqFood;
    private float levelPlayer;
    private float[] performencesPlayer;
    private float[] attentionPlayer;
    private float timeLastEat;
    public int sizeAdaptation = 10;




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
        performencesPlayer = new float[sizeAdaptation];
        for (int i = 0; i < performencesPlayer.Length; i++)
        {
            performencesPlayer[i] = 0;
        }
        attentionPlayer = new float[sizeAdaptation];
        for (int i = 0; i < attentionPlayer.Length; i++)
        {
            attentionPlayer[i] = 0;
        }

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

    public void EatFood(float lipidsV, float proteinsV, float carbosV, int indexFood)
    {
        timeLastEat = Time.time;
        if (isGood(indexFood)){
            audioSource.PlayOneShot(soundEat);
            gaugesM.AddGauges(lipidsV, proteinsV, carbosV, true);
        }
        else
        {
            audioSource.PlayOneShot(soundDisgust);
            gaugesM.AddGauges(-lipidsV, -proteinsV, -carbosV, true);
        }
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
        //    StartCoroutine("scoreHighlightFeedback", 2);
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
        //    StartCoroutine("timingHighlightFeedback", 2);
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
        slidingWindow_statePlayer = new int[slidingWindowSize];
        slidingWindow_food = new int[slidingWindowSize];
        slidingWindow_eaten = new int[slidingWindowSize];
        slidingWindow_lookedGauges = new float[slidingWindowSize];
        foodSequence = new int[slidingWindowSize];
        currentIndexWindow = 0;
    }

    public void SetSlidingWindow(int indexFood, int eaten)
    {
        slidingWindow_food[currentIndexWindow] = indexFood;
        slidingWindow_statePlayer[currentIndexWindow] = healthEquilibriumFactor;
        slidingWindow_eaten[currentIndexWindow] = eaten;
        currentIndexWindow++;
        
        if (currentIndexWindow == slidingWindow_food.Length) { analyzer(); }
    }

    IEnumerator CreateFoodSequenceProcess(float intervalSpawn)
    {
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
            foreach(int val in slidingWindow_eaten)
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
            gaugesM.SetDescentPace("yellowGauge", (gnammsterDietThresholds_lipids[1]/6) / 100);
            gaugesM.SetDescentPace("redGauge", (gnammsterDietThresholds_proteins[1]/4) / 100);
            gaugesM.SetDescentPace("blueGauge", (gnammsterDietThresholds_carbos[1]/4) / 100);

            rotator._speed = Mathf.Min(rotator._speed + (rotator._speed * nbSlidingWindowsCreated * speedPace), maxSpeedWheel);
            ResetSlidingWindow(5);
            for (int i = 0; i < foodSequence.Length; i++)
            {
                foodSequence[i] = 0;
            }
            int bad1 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];
            int bad2 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];

            int badOrder1 = Random.Range(0, foodSequence.Length);
            int badOrder2 = badOrder1;
            if (foodSequence.Length > 1)
            {
                while (badOrder2 == badOrder1)
                {
                    badOrder2 = Random.Range(0, foodSequence.Length);
                }
            }
            foodSequence[badOrder1] = bad1;
            foodSequence[badOrder2] = bad2;
            launchNewSlidingWindow = true;
            return;
        }
        else if (beginnerMode)
        {
            float perf = getPerformenceOnWindow();
            updateLevelPlayer(perf);
            rotator._speed = Mathf.Min(rotator._speed + (rotator._speed * nbSlidingWindowsCreated * speedPace), maxSpeedWheel);
            Debug.Log(levelPlayer);
            if(levelPlayer > 0.14)
            {
                beginnerMode = false;
            }
            else
            {
                ResetSlidingWindow(5);
                for (int i = 0; i < foodSequence.Length; i++)
                {
                    foodSequence[i] = 0;
                }
                int bad1 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];
                int bad2 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];
                int bad3 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];

                int badOrder1 = Random.Range(0, foodSequence.Length);
                int badOrder2 = badOrder1;
                if (foodSequence.Length > 1)
                {
                    while (badOrder2 == badOrder1)
                    {
                        badOrder2 = Random.Range(0, foodSequence.Length);
                    }
                }
                int badOrder3 = badOrder2;
                if (foodSequence.Length > 2)
                {
                    while (badOrder3 == badOrder1 && badOrder3 == badOrder2)
                    {
                        badOrder3 = Random.Range(0, foodSequence.Length);
                    }
                }
                foodSequence[badOrder1] = bad1;
                foodSequence[badOrder2] = bad2;
                foodSequence[badOrder3] = bad3;
                launchNewSlidingWindow = true;
                obstaclesM.create = false;
                beginnerMode = false;
                return;
            }
        }
        obstaclesM.create = false;
        float perfs = getPerformenceOnWindow();
        updateLevelPlayer(perfs);
        rotator._speed = Mathf.Min(rotator._speed + (rotator._speed * nbSlidingWindowsCreated * speedPace), maxSpeedWheel);
        Debug.Log("Niveau : " + levelPlayer);
        if (levelPlayer > 0.4f)
        {
            freqFood = 1.5f;
            ResetSlidingWindow(5);
            for (int i = 0; i < foodSequence.Length; i++)
            {
                foodSequence[i] = gnammsterDiet_goodFood[Random.Range(0, gnammsterDiet_goodFood.Length)]; ;
            }
            int AmbGood = gnammsterDiet_ambiguousGoodFood[Random.Range(0, gnammsterDiet_ambiguousGoodFood.Length)];
            int bad = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];
            int AmbBad = gnammsterDiet_ambiguousBadFood[Random.Range(0, gnammsterDiet_ambiguousBadFood.Length)];

            int AmbGoodOrder = Random.Range(0, foodSequence.Length);
            int badOrder = AmbGoodOrder;
            if (foodSequence.Length > 1)
            {
                while (badOrder == AmbGoodOrder)
                {
                    badOrder = Random.Range(0, foodSequence.Length);
                }
            }
            int AmbBadOrder = badOrder;
            if (foodSequence.Length > 2)
            {
                while (AmbBadOrder == AmbGoodOrder && AmbBadOrder == badOrder)
                {
                    AmbBadOrder = Random.Range(0, foodSequence.Length);
                }
            }
            foodSequence[AmbGoodOrder] = AmbGood;
            foodSequence[badOrder] = bad;
            foodSequence[AmbBadOrder] = AmbBad;
            launchNewSlidingWindow = true;
        }
        else
        {
            freqFood = 1.75f;
            ResetSlidingWindow(5);
            for (int i = 0; i < foodSequence.Length; i++)
            {
                foodSequence[i] = gnammsterDiet_goodFood[Random.Range(0, gnammsterDiet_goodFood.Length)];
            }
            int bad1 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];
            int bad2 = gnammsterDiet_badFood[Random.Range(0, gnammsterDiet_badFood.Length)];

            int badOrder1 = Random.Range(0, foodSequence.Length);
            int badOrder2 = badOrder1;
            if (foodSequence.Length > 1)
            {
                while (badOrder2 == badOrder1)
                {
                    badOrder2 = Random.Range(0, foodSequence.Length);
                }
            }
            foodSequence[badOrder1] = bad1;
            foodSequence[badOrder2] = bad2;
            launchNewSlidingWindow = true;
        }
    }


    float getPerformenceOnWindow()
    {
        float sum = 0;/*
        for(int eq = 0; eq < slidingWindow_statePlayer.Length; eq++)
        {
            if ((!isGood(slidingWindow_food[eq])) && (slidingWindow_eaten[eq] == 1))
            {
                sum = sum + 0;
            }
            else
            {
                sum = sum + slidingWindow_statePlayer[eq];
            }
        }*/
        if (eyeTrack.get_activated())
        {
            int nb_eaten = 0;
            for (int eq = 0; eq < slidingWindow_statePlayer.Length; eq++)
            {
                if ((slidingWindow_eaten[eq] == 1))
                {
                    sum = sum + slidingWindow_lookedGauges[eq];
                    nb_eaten++;
                }
            }
            sum = sum / (nb_eaten);
        }
        return sum;
    }


    void updateLevelPlayer(float newPerf)
    {
        /*for(int i = performencesPlayer.Length - 2; i >= 0; i--)
        {
            performencesPlayer[i + 1] = performencesPlayer[i];
        }
        performencesPlayer[0] = newPerf;
        float sum = 0;
        foreach(float perf in performencesPlayer)
        {
            sum = sum + perf;
        }
        levelPlayer = sum / performencesPlayer.Length;*/
        if (eyeTrack.get_activated())
        {
            for (int i = attentionPlayer.Length - 2; i >= 0; i--)
            {
                attentionPlayer[i + 1] = attentionPlayer[i];
            }
            attentionPlayer[0] = newPerf;
            float sum = 0;
            foreach (float perf in attentionPlayer)
            {
                sum = sum + perf;
            }
            levelPlayer = sum / attentionPlayer.Length;
        }
    }


    bool isGood(int indexFood)
    {
        foreach(int ind in gnammsterDiet_goodFood)
        {
            if(ind == indexFood)
            {
                return true;
            }
        }
        foreach (int ind in gnammsterDiet_ambiguousGoodFood)
        {
            if (ind == indexFood)
            {
                return true;
            }
        }
        return false;
    }

    public void lookedAtGauges()
    {
        float lookedTime = Time.time;
        
        float timeSpend = lookedTime - timeLastEat;
        Debug.Log(timeSpend);
        if (timeSpend >= 0f && timeSpend <= 1f)
        {
            Debug.Log("LOOKED AT 2");
            slidingWindow_lookedGauges[currentIndexWindow] = 1f;
        }
    }

}
