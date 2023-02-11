using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugesManager : MonoBehaviour
{

    // Current values of gauges
    private int lipidsValue;
    private int proteinsValue;
    private int carboValue;

    // Defining critical gauge thresholds : [lower, equilibriumMin, equilibriumMax, upper]
    private int[] thresholds_yellowGauge = {10, 40, 60, 90};
    private int[] thresholds_redGauge = {10, 40, 60, 90};
    private int[] thresholds_blueGauge = {10, 40, 60, 90};

    // Slider to manipulate the gauges' nutritional levels
    public Slider yellowGaugeSlider;
    public Slider redGaugeSlider;
    public Slider blueGaugeSlider;

    public Image yellowGaugeImage;
    public Image redGaugeImage;
    public Image blueGaugeImage;

    public Sprite yellowSadGauges, redSadGauges, blueSadGauges;
    public Sprite yellowNormalGauges, redNormalGauges, blueNormalGauges;
    public Sprite yellowHappyGauges, redHappyGauges, blueHappyGauges;


    // Start is called before the first frame update
    void Start()
    {
        lipidsValue = 20;
        proteinsValue = 20;
        carboValue = 20;
        showGauges();
    }

    private void Update()
    {
        setGauges(10, 55, 95);
    }

    void setGauges(int lipidsV, int proteinsV, int carboV)
    {
        lipidsValue = lipidsV;
        proteinsValue = proteinsV;
        carboValue = carboV;
        showGauges();
    }

    void showGauges()
    {
        showGaugesMood();
        yellowGaugeSlider.value = lipidsValue;
        redGaugeSlider.value = proteinsValue;
        blueGaugeSlider.value = carboValue;
    }

    void showGaugesMood()
    {

        if (lipidsValue < thresholds_yellowGauge[0] || lipidsValue > thresholds_yellowGauge[3]) {
            yellowGaugeImage.sprite = yellowSadGauges;
        } 
        else if (lipidsValue > thresholds_yellowGauge[1] && lipidsValue < thresholds_yellowGauge[2])
        {
            yellowGaugeImage.sprite = yellowHappyGauges;
        } 
        else
        {
            yellowGaugeImage.sprite = yellowNormalGauges;
        }



        if (proteinsValue < thresholds_redGauge[0] || proteinsValue > thresholds_redGauge[3])
        {
            redGaugeImage.sprite = redSadGauges;
        }
        else if (proteinsValue > thresholds_redGauge[1] && proteinsValue < thresholds_redGauge[2])
        {
            redGaugeImage.sprite = redHappyGauges;
        }
        else
        {
            redGaugeImage.sprite = redNormalGauges;
        }



        if (carboValue < thresholds_blueGauge[0] || carboValue > thresholds_blueGauge[3])
        {
            blueGaugeImage.sprite = blueSadGauges;
        }
        else if (carboValue > thresholds_blueGauge[1] && carboValue < thresholds_blueGauge[2])
        {
            blueGaugeImage.sprite = blueHappyGauges;
        }
        else
        {
            blueGaugeImage.sprite = blueNormalGauges;
        }

    }

}
