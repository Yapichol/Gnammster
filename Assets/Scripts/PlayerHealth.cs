using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider yellowGaugeSlider;
    public Slider redGaugeSlider;
    public Slider blueGaugeSlider;

    public EndGame endGame;

    private bool isHurt;
    public float hurtRestoreTime;

    // Start is called before the first frame update
    void Start()
    {
        isHurt = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(yellowGaugeSlider != null && yellowGaugeSlider.value <= 0)
        {
            endGame.endinGame();
        }
        else if (redGaugeSlider != null && redGaugeSlider.value <= 0)
        {
            endGame.endinGame();
        }
        else if (blueGaugeSlider != null && blueGaugeSlider.value <= 0)
        {
            endGame.endinGame();
        }
    }



    public void getHurt()
    {
        Debug.Log("Hurt !");
        if (isHurt)
        {
            if (endGame != null)
            {
                endGame.endinGame();
            }
            return;
        }
        isHurt = true;
        StartCoroutine(healingPorcess(hurtRestoreTime));
    }



    private IEnumerator healingPorcess(float healTime)
    {
        float timer = 0f;
        while(timer < healTime)
        {
            timer = timer + Time.deltaTime;
            yield return null;
        }
        isHurt = false;
        Debug.Log("Healed");
        yield return null;
    }
}
