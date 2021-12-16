using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class StartGame : MonoBehaviour
{
    [SerializeField] private Image logoImage = null;

    private void Start()
    {
        logoImage.DOColor(Color.white, 2);
        logoImage.transform.DOScale(Vector2.one * 0.8f, 2).OnComplete(()=> { SceneManager.LoadScene(1); });
    }
}
