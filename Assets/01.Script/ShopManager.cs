using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoSingleton<ShopManager>
{
    [System.Serializable]
    private struct ShopItem
    {
        public Sprite skinImage;
        public string skinName;
        public bool hasKey;
        public bool hasSkin;
    }

    [SerializeField]
    private List<ShopItem> shopItems = new List<ShopItem>();
    [SerializeField]
    private GameObject content = null;
    [SerializeField, Tooltip("Content In The Contents")]
    private GameObject nakami = null;

    void Start()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            GameObject content = Instantiate(nakami, Vector2.zero, Quaternion.identity, this.content.transform);
            content.GetComponent<ShopItems>().ConnectBuyButton(content.GetComponent<ShopItem>().hasSkin);
        }
    }
}

