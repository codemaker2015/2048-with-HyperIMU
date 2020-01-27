using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellStyle
{
    public int number;
    public int textSize;
    public Color32 cellColor;
    public Color32 textColor;
}

public class CellStyleHolder : Singleton<CellStyleHolder>
{
    public CellStyle[] cellStyle;

    private void Start()
    {
        cellStyle[0].number = 2;
        cellStyle[0].cellColor = new Color32(231, 220, 208, 255);
        cellStyle[0].textColor = new Color32(126, 109, 80, 255);
        cellStyle[0].textSize = 160;

        cellStyle[1].number = 4;
        cellStyle[1].cellColor = new Color32(246, 232, 203, 255);
        cellStyle[1].textColor = new Color32(126, 109, 80, 255);
        cellStyle[1].textSize = 160;

        cellStyle[2].number = 8;
        cellStyle[2].cellColor = new Color32(251, 178, 125, 255);
        cellStyle[2].textColor = new Color32(255, 255, 255, 255);
        cellStyle[2].textSize = 160;

        cellStyle[3].number = 16;
        cellStyle[3].cellColor = new Color32(244, 144, 70, 255);
        cellStyle[3].textColor = new Color32(255, 255, 255, 255);
        cellStyle[3].textSize = 160;

        cellStyle[4].number = 32;
        cellStyle[4].cellColor = new Color32(254, 125, 66, 255);
        cellStyle[4].textColor = new Color32(255, 255, 255, 255);
        cellStyle[4].textSize = 160;

        cellStyle[5].number = 64;
        cellStyle[5].cellColor = new Color32(253, 101, 32, 255);
        cellStyle[5].textColor = new Color32(255, 255, 255, 255);
        cellStyle[5].textSize = 160;

        cellStyle[6].number = 128;
        cellStyle[6].cellColor = new Color32(245, 210, 115, 255);
        cellStyle[6].textColor = new Color32(255, 255, 255, 255);
        cellStyle[6].textSize = 160;

        cellStyle[7].number = 256;
        cellStyle[7].cellColor = new Color32(243, 196, 55, 255);
        cellStyle[7].textColor = new Color32(255, 255, 255, 255);
        cellStyle[7].textSize = 160;

        cellStyle[8].number = 512;
        cellStyle[8].cellColor = new Color32(247, 188, 12, 255);
        cellStyle[8].textColor = new Color32(255, 255, 255, 255);
        cellStyle[8].textSize = 160;

        cellStyle[9].number = 1024;
        cellStyle[9].cellColor = new Color32(244, 102, 116, 255);
        cellStyle[9].textColor = new Color32(255, 255, 255, 255);
        cellStyle[9].textSize = 160;

        cellStyle[10].number = 2048;
        cellStyle[10].cellColor = new Color32(243, 75, 92, 255);
        cellStyle[10].textColor = new Color32(255, 255, 255, 255);
        cellStyle[10].textSize = 160;

        cellStyle[11].number = 4096;
        cellStyle[11].cellColor = new Color32(111, 181, 215, 255);
        cellStyle[11].textColor = new Color32(255, 255, 255, 255);
        cellStyle[11].textSize = 160;

        cellStyle[12].number = 8192;
        cellStyle[12].cellColor = new Color32(0, 127, 194, 255);
        cellStyle[12].textColor = new Color32(255, 255, 255, 255);
        cellStyle[12].textSize = 160;
    }
}
