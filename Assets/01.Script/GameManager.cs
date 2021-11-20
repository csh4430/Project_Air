using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int pickedUnitsCnt = 0;
    private int unitHave = 0;
    private int years = 0;
    public int mode = 1;

    private void Start()
    {
        SetRandomPosition();
    }

    private void Update()
    {
        if (isProcessing)
        {
            CheckClick();
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
        usedVector.Clear();
    }

    public void SetGame() //단계 넘어갈때
    {
        mode++;
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
        pickedUnitsCnt = 0;
        unitHave = 0;
        years = 0;
        mode = 1;
        SetRandomPosition();
        ResetUnits(); 
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
        ResetGame();
        gameOverPanel.SetActive(true);
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
                                    isThrew = true;
                                    un.SetFloat(true);
                                    un.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
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
            un.SetFloat(true);
        }
    }
}
