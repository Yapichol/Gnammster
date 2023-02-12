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
    private float carboValue;

    // Max values of gauges (grams)
    private float lipidsMaxValue;
    private float proteinsMaxValue;
    private float carboMaxValue;

    // Defining critical gauge thresholds (grams) : [lower, equilibriumMin, equilibriumMax, upper]
    private float[] thresholds_yellowGauge;
    private float[] thresholds_redGauge;
    private float[] thresholds_blueGauge;

    // Sliders to manipulate the gauges' nutritional levels
    public Slider yellowGaugeSlider;
    public Slider redGaugeSlider;
    public Slider blueGaugeSlider;

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



    // Start is called before the first frame update
    void Start()
    {
        thresholds_yellowGauge = new float[] { 0f, 0f, 0f, 0f };
        thresholds_redGauge = new float[] { 0f, 0f, 0f, 0f };
        thresholds_blueGauge = new float[] { 0f, 0f, 0f, 0f };

        SetGauges(0f, 0f, 0f);
    }

    private void Update()
    {
        descentGaugesProcess();
    }

    public int GetEquilibriumFactor()
    {
        return yellowGaugeMood + redGaugeMood + blueGaugeMood;
    }


    public void AddGauges(float lipidsV, float proteinsV, float carboV)
    {
        SetGauges(lipidsValue + lipidsV, proteinsValue + proteinsV, carboValue + carboV);
    }

    public void SetGauges(float lipidsV, float proteinsV, float carboV)
    {
        lipidsValue = lipidsV;
        proteinsValue = proteinsV;
        carboValue = carboV;
        SetGaugesMood();
        ShowGauges();
    }

    public void SetMaxGauges(float lipidsMaxV, float proteinsMaxV, float carboMaxV)
    {
        lipidsMaxValue = lipidsMaxV;
        proteinsMaxValue = proteinsMaxV;
        carboMaxValue = carboMaxV;

        yellowGaugeSlider.minValue = 0f;
        redGaugeSlider.minValue = 0f;
        blueGaugeSlider.minValue = 0f;

        yellowGaugeSlider.maxValue = lipidsMaxValue;
        redGaugeSlider.maxValue = proteinsMaxValue;
        blueGaugeSlider.maxValue = carboMaxValue;
    }

    void SetGaugesMood()
    {
        if (lipidsValue < thresholds_yellowGauge[0] || lipidsValue > thresholds_yellowGauge[3])         { yellowGaugeMood = 1; } 
        else if (lipidsValue > thresholds_yellowGauge[1] && lipidsValue < thresholds_yellowGauge[2])    { yellowGaugeMood = 3; }
        else { yellowGaugeMood = 2; }

        if (proteinsValue < thresholds_redGauge[0] || proteinsValue > thresholds_redGauge[3])           { redGaugeMood = 1; }
        else if (proteinsValue > thresholds_redGauge[1] && proteinsValue < thresholds_redGauge[2])      { redGaugeMood = 3; }
        else { redGaugeMood = 2; }

        if (carboValue < thresholds_blueGauge[0] || carboValue > thresholds_blueGauge[3])               { blueGaugeMood = 1; }
        else if (carboValue > thresholds_blueGauge[1] && carboValue < thresholds_blueGauge[2])          { blueGaugeMood = 3; }
        else { blueGaugeMood = 2; }
    }

    void ShowGauges()
    {
        yellowGaugeSlider.value = lipidsValue;
        redGaugeSlider.value = proteinsValue;
        blueGaugeSlider.value = carboValue;

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
        if (gaugeType == "yellowGauge")     { yellowGaugeDescentPace = dp; }
        else if (gaugeType == "redGauge")   { redGaugeDescentPace = dp; }
        else if (gaugeType == "blueGauge")  { blueGaugeDescentPace = dp; }
    }

    void descentGaugesProcess()
    {
        lipidsValue -= yellowGaugeDescentPace * Time.deltaTime;
        proteinsValue -= redGaugeDescentPace * Time.deltaTime;
        carboValue -= blueGaugeDescentPace * Time.deltaTime;

        lipidsValue = Math.Max(0f, lipidsValue);
        proteinsValue = Math.Max(0f, proteinsValue);
        carboValue = Math.Max(0f, carboValue);

        SetGauges(lipidsValue, proteinsValue, carboValue);
        Debug.Log("descending ------> lipidsValue :" + lipidsValue + "      proteinsValue :" + proteinsValue + "     carboValue :" + carboValue);
    }

}
