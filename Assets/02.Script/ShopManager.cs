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
        public AnimatorOverrideController skinAnimator_Blue;
        public AnimatorOverrideController skinAnimator_Green;
        public AnimatorOverrideController skinAnimator_Pink;
        public AnimatorOverrideController skinAnimator_Purple;
        public AnimatorOverrideController skinAnimator_Red;
    }

    public List<ShopItems> shopItems = new List<ShopItems>();

    private List<GameObject> shopItemGameObject = new List<GameObject>();

    public GameObject contentPosition = null;

    [Tooltip("파랑, 초록, 핑크, 보라, 빨강 순으로 넣어야 함")]
    public List<GameObject> units = new List<GameObject>();

    void Start()
    {
        CreateSkinList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateSkinList()
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

    void UnitAnimatorChange()
    {
        for (int i = 0; i < shopItemGameObject.Count; i++)
        {
            units[0].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator_Blue.runtimeAnimatorController;
            units[1].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator_Green.runtimeAnimatorController;
            units[2].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator_Pink.runtimeAnimatorController;
            units[3].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator_Purple.runtimeAnimatorController;
            units[4].GetComponent<Animator>().runtimeAnimatorController = shopItems[i].skinAnimator_Red.runtimeAnimatorController;
        }
    }


    void ListContentChange()
    {
        for(int i = 0; i < shopItemGameObject.Count; i++)
        {
            shopItemGameObject[i].transform.GetChild(0).GetComponent<Image>().sprite = shopItems[i].skinImage;
            shopItemGameObject[i].transform.GetChild(1).GetComponent<Text>().text = shopItems[i].skinName;
            shopItemGameObject[i].transform.GetChild(2).GetComponent<Text>().text = shopItems[i].cost.ToString();
            shopItemGameObject[i].transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => UnitAnimatorChange());
        }
    }

    void Reset()
    {
        for(int i = 0; i < shopItemGameObject.Count; i++)
        {
            ObjectPool.Instance.ReturnObject(PoolObjectType.Content, shopItemGameObject[i]);
        }

        shopItemGameObject.Clear();
    }
}
