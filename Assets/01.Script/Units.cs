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
        isFloating = false;
        GameManager.Instance.CheckClear();
    }

    public void SetPick(bool picked)
    {
        isPicked = picked;
    }
}
