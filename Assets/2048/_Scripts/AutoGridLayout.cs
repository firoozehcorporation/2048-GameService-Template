using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _2048._Scripts
{
    [ExecuteInEditMode]
    [AddComponentMenu("Layout/Auto Grid Layout Group", 152)]
    public class AutoGridLayout : GridLayoutGroup
    {
        [SerializeField]
        private bool _isColumn = false;
        [SerializeField]
        private int _column = 1;

        [SerializeField]
        private int _row = 1;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            float iColumn = -1;
            float iRow = -1;
            if (_isColumn)
            {
                iColumn = _column;
                if (iColumn <= 0)
                {
                    iColumn = 1;
                }
                iRow = Mathf.CeilToInt(this.transform.childCount / iColumn);
            }
            else
            {
                iRow = _row;
                if (iRow <= 0)
                {
                    iRow = 1;
                }
                iColumn = Mathf.CeilToInt(this.transform.childCount / iRow);
            }
            float fHeight = (rectTransform.rect.height - ((iRow - 1) * (spacing.y))) - ((padding.top + padding.bottom));
            float fWidth = (rectTransform.rect.width - ((iColumn - 1) * (spacing.x))) - ( (padding.right + padding.left));
            Vector2 vSize = new Vector2(fWidth / iColumn, (fHeight) / iRow);
            cellSize = vSize;
        }
    }
}
