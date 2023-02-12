using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugesManager : MonoBehaviour
{

    // Current values of gauges (grams)
    private float lipidsValue;
    private float proteinsValue;
    private float carbosValue;

    // Max values of gauges (grams)
    private float lipidsMaxValue;
    private float proteinsMaxValue;
    private float carbosMaxValue;

    // Defining critical gauge thresholds (grams) : [lower, equilibriumMin, equilibriumMax, upper]
    private float[] thresholds_yellowGauge;
    private float[] thresholds_redGauge;
    private float[] thresholds_blueGauge;

    // Sliders to manipulate the gauges' nutritional levels
    public Slider yellowGaugeSlider;
    public Slider redGaugeSlider;
    public Slider blueGaugeSlider;

    // Variable to highlight the gauges' fill area color
    public Image yellowGaugeFill;
    public Image redGaugeFill;
    public Image blueGaugeFill;

    // Variable to manipulate  the gauges' emotional mood : 1 means sad, 2 means normal, 3 means happy 
    private int yellowGaugeMood;
    private int redGaugeMood;
    private int blueGaugeMood;

    // Descent pace of each gauge level (grams/second)
    private float yellowGaugeDescentPace;
    private float redGaugeDescentPace;
    private float blueGaugeDescentPace;

    // Sprits to show gauges emotions in fontion of thresholds
    public Image yellowGaugeImage;
    public Image redGaugeImage;
    public Image blueGaugeImage;

    public Sprite yellowSadGauges, redSadGauges, blueSadGauges;
    public Sprite yellowNormalGauges, redNormalGauges, blueNormalGauges;
    public Sprite yellowHappyGauges, redHappyGauges, blueHappyGauges;

    private Color yellowGauge_originalColor;
    private Color redGauge_originalColor;
    private Color blueGauge_originalColor;
    private Color highlightColor;
    private float gaugeHighlightTime; // for how many seconds a single highlight lasts
    private int highlightTimes;



    // Start is called before the first frame update
    void Start()
    {
        thresholds_yellowGauge = new float[] { 0f, 0f, 0f, 0f };
        thresholds_redGauge = new float[] { 0f, 0f, 0f, 0f };
        thresholds_blueGauge = new float[] { 0f, 0f, 0f, 0f };

        SetGauges(0f, 0f, 0f);

        yellowGauge_originalColor = new Color(yellowGaugeFill.color.r, yellowGaugeFill.color.g, yellowGaugeFill.color.b, 1f);
        redGauge_originalColor = new Color(redGaugeFill.color.r, redGaugeFill.color.g, redGaugeFill.color.b, 1f);
        blueGauge_originalColor = new Color(blueGaugeFill.color.r, blueGaugeFill.color.g, blueGaugeFill.color.b, 1f);
        highlightColor = new Color(255f, 255f, 255f, 1f);
        gaugeHighlightTime = 0.1f;
        highlightTimes = 2;

    }

    private void Update()
    {
        descentGaugesProcess();
    }

    public int GetEquilibriumFactor()
    {
        return yellowGaugeMood + redGaugeMood + blueGaugeMood;
    }

    public void AddGauges(float lipidsV, float proteinsV, float carbosV, bool withHighlight)
    {
        SetGauges(lipidsValue + lipidsV, proteinsValue + proteinsV, carbosValue + carbosV);

        if (withHighlight && lipidsV > 0f) { StartCoroutine("yellowHighlightFeedback", highlightTimes); } // parameter is how many times we highlight the gauge
        if (withHighlight && proteinsV > 0f) { StartCoroutine("redHighlightFeedback", highlightTimes); }
        if (withHighlight && carbosV > 0f) { StartCoroutine("blueHighlightFeedback", highlightTimes); }
}

    public void SetGauges(float lipidsV, float proteinsV, float carbosV)
    {
        lipidsValue = Math.Min(lipidsV, lipidsMaxValue);
        proteinsValue = Math.Min(proteinsV, proteinsMaxValue);
        carbosValue = Math.Min(carbosV, carbosMaxValue);
        SetGaugesMood ();
        ShowGauges();
    }

    public void SetMaxGauges(float lipidsMaxV, float proteinsMaxV, float carbosMaxV)
    {
        lipidsMaxValue = lipidsMaxV;
        proteinsMaxValue = proteinsMaxV;
        carbosMaxValue = carbosMaxV;

        yellowGaugeSlider.minValue = 0f;
        redGaugeSlider.minValue = 0f;
        blueGaugeSlider.minValue = 0f;

        yellowGaugeSlider.maxValue = lipidsMaxValue;
        redGaugeSlider.maxValue = proteinsMaxValue;
        blueGaugeSlider.maxValue = carbosMaxValue;
    }

    void SetGaugesMood()
    {
        if (lipidsValue < thresholds_yellowGauge[0] || lipidsValue > thresholds_yellowGauge[3])         { yellowGaugeMood = 1; } 
        else if (lipidsValue > thresholds_yellowGauge[1] && lipidsValue < thresholds_yellowGauge[2])    { yellowGaugeMood = 3; }
        else { yellowGaugeMood = 2; }

        if (proteinsValue < thresholds_redGauge[0] || proteinsValue > thresholds_redGauge[3])           { redGaugeMood = 1; }
        else if (proteinsValue > thresholds_redGauge[1] && proteinsValue < thresholds_redGauge[2])      { redGaugeMood = 3; }
        else { redGaugeMood = 2; }

        if (carbosValue < thresholds_blueGauge[0] || carbosValue > thresholds_blueGauge[3])               { blueGaugeMood = 1; }
        else if (carbosValue > thresholds_blueGauge[1] && carbosValue < thresholds_blueGauge[2])          { blueGaugeMood = 3; }
        else { blueGaugeMood = 2; }
    }

    void ShowGauges()
    {
        yellowGaugeSlider.value = lipidsValue;
        redGaugeSlider.value = proteinsValue;
        blueGaugeSlider.value = carbosValue;

        if (yellowGaugeMood == 1) { yellowGaugeImage.sprite = yellowSadGauges; }
        else if (yellowGaugeMood == 2) { yellowGaugeImage.sprite = yellowNormalGauges; }
        else { yellowGaugeImage.sprite = yellowHappyGauges; }

        if (redGaugeMood == 1) { redGaugeImage.sprite = redSadGauges; }
        else if (redGaugeMood == 2) { redGaugeImage.sprite = redNormalGauges; }
        else { redGaugeImage.sprite = redHappyGauges; }

        if (blueGaugeMood == 1) { blueGaugeImage.sprite = blueSadGauges; }
        else if (blueGaugeMood == 2) { blueGaugeImage.sprite = blueNormalGauges; }
        else { blueGaugeImage.sprite = blueHappyGauges; }
    }


    public void SetThresholds(string gaugeType, float lower, float equilibriumMin, float equilibriumMax, float upper)
    {
        if (gaugeType == "yellowGauge")     { thresholds_yellowGauge = new float[] { lower, equilibriumMin, equilibriumMax, upper }; }
        else if (gaugeType == "redGauge")   { thresholds_redGauge = new float[] { lower, equilibriumMin, equilibriumMax, upper }; }
        else if (gaugeType == "blueGauge")  { thresholds_blueGauge = new float[] { lower, equilibriumMin, equilibriumMax, upper }; }
    }

    public float GetLowerThreshold(string gaugeType)
    {
        if (gaugeType == "yellowGauge")     { return thresholds_yellowGauge[0]; }
        else if (gaugeType == "redGauge")   { return thresholds_redGauge[0]; }
        else if (gaugeType == "blueGauge")  { return thresholds_blueGauge[0]; }
        else return 0f;
    }

    public void SetDescentPace(string gaugeType, float dp)
    {
        Debug.Log(gaugeType + " " + dp);
        if (gaugeType == "yellowGauge")     { yellowGaugeDescentPace = dp; }
        else if (gaugeType == "redGauge")   { redGaugeDescentPace = dp; }
        else if (gaugeType == "blueGauge")  { blueGaugeDescentPace = dp; }
    }

    void descentGaugesProcess()
    {
        lipidsValue -= yellowGaugeDescentPace * Time.deltaTime;
        proteinsValue -= redGaugeDescentPace * Time.deltaTime;
        carbosValue -= blueGaugeDescentPace * Time.deltaTime;

        lipidsValue = Math.Max(0f, lipidsValue);
        proteinsValue = Math.Max(0f, proteinsValue);
        carbosValue = Math.Max(0f, carbosValue);

        SetGauges(lipidsValue, proteinsValue, carbosValue);
        Debug.Log("descending ------> lipidsValue :" + lipidsValue + "      proteinsValue :" + proteinsValue + "     carbosValue :" + carbosValue);
    }

    IEnumerator yellowHighlightFeedback(int highlightTimes)
    {
        for (int i = 0; i< highlightTimes; i++)
        {
            yield return new WaitForSeconds(gaugeHighlightTime);
            yellowGaugeFill.color = highlightColor;
            yield return new WaitForSeconds(gaugeHighlightTime);
            yellowGaugeFill.color = yellowGauge_originalColor;
        }
    }

    IEnumerator redHighlightFeedback(int highlightTimes)
    {
        for (int i = 0; i < highlightTimes; i++)
        {
            yield return new WaitForSeconds(gaugeHighlightTime);
            redGaugeFill.color = highlightColor;
            yield return new WaitForSeconds(gaugeHighlightTime);
            redGaugeFill.color = redGauge_originalColor;
        }
    }

    IEnumerator blueHighlightFeedback(int highlightTimes)
    {
        for (int i = 0; i < highlightTimes; i++)
        {
            yield return new WaitForSeconds(gaugeHighlightTime);
            blueGaugeFill.color = highlightColor;
            yield return new WaitForSeconds(gaugeHighlightTime);
            blueGaugeFill.color = blueGauge_originalColor;
        }
    }

}
