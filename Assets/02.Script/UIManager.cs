using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private List<GameObject> units = new List<GameObject>();
    [SerializeField] private Text yearsCntText = null;
    [SerializeField] private Text highestYearsCntText = null;
    [SerializeField] private Text tutorialText = null;
    [SerializeField] private Slider timeLimitSlider = null;
    [SerializeField] private GameObject uiCanvasObject = null;
    [SerializeField] private RectTransform maskRect = null;
    [SerializeField] private Image stageShowImage = null;
    [SerializeField] private Sprite[] stageShowImages = null;
    [SerializeField] private Sprite[] soundMuteImage = null;
    [SerializeField] private Button soundMuteButton = null;
    [SerializeField] private Button BounceAllButton = null;
    [SerializeField] private Text[] EndYearText = new Text[2];
    
    public GameObject _UICANVAS { get { return uiCanvasObject; } }

    public List<GameObject> Units
    { 
        get
        {
            return units;
        }
    }


    public void Awake()
    {
        BounceAllButton.onClick.AddListener(GameManager.Instance.BounceAll);
    }

    public void GetUnits(int num, int cnt)
    {
        units[num].transform.SetSiblingIndex(cnt);
        units[num].SetActive(true);
    }

    public void ResetList()
    {
        foreach(var un in units)
        {
            un.SetActive(false);
        }
    }

    public void SetYearText(int years)
    {
        yearsCntText.text = years.ToString();
    }

    public void SetHighestYearText(int years)
    {
        highestYearsCntText.text = string.Format("BEST {0}", years);
    }

    public void SetSliderValue(float val)
    {
        timeLimitSlider.value = val;
    }

    public void SetMaskPos(Vector2 pos)
    {
        maskRect.position = pos;
        maskRect.transform.GetChild(0).transform.position = Vector2.zero;
        SetTutorialText(pos + Vector2.up * 0.6f);
    }

    public void SetMaskActive(bool isOn)
    {
        maskRect.transform.parent.gameObject.SetActive(isOn);
        if(isOn)
            SetMaskPos(GameManager.Instance.Units[0].transform.position);
    }

    public void SetTutorialText(Vector2 pos)
    {
        tutorialText.transform.position = pos;
        StartCoroutine(TypeText(tutorialText, "공기놀이는 해 봤겠지만", () => {
            StartCoroutine(TypeText(tutorialText, "내가 설명해줄게!", () => {
                StartCoroutine(TypeText(tutorialText, "일단 공기를 한번 눌러 선택하고", () => {
                    StartCoroutine(TypeText(tutorialText, "한 번 더 눌러서, 공기를 띄워!", () => {
                        StartCoroutine(TypeText(tutorialText, "그다음, 단계에 따라 공기를 주워!", () => {
                            StartCoroutine(TypeText(tutorialText, "쉽지? 그럼 힘내!", GameManager.Instance.StopTutorial));
                        }));
                    }));
                }));
            }));
        }));
        
    }

    private IEnumerator TypeText(Text textBox, string text, Action callBack)
    {
        textBox.text = "";
        for(int i = 0; i < text.Length; i++)
        {
            textBox.text = string.Concat(textBox.text, text[i].ToString());
            Debug.Log(i);   
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        callBack();
    }

    public void SetSetting(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        Time.timeScale = !gameObject.activeInHierarchy ? 1 : 0;
        GameManager.Instance.PauseGame(gameObject.activeInHierarchy);
    }

    public void Show(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
    public void StageShowImageChange(int stage)
    {
        stageShowImage.sprite = stageShowImages[stage - 1];
    }

    public void SoundMuteMuttonImageChange(bool isMute)
    {
        if (isMute)
            soundMuteButton.GetComponent<Image>().sprite = soundMuteImage[0];
        else
            soundMuteButton.GetComponent<Image>().sprite = soundMuteImage[1];
    }

    public void SetEndText(int year, int highestYear)
    {
        EndYearText[0].text = string.Format("년수: {0}", year);
        EndYearText[1].text = string.Format("최고 년수: {0}", highestYear);
    }
}
