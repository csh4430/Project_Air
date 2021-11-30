using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private List<GameObject> units = new List<GameObject>();
    [SerializeField] private Text yearsCntText = null;
    [SerializeField] private Text highestYearsCntText = null;
    [SerializeField] private Slider timeLimitSlider = null;
    [SerializeField] private GameObject uiCanvasObject = null;
    public GameObject _UICANVAS { get { return uiCanvasObject; } }

    public List<GameObject> Units
    { 
        get
        {
            return units;
        }
    }

    public void GetUnits(int num, int cnt)
    {
        units[num].transform.SetSiblingIndex(cnt);
        units[num].SetActive(true);
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

    public void SetHighestYearText(int years)
    {
        highestYearsCntText.text = string.Format("BEST {0}", years);
    }

    public void SetSliderValue(float val)
    {
        timeLimitSlider.value = val;
    }
}
