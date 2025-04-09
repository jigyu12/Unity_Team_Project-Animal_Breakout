using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridAutoScaler : MonoBehaviour
{
    [SerializeField] int columns; 
    [SerializeField] int rows;
    [SerializeField] bool isUseColumnsCount;
    [SerializeField] bool isUseRowsCount;

    private GridLayoutGroup gridLayoutGroup;
    private RectTransform rectTransform;

    private Vector2 cellSize;

    private void Start()
    {
        TryGetComponent(out gridLayoutGroup);
        TryGetComponent(out rectTransform);
    }

    private void OnRectTransformDimensionsChange()
    {
        if (gridLayoutGroup is null || rectTransform is null) 
            return;
        
        AdjustCellSize();
    }

    private void AdjustCellSize()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        if (isUseColumnsCount && !isUseRowsCount && columns > 0)
        {
            float totalHorizontalSpacing = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + gridLayoutGroup.spacing.x * (columns - 1);
            float cellWidth = (width - totalHorizontalSpacing) / columns;
            
            cellSize.x = cellWidth;
            cellSize.y = gridLayoutGroup.cellSize.y;
            
            gridLayoutGroup.cellSize = cellSize;
        }
        else if (!isUseColumnsCount && isUseRowsCount && rows > 0)
        {
            float totalVerticalSpacing = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + gridLayoutGroup.spacing.y * (rows - 1);
            float cellHeight = (height - totalVerticalSpacing) / rows;
            
            cellSize.x = gridLayoutGroup.cellSize.x;
            cellSize.y = cellHeight;
            
            gridLayoutGroup.cellSize = cellSize;
        }
        else if (isUseColumnsCount && isUseRowsCount && columns > 0 && rows > 0)
        {
            float totalHorizontalSpacing = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right + gridLayoutGroup.spacing.x * (columns - 1);
            float totalVerticalSpacing = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom + gridLayoutGroup.spacing.y * (rows - 1);
            float cellWidth = (width - totalHorizontalSpacing) / columns;
            float cellHeight = (height - totalVerticalSpacing) / rows;
            
            cellSize.x = cellWidth;
            cellSize.y = cellHeight;
            
            gridLayoutGroup.cellSize = cellSize;
        }
    }
}