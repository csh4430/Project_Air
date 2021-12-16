using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    private float diffTime = 1;
    private bool timeMove = false;
    private float sec = 3f;

    void Update()
    {
        if(timeMove && GameManager.Instance.isProcessing)
            UIManager.Instance.SetSliderValue(diffTime -= Time.deltaTime / sec);
    }

    public void SetTimer(int num, float second = 3f)
    {
        switch (num)
        {
            case 1:
                timeMove = true;
                break;
            case 0:
                diffTime = 1f;
                sec = second;
                break;
            case -1:
                timeMove = false;
                break;
            default:
                Debug.LogError("Error On SetTimer");
                break;
        }
    }

    public bool CheckTimer()
    {
        return diffTime <= 0;
    }
}
