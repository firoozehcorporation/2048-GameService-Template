using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[ExecuteInEditMode]
[AddComponentMenu("Layout/Auto Grid Layout Group", 152)]
public class AutoGridLayout : GridLayoutGroup
{
    [SerializeField]
    private bool m_IsColumn;
    [SerializeField]
    private int m_Column = 1, m_Row = 1;
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        float iColumn = -1;
        float iRow = -1;
        if (m_IsColumn)
        {
            iColumn = m_Column;
            if (iColumn <= 0)
            {
                iColumn = 1;
            }
            iRow = Mathf.CeilToInt(this.transform.childCount / iColumn);
        }
        else
        {
            iRow = m_Row;
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
