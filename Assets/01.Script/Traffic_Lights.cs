using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic_Lights : MonoBehaviour
{
    private bool isGreen = false;
    private IEnumerator ChangeLights()
    {
        while (true)
        {
            isGreen = false;
            yield return new WaitForSeconds(10f);
            isGreen = true;
            yield return new WaitForSeconds(5);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGreen)
        {

        }
    }
}
