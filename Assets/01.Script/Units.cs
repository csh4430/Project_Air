using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public int num;
    public bool isPicked;
    public bool isFloating;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPicked)
        {
            isFloating = false;
            GameManager.Instance.FallUnit();
        }
        if (isFloating)
        {
            GameManager.Instance.CheckUnitInMode5();
        }
    }

    public void SetPick(bool picked)
    {
        isPicked = picked;
    }
    public void SetFloat(bool floated)
    {
        isFloating = floated;
    }
}