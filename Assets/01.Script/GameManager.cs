using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<float> X = new List<float>();
    public List<float> Xs { get { return X; } }

    [SerializeField] private PlayerBase playerBase = null;

    public int gameMode { get; private set; }

    public PlayerBase PlayerBase
    {
        get
        {
            return playerBase;
        }
    }

    public void SetGameMode(int mode)
    {
        gameMode = mode;
    }
}
