using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStyleHolder : MonoBehaviour
{
    [System.Serializable]
    public class TileStyle
    {
        public string Name;
        public int Number;
        public Color32 TileColor;
        public Color32 TextColor;
    }

    public TileStyle[] TileStyles;

    private void OnValidate()
    {
        for (int i = 0; i < TileStyles.Length; i++)
        {
            TileStyles[i].Name = TileStyles[i].Number.ToString();
            if (TileStyles[i].TileColor.a == 0) { TileStyles[i].TileColor = new Color32(255, 255, 255, 255); }
            if (TileStyles[i].TextColor.a == 0) { TileStyles[i].TextColor = new Color32(255, 255, 255, 255); }
        }
    }

    public int GetIndexOfNumber(int number)
    {
        for (int i = 0; i < TileStyles.Length; i++)
        {
            if(TileStyles[i].Number == number)
            {
                return i;
            }
        }
        return -1;
    }
}

