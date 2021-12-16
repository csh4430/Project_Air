using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public struct ShopItems
    {
        public string skinName;
        public Image skinImage;
        public bool hasSkin;
        public Animator skinAnimator;
    }

    public List<ShopItems> shopItems = new List<ShopItems>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
