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
        //public AnimatorOverrideController skinAnimator_Blue; // 에니매이션 추가되면 함
        //public AnimatorOverrideController skinAnimator_Green;
        //public AnimatorOverrideController skinAnimator_Pink;
        //public AnimatorOverrideController skinAnimator_Purple;
        //public AnimatorOverrideController skinAnimator_Red;
        public AnimatorOverrideController[] skinAnimator;
    }

    public List<ShopItems> shopItems = new List<ShopItems>();

    private List<GameObject> shopItemGameObject = new List<GameObject>();

    public GameObject contentPosition = null;

    [Tooltip("파랑, 초록, 핑크, 보라, 빨강 순으로 넣어야 함")]
    public List<GameObject> units = new List<GameObject>();

    void Start()
    {
        CreateSkinContent();
    }

    void CreateSkinContent() // 상점에 content 추가하기
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

    void UnitAnimatorChange(int i) // 유닛 스킨 끼는거
    {
        for (int j = 0; j < units.Count; j++)
        {
            units[i].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator[j];
        }
    }

    public void ListContentChange() // 각각의 내용물안에 내용 넣기
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

    void Reset() // 안쓸 것 같은데 혹시 안에 있는 내용물 없에 버릴거면 이 함수쓰세요
    {
        for(int i = 0; i < shopItemGameObject.Count; i++)
            ObjectPool.Instance.ReturnObject(PoolObjectType.Content, shopItemGameObject[i]);

        shopItemGameObject.Clear();
    }
}
