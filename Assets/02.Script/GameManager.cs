using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<Units> unitList = new List<Units>();
    public List<Units> Units { get { return unitList; } }
    private List<Vector2> usedVector = new List<Vector2>();
    [SerializeField] private GameObject gameOverPanel = null;
    [SerializeField] private GameObject gameStartPanel = null;
    [SerializeField] private Button startBtn = null;

    private AudioSource audio = null;
    private bool isPicked = false;
    private bool isThrew = false;
    private bool isChecking = false;
    private bool isCleared = false;
    private bool isSpread = false;
    public bool isProcessing { get; private set; }
    private int stageHad = 0;
    private int pickedUnitsCnt = 0;
    private int unitHave = 0;
    private int years = 0;
    public int mode = 0;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        Application.targetFrameRate = 30;

        isProcessing = true;
    }

    private void Start()
    {
        startBtn.onClick.AddListener(() =>
        {

            Time.timeScale = 1;
            isProcessing = true;
            SoundManager.Instance.SetSoundClip(audio, "UI", "BB");
            audio.Play();
            SetGame();
        });
    }

    private void Update()
    {
        if (isProcessing)
        {
            CheckClick();
        }
        if (TimeManager.Instance.CheckTimer() && !isCleared)
        {
            GameOver();
        }
    }

    public void PauseGame(bool isOn)
    {
        isProcessing = !isOn;
    }

    private void DoTutorial()
    {
        isProcessing = false;
        FileManager.Instance.save.hadTutorial = true;
        FileManager.Instance.SaveToJson();
        UIManager.Instance.SetMaskActive(true);
    }

    public void StopTutorial()
    {
        isProcessing = true;
        UIManager.Instance.SetMaskActive(false);
    }

    private void SetRandomPosition()
    {
        int horizon = (int)(Camera.main.orthographicSize * Camera.main.aspect);
        int vertical = (int)(Camera.main.orthographicSize / 2);
        StartCoroutine(GetRandomPosition(-horizon, horizon, -vertical, vertical));
    }

    public IEnumerator GetRandomPosition(int minX, int maxX, int minY, int maxY)
    {
        SetAllUnit(false);
        Debug.Log(unitList.Count);
        int valX;
        int valY;
        for (int i = 0; i < unitList.Count; i++)
        {
            do
            {
                yield return new WaitForEndOfFrame();
                valX = Random.Range(minX, maxX);
                valY = Random.Range(minY, maxY);
            } while (usedVector.Contains(new Vector3(valX, valY, valY)));
            usedVector.Add(new Vector2(valX, valY));

            unitList[i].transform.parent.position = new Vector3(valX + 0.5f, valY, valY);
            Debug.Log(i);
        }
        SetAllUnit(true);
        audio.PlayOneShot(SoundManager.Instance.FindClip("Units", "Roll"));

        for (int i = 0; i < unitList.Count; i++)
        {
            unitList[i].transform.parent.DOMove(new Vector2(4, 0), 1).From().OnComplete(()=>
            {
                if (!FileManager.Instance.save.hadTutorial)
                    DoTutorial();
                isSpread = true;
            });
        }

        usedVector.Clear();
    }

    public void SetAllUnit(bool isON)
    {
        foreach (var un in unitList)
        {
            un.transform.parent.gameObject.SetActive(isON);
            un.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            un.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(1f, 5), ForceMode2D.Impulse);
        }
    }

    public void SetGame() //단계 넘어갈때
    {
        if (!isProcessing) return;
        isSpread = false;
        SetAllUnit(false);
        stageHad++;
        mode++;
        if (mode >= 6)
        {
            mode = 1;
        }
        UIManager.Instance.StageShowImageChange(mode);
        unitHave = 0;
        pickedUnitsCnt = 0;
        isPicked = false;
        isThrew = false;
        SetRandomPosition();
        UIManager.Instance.SetHighestYearText(FileManager.Instance.save.highestYear);
        UIManager.Instance.ResetList();
        TimeManager.Instance.SetTimer(1);
        TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);
        foreach (var un in unitList)
        {
            un.SetFloat(false);
            un.SetPick(false);
        }
    }

    public void ResetGame() //초기화
    {
        isSpread = false;
        isPicked = false;
        isThrew = false;
        isChecking = false;
        isCleared = false;
        isProcessing = true;
        stageHad = 0;
        pickedUnitsCnt = 0;
        unitHave = 0;
        years = 0;
        mode = 1;
        UIManager.Instance.StageShowImageChange(1);
        SetRandomPosition();
        UIManager.Instance.ResetList();
        TimeManager.Instance.SetTimer(0);
        TimeManager.Instance.SetTimer(1);
        UIManager.Instance.SetYearText(years);
        foreach (var un in unitList)
        {
            un.transform.position = new Vector3(un.transform.position.x, un.transform.position.y, -1);
            un.SetFloat(false);
            un.SetPick(false);
        }
    }

    private void GameOver()
    {
        UIManager.Instance.SetEndText(years, FileManager.Instance.save.highestYear);
        isProcessing = false;
        SetAllUnit(false);
        mode = 0;
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.DOMove(new Vector2(0, -6), .5f).From();
        FileManager.Instance.SaveToJson();
        UIManager.Instance._UICANVAS.SetActive(false);
        TimeManager.Instance.SetTimer(0, 1);
        TimeManager.Instance.SetTimer(-1);
    }

    public void Home()
    {
        isProcessing = true;
        gameStartPanel.SetActive(true);
        gameStartPanel.transform.DOMove(new Vector2(0, -6), .5f).From();
    }

    public void FallUnit()
    {
        if (!isSpread) return;
        if (!isProcessing) return;
        isThrew = false;
        isChecking = false;
        if (!isCleared)
        {
            GameOver();
        }
        else
        {
            if(unitHave == 4)
            {
                SetGame();
            }
        }
        isCleared = false;
    }

    private void CheckUnit()
    {
        if(mode == 3)
        {
            if(unitHave == 4)
            {
                pickedUnitsCnt = 3;
            }
        }
        if(pickedUnitsCnt == mode)
        {
            pickedUnitsCnt = 0;
            isCleared = true;
        }
        else
        {
            isCleared = false;
        }
    }

    private void CheckClick()
    {
        if (!isSpread) return;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (isProcessing == false) return;

            if (hit.collider != null)
            {
                Debug.Log(hit.transform.name);
                foreach (var un in unitList)
                {
                    if(string.Compare(un.name, hit.transform.name) == 0)
                    {
                        if(mode == 5)
                        {
                            if (isThrew)
                            {
                                audio.PlayOneShot(SoundManager.Instance.FindClip("Units", "Catch"));

                                TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);
                                un.transform.parent.gameObject.SetActive(false);
                                UIManager.Instance.SetYearText(++years);
                                if(years > FileManager.Instance.save.highestYear)
                                {
                                    FileManager.Instance.save.highestYear = years;
                                    Debug.Log(FileManager.Instance.save.highestYear);
                                    UIManager.Instance.SetHighestYearText(FileManager.Instance.save.highestYear);
                                }
                                UIManager.Instance.GetUnits(unitList.IndexOf(un), ++unitHave);
                                if(unitHave >= 5)
                                {
                                    if (!isProcessing) return;
                                    SetGame();
                                }
                                return;
                            }
                            return;
                        }

                        if (!isPicked)
                        {
                            un.SetPick(true);
                            isPicked = true;
                        }
                        else
                        {
                            if (un.isPicked)
                            {
                                if (!isThrew)
                                {
                                    TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);

                                    isThrew = true;
                                    un.SetFloat(true);
                                }
                                else
                                {
                                    if (!isChecking)
                                    {
                                        isChecking = true;
                                        un.GetComponent<SpriteRenderer>().color = Color.white * 0.5f;
                                        CheckUnit();
                                    }
                                }
                            }
                            else
                            {

                                if (isThrew && !isChecking)
                                {
                                    audio.PlayOneShot(SoundManager.Instance.FindClip("Units", "Catch"));
                                    TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);
                                    un.transform.parent.gameObject.SetActive(false);
                                    pickedUnitsCnt++;
                                    UIManager.Instance.GetUnits(unitList.IndexOf(un), ++unitHave);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void BounceAll()
    {
        if (mode != 5) return; 
        if (!isSpread) return;
        if (isThrew) return; 
        isThrew = true;
        foreach (var un in unitList)
        {
            un.SetFloat(true);
        }
    }
}