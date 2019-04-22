using UnityEngine;

namespace _2048._Scripts
{
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
            foreach (var t in TileStyles)
            {
                t.Name = t.Number.ToString();
                if (t.TileColor.a == 0) { t.TileColor = new Color32(255, 255, 255, 255); }
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
}

