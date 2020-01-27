using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum MoveDirection
{
    Left,
    Right,
    Up,
    Down
}

public class GamePlayManager : Singleton<GamePlayManager>{

    Cell[,] allCells = new Cell[4,4];
    List<Cell> emptyCells = new List<Cell>();
    List<Cell[]> cellRows = new List<Cell[]>();
    List<Cell[]> cellCols = new List<Cell[]>();
    ScoreManager scoreManager;
    public Text gameOverScore;
    public Text wonScore;
    public GameObject gameOverPanel;
    public GameObject wonPanel;
    int delay = 0;
    bool[] moveCompleteFlag = new bool[4] { true, true, true, true };

	public UDPReceiver reciever;
	private float x, y, z;

	private DateTime time;

    void Start()
    {
        scoreManager = ScoreManager.Instance();
        InitCell();
        InitCellRowAndCols();
        AddNewCell();

		reciever = GameObject.Find("UDPReceiver").GetComponent<UDPReceiver>();
		time = DateTime.Now;
    }

    void Update()
    {

		if(Input.GetKeyDown (KeyCode.Escape))
			Application.Quit ();

		try
		{
			string[] coords = reciever.result.Split(',');
			x = float.Parse(coords[0]);
			z = float.Parse(coords[1]);
		}
		catch (Exception err)
		{
			Debug.Log(err.ToString());
		}

		if (z < -2 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			Move (MoveDirection.Left);
		} else if (z > 2 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			Move (MoveDirection.Right);
		} else if (x > 3 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			Move (MoveDirection.Down);
		} else if (x < -3 && (DateTime.Now - time).Seconds > 1) {
			time = DateTime.Now;
			Move (MoveDirection.Up);

		}

		if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
             Move(MoveDirection.Left);
        }
		if (Input.GetKeyDown(KeyCode.RightArrow))
        {
             Move(MoveDirection.Right);
        }
		if (Input.GetKeyDown(KeyCode.UpArrow))
        {
             Move(MoveDirection.Up);
        }
		if (Input.GetKeyDown(KeyCode.DownArrow))
        {
             Move(MoveDirection.Down);
        }
    }

	IEnumerator Delay(int sec, Action callback){
		yield return new WaitForSeconds (sec);
		callback.Invoke ();
	}

    void InitCell()
    {
        Cell[] cellList = GameObject.FindObjectsOfType<Cell>();
        foreach(Cell c in cellList)
        {
            c.Number = 0;
            allCells[c.row, c.col] = c;
            emptyCells.Add(c);
        }
    }

    void InitCellRowAndCols()
    {
        cellCols.Add(new Cell[] { allCells[0, 0], allCells[1, 0], allCells[2, 0], allCells[3, 0] });
        cellCols.Add(new Cell[] { allCells[0, 1], allCells[1, 1], allCells[2, 1], allCells[3, 1] });
        cellCols.Add(new Cell[] { allCells[0, 2], allCells[1, 2], allCells[2, 2], allCells[3, 2] });
        cellCols.Add(new Cell[] { allCells[0, 3], allCells[1, 3], allCells[2, 3], allCells[3, 3] });

        cellRows.Add(new Cell[] { allCells[0, 0], allCells[0, 1], allCells[0, 2], allCells[0, 3] });
        cellRows.Add(new Cell[] { allCells[1, 0], allCells[1, 1], allCells[1, 2], allCells[1, 3] });
        cellRows.Add(new Cell[] { allCells[2, 0], allCells[2, 1], allCells[2, 2], allCells[2, 3] });
        cellRows.Add(new Cell[] { allCells[3, 0], allCells[3, 1], allCells[3, 2], allCells[3, 3] });
    }

    void AddNewCell()
    {
        if (emptyCells.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, emptyCells.Count);
			int random = UnityEngine.Random.Range(0, 10);
            if (emptyCells[index].Number == 0)
            {
                if (random == 0)
                    emptyCells[index].Number = 4;
                else
                    emptyCells[index].Number = 2;
            }
            emptyCells[index].SetAnimatorAppear();
            emptyCells.RemoveAt(index);
        }
    }

    void ClearEmptyCellList()
    {
        emptyCells.Clear();
        foreach(Cell c in allCells)
        {
            if (c.Number == 0)
            {
                emptyCells.Add(c);
            }
        }
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene("2048");
    }

    bool DownMove(Cell[] lines)
    {
        for(int i = 0; i < lines.Length - 1; i++)
        {
            if (lines[i].Number == 0 && lines[i + 1].Number != 0)
            {
                lines[i].Number = lines[i + 1].Number;
                lines[i + 1].Number = 0;
                return true;
            }

            if (lines[i].Number != 0 && lines[i].Number == lines[i + 1].Number &&
                !lines[i].addNumber && !lines[i + 1].addNumber)
            {
                /*if (lines[i].Number == 8192)
                    WinGame();*/
                lines[i].Number = lines[i].Number * 2;
                lines[i + 1].Number = 0;
                lines[i].addNumber = true;
                lines[i].SetAnimatorMerge();
                emptyCells.Add(lines[i + 1]);
                scoreManager.Score += lines[i].Number;
                return true;
            }
        }
        return false;
    }

    bool UpMove(Cell[] lines)
    {
        for (int i = lines.Length - 1; i > 0; i--)
        {
            if (lines[i].Number == 0 && lines[i - 1].Number != 0)
            {
                lines[i].Number = lines[i - 1].Number;
                lines[i - 1].Number = 0;
                return true;
            }

            if(lines[i].Number!=0&&lines[i].Number==lines[i-1].Number&&
                !lines[i].addNumber && !lines[i - 1].addNumber)
            {
                /*if (lines[i].Number == 8192)
                    WinGame();*/
                lines[i].Number = lines[i].Number * 2;
                lines[i - 1].Number = 0;
                lines[i].addNumber = true;
                lines[i].SetAnimatorMerge();
                emptyCells.Add(lines[i - 1]);
                scoreManager.Score += lines[i].Number;
                return true;
            }
        } 
        return false;
    }
    void GameOver()
    {
        gameOverScore.text = scoreManager.Score.ToString();
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void WinGame()
    {
        wonScore.text = scoreManager.Score.ToString();
        wonPanel.SetActive(true);
        Time.timeScale = 0;
    }

    bool CanMove()
    {
        if (emptyCells.Count > 0)
            return true;
        else
        {
            for(int i = 0; i < cellCols.Count; i++)
            {
                for(int j = 0; j < cellRows.Count - 1; j++)
                {
                    if (allCells[j, i].Number == allCells[j + 1, i].Number)
                        return true;
                }
            }
            for (int i = 0; i < cellRows.Count; i++)
            {
                for (int j = 0; j < cellCols.Count - 1; j++)
                {
                    if (allCells[i, j].Number == allCells[i, j + 1].Number)
                        return true;
                }
            }
        }
        return false;
    }

    public void Move(MoveDirection move)
    {
        foreach (Cell c in allCells)
            c.ResetAddNumberFlag();
        for (int i = 0; i < cellRows.Count; i++)
        {
            switch (move)
            {
                case MoveDirection.Down:
                    while (UpMove(cellCols[i])) { }
                    break;
                case MoveDirection.Left:
                    while (DownMove(cellRows[i])) { }
                    break;
                case MoveDirection.Right:
                    while (UpMove(cellRows[i])) { }
                    break;
                case MoveDirection.Up:
                    while (DownMove(cellCols[i])) { }
                    break;
            }
        }
        ClearEmptyCellList();
        AddNewCell();
        if (!CanMove())
            GameOver();
    }

    /*IEnumerator DownMoveCoroutine(Cell[] c,int delay)
    {
        while (UpMove(c))
        {
            yield return new WaitForSeconds(delay);
        }
    }*/
}
