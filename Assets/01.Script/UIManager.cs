using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private List<GameObject> units = new List<GameObject>();
    [SerializeField] private Text yearsCntText = null;
    [SerializeField] private Slider timeLimitSlider = null;

    public List<GameObject> Units
    { 
        get
        {
            return units;
        }
    }

    public void GetUnits(int cnt)
    {
        for (int a = 0; a < cnt; a++)
        {
            units[a].SetActive(true);
        }
    }

    public void ResetList()
    {
        foreach(var un in units)
        {
            un.SetActive(false);
        }
    }

    public void SetYearText(int years)
    {
        yearsCntText.text = years.ToString();
    }

    public void SetSliderValue(float val)
    {
        timeLimitSlider.value = val;
    }
}
