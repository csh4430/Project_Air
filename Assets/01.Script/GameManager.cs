using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<float> X = new List<float>();
    public List<float> Xs { get { return X; } }
}