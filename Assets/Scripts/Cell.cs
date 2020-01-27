using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour {

    [HideInInspector]
    public bool addNumber = false;
    public int row,col;
    int number;
    Text cellText;
    Image cellBackground;
    CellStyleHolder cellStyleHolder;
    Animator anim;

    public int Number
    {
        get
        {
            return number;
        }
        set
        {
            number = value;
            if (number == 0)
            {
                SetCellHide();
            }
            else
            {
                ChangeCellStyle(number);
                SetCellVisual();
            }
        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cellBackground = transform.GetChild(0).GetComponent<Image>();
        cellText = transform.GetChild(1).GetComponent<Text>();
        cellStyleHolder = CellStyleHolder.Instance();
    }

    public void SetAnimatorAppear()
    {
        anim.SetTrigger("Appear");
    }

    public void SetAnimatorMerge()
    {
        anim.SetTrigger("Merge");
    }

    void GetCellStyle(int index)
    {
        int i = index % 12;
        cellBackground.color = cellStyleHolder.cellStyle[i].cellColor;
        cellText.color = cellStyleHolder.cellStyle[i].textColor;
        cellText.text = cellStyleHolder.cellStyle[i].number.ToString();
        cellText.fontSize = 160 - i / 12 * 20;
    }

    void ChangeCellStyle(int index)
    {
        if (index > 8192)
            index = index / 8192;
        switch (index)
        {
            case 2:
                GetCellStyle(0);
                break;
            case 4:
                GetCellStyle(1);
                break;
            case 8:
                GetCellStyle(2);
                break;
            case 16:
                GetCellStyle(3);
                break;
            case 32:
                GetCellStyle(4);
                break;
            case 64:
                GetCellStyle(5);
                break;
            case 128:
                GetCellStyle(6);
                break;
            case 256:
                GetCellStyle(7);
                break;
            case 512:
                GetCellStyle(8);
                break;
            case 1024:
                GetCellStyle(9);
                break;
            case 2048:
                GetCellStyle(10);
                break;
            case 4096:
                GetCellStyle(11);
                break;
            case 8192:
                GetCellStyle(12);
                break;
            default:
                Debug.LogError("invalid number!");
                break;
        }
    }

    void SetCellVisual()
    {
        cellText.enabled = true;
        cellBackground.enabled = true;
    }

    void SetCellHide()
    {
        cellText.enabled = false;
        cellBackground.enabled = false;
    }

    public void ResetAddNumberFlag()
    {
        addNumber = false;
    }
}
