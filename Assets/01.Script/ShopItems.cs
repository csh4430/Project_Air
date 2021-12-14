using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItems : MonoBehaviour
{
    [SerializeField]
    private Text skinName = null;
    [SerializeField]
    private Image skinImage = null;
    [SerializeReference]
    private Button buyButon = null;
    [SerializeField]
    private Text buyText = null;

    public void SetSkinName(string name)
    {
        skinName.text = name;
    }

    public void SetSkinImage(Sprite image)
    {
        skinImage.sprite = image;
    }

    public void ConnectBuyButton(bool isHas)
    {
        if (!isHas)
        {
            buyText.text = "BUY";
            //ShopManager.Instance.
        }
        else if(isHas)
        {
            buyText.text = "USE";
        }
        buyButon.onClick.AddListener(() => BuyAndUse(isHas));
    }

    void BuyAndUse(bool isHas)
    {
        if (isHas)
        {
            // ���ϸ����� ����(��Ų ����)
        }
        else if (!isHas)
        {
            // ��Ų ����
            // ���ϸ����� ����(��ƾ����)
            // ���
        }
    }

    
}
