using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<GameObject> units = new List<GameObject>();
    [SerializeField] private List<Units> unitList = new List<Units>();
    private List<int> usedValX = new List<int>();
    private List<int> usedValY = new List<int>();
    public List<GameObject> Units { get { return units; } }

    private bool isPicked = false;
    private bool isThrew = false;
    private int pickedUnitsCnt = 0;
    private int unitHave = 0;
    private int mode = 1;

    private void Start()
    {
        SetRandomPosition();
    }

    private void Update()
    {
        CheckClick();
    }
    private void SetRandomPosition()
    {
        foreach (var un in units)
        {
            un.transform.position = GetRandomPosition((int)-(Camera.main.orthographicSize * Camera.main.aspect), (int)(Camera.main.orthographicSize * Camera.main.aspect), (int)-Camera.main.orthographicSize / 2, (int)Camera.main.orthographicSize / 2);
        }
    }
    public Vector2 GetRandomPosition(int minX, int maxX, int minY, int maxY)
    {
        int valX = Random.Range(minX, maxX+1);
        while (usedValX.Contains(valX))
        {
            valX = Random.Range(minX, maxX+1);
        }

        int valY = Random.Range(minY, maxY+1);
        while (usedValY.Contains(valY))
        {
            valY = Random.Range(minY, maxY+1);
        }

        Debug.Log(valX + " " + valY);
        return new Vector2(valX * 0.8f, valY * 0.8f);
    }

    public void ResetGame()
    {
        unitHave = 0;
        pickedUnitsCnt = 0;
        foreach(var un in units)
        {
            un.SetActive(true);
        }
    }

    public void CheckClear()
    {
        isThrew = false;

        if (mode == 3)
        {
            if(pickedUnitsCnt == 1 && unitHave == 4)
            {
                pickedUnitsCnt = 3;
            }
        }

        if (mode != pickedUnitsCnt)
        {

            ResetGame();
            UIManager.Instance.ResetList();
            return;
        }
        pickedUnitsCnt = 0;
        if(unitHave >= 4)
        {
            mode++;
            isPicked = false;
            isThrew = false;
            SetRandomPosition();
            ResetGame();
            UIManager.Instance.ResetList(); 
            foreach(var un in unitList)
            {
                un.SetPick(false);
            }
        }
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                if(!isPicked)
                {
                    hit.transform.position = new Vector2(0, -4);
                    unitList[units.IndexOf(hit.collider.gameObject)].SetPick(true);
                    isPicked = true;
                    return;
                }

                foreach (var u in units)
                {
                    if (string.Compare(u.name, hit.collider.name) == 0)
                    {
                        Units un = u.GetComponent<Units>();
                        Rigidbody2D urb = u.GetComponent<Rigidbody2D>();
                        if (un.isPicked)
                        {
                            if (un.isFloating)
                            {
                                return;
                            }
                            urb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
                            un.isFloating = true;
                            isThrew = true;
                            return;
                        }
                        if (isThrew)
                        {
                            u.SetActive(false);
                            pickedUnitsCnt++;
                            UIManager.Instance.GetUnits(++unitHave);
                            return;
                        }
                    }
                }
            }
        }
    }
}
