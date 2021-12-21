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
        public Sprite skinImage;
        public bool hasSkin;
        public int cost;
        //public AnimatorOverrideController skinAnimator_Blue; // ���ϸ��̼� �߰��Ǹ� ��
        //public AnimatorOverrideController skinAnimator_Green;
        //public AnimatorOverrideController skinAnimator_Pink;
        //public AnimatorOverrideController skinAnimator_Purple;
        //public AnimatorOverrideController skinAnimator_Red;
        public AnimatorOverrideController[] skinAnimator;
    }

    public List<ShopItems> shopItems = new List<ShopItems>();

    private List<GameObject> shopItemGameObject = new List<GameObject>();

    public GameObject contentPosition = null;

    [Tooltip("�Ķ�, �ʷ�, ��ũ, ����, ���� ������ �־�� ��")]
    public List<GameObject> units = new List<GameObject>();

    void Start()
    {
        CreateSkinContent();
    }

    void CreateSkinContent() // ������ content �߰��ϱ�
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            GameObject content = ObjectPool.Instance.GetObject(PoolObjectType.Content);
            content.transform.SetParent(contentPosition.transform);
            content.transform.localScale = Vector3.one;

            shopItemGameObject.Add(content);
        }

        ListContentChange();
    }

    void UnitAnimatorChange(int i) // ���� ��Ų ���°�
    {
        for (int j = 0; j < units.Count; j++)
        {
            units[i].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator[j];
        }
    }

    public void ListContentChange() // ������ ���빰�ȿ� ���� �ֱ�
    {
        for(int i = 0; i < shopItemGameObject.Count; i++)
        {
            shopItemGameObject[i].transform.GetChild(0).GetComponent<Image>().sprite = shopItems[i].skinImage;
            shopItemGameObject[i].transform.GetChild(1).GetComponent<Text>().text = shopItems[i].skinName;
            shopItemGameObject[i].transform.GetChild(2).GetComponent<Text>().text = shopItems[i].cost.ToString();
            shopItemGameObject[i].transform.GetChild(3).GetChild(0).GetComponent<Text>().text = shopItems[i].hasSkin ? "USE" : "BUY";
            shopItemGameObject[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => BuyOrUse(i));
        }
    }

    public void BuyOrUse(int i)
    {
        if (shopItems[i].hasSkin)
        {
            UnitAnimatorChange(i);
        }
        else
        {
            if (FileManager.Instance.save.coin < shopItems[i].cost) return;
            Debug.Log(FileManager.Instance.save.coin);
            FileManager.Instance.save.coin -= shopItems[i].cost;
            Debug.Log(FileManager.Instance.save.coin);
            FileManager.Instance.SaveToJson();
            ChangeHas(shopItems[i]);
            UnitAnimatorChange(i);
        }
    }

    void ChangeHas(ShopItems shopItems)
    {
        shopItems.hasSkin = true;
    }

    void Reset() // �Ⱦ� �� ������ Ȥ�� �ȿ� �ִ� ���빰 ���� �����Ÿ� �� �Լ�������
    {
        for(int i = 0; i < shopItemGameObject.Count; i++)
            ObjectPool.Instance.ReturnObject(PoolObjectType.Content, shopItemGameObject[i]);

        shopItemGameObject.Clear();
    }
}
