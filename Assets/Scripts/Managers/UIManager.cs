using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Image> battery = new List<Image>();
    public Gradient emptyToFull;

    public void Start()
    {
        StartBattery();
    }

    public void StartBattery()
    {
        for (int i = 0; i < battery.Count; i++)
        {
            battery[i].color = Color.clear;
            battery[i].gameObject.SetActive(false);
        }
    }

    public void UpdateBatteryUI(int numberActif, int numberMax)
    {
        for (int i = 0; i < numberMax; i++)
        {
            battery[i].color = emptyToFull.Evaluate(1);
            battery[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < numberActif; i++)
        {
            battery[i].color = emptyToFull.Evaluate(0);
        }
    }

    //Coroutine for the battery
}
