using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<Units> unitList = new List<Units>();
    private List<Vector2> usedVector = new List<Vector2>();
    [SerializeField] private GameObject gameOverPanel = null;
    private bool isPicked = false;
    private bool isThrew = false;
    private bool isChecking = false;
    private bool isCleared = false;
    private bool isProcessing = true;
    private int stageHad = 0;
    private int pickedUnitsCnt = 0;
    private int unitHave = 0;
    private int years = 0;
    public int mode = 0;

    private void Update()
    {
        if (isProcessing)
        {
            CheckClick();
        }
        if (TimeManager.Instance.CheckTimer())
        {
            GameOver();
        }
    }

    private void SetRandomPosition()
    {
        int horizon = (int)(Camera.main.orthographicSize * Camera.main.aspect);
        int vertical = (int)(Camera.main.orthographicSize / 2);
        StartCoroutine(GetRandomPosition(-horizon, horizon, -vertical, vertical));
    }
    public IEnumerator GetRandomPosition(int minX, int maxX, int minY, int maxY)
    {
        Debug.Log(unitList.Count);
        int valX;
        int valY;
        SetAllUnit(false);
        for (int i = 0; i < unitList.Count; i++)
        {
            do
            {
                yield return new WaitForEndOfFrame();
                valX = Random.Range(minX, maxX);
                valY = Random.Range(minY, maxY);
            } while (usedVector.Contains(new Vector2(valX, valY)));
            usedVector.Add(new Vector2(valX, valY));

            unitList[i].transform.parent.position = new Vector2(valX, valY);
            Debug.Log(i);
        }

        SetAllUnit(true);

        for(int i = 0; i < unitList.Count; i++)
        {
            unitList[i].transform.DOMove(new Vector2(0, -6), 1).From();
        }
        usedVector.Clear();
    }

    private void SetAllUnit(bool isON)
    {
        foreach (var un in unitList)
        {
            un.gameObject.SetActive(isON);
        }
    }

    public void SetGame() //단계 넘어갈때
    {
        mode++;
        stageHad++;
        if(mode >= 6)
        {
            mode = 1;
        }
        unitHave = 0;
        pickedUnitsCnt = 0;
        isPicked = false;
        isThrew = false;
        SetRandomPosition();
        ResetUnits();
        TimeManager.Instance.SetTimer(1);
        TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);
        foreach(var un in unitList)
        {
            un.SetFloat(false);
            un.SetPick(false);
        }
    }

    public void ResetGame() //초기화
    {
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
        SetRandomPosition();
        ResetUnits();
        TimeManager.Instance.SetTimer(0);
        TimeManager.Instance.SetTimer(1);
        UIManager.Instance.SetYearText(years);
        foreach (var un in unitList)
        {
            un.SetFloat(false);
            un.SetPick(false);
        }
    }

    private void ResetUnits()
    {
        foreach(var un in unitList)
        {
            un.transform.parent.gameObject.SetActive(true);
        }
        UIManager.Instance.ResetList();
    }

    private void GameOver()
    {
        isProcessing = false;
        gameOverPanel.SetActive(true);
        TimeManager.Instance.SetTimer(0, 1);
        TimeManager.Instance.SetTimer(-1);
    }

    public void FallUnit()
    {
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

    public void CheckUnitInMode5()
    {
        if (mode != 5) return;
        isThrew = false;
        SetGame();
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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

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
                                TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);

                                un.transform.parent.gameObject.SetActive(false);
                                UIManager.Instance.SetYearText(++years);
                                UIManager.Instance.GetUnits(++unitHave);
                                if(unitHave >= 5)
                                {
                                    SetGame();
                                }
                                return;
                            }
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
                                    un.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                                    //un.transform.DOLocalRotate(new Vector3(0, 0, 180), 2, RotateMode.Fast).OnComplete(() => un.transform.DOLocalRotate(new Vector3(0, 0, 0), 0));
                                }
                                else
                                {
                                    if (!isChecking)
                                    {
                                        isChecking = true;
                                        CheckUnit();
                                    }
                                }
                            }
                            else
                            {
                                if (isThrew && !isChecking)
                                {
                                    TimeManager.Instance.SetTimer(0, stageHad * 0.05f + 3);

                                    un.transform.parent.gameObject.SetActive(false);
                                    pickedUnitsCnt++;
                                    UIManager.Instance.GetUnits(++unitHave);
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
        if (isThrew) return; 
        isThrew = true;
        foreach (var un in unitList)
        {
            un.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            //un.transform.DOLocalRotate(new Vector3(0, 0, 180), 2, RotateMode.Fast).OnComplete(() => un.transform.DOLocalRotate(new Vector3(0, 0, 0), 0));
            un.SetFloat(true);
        }
    }
}
