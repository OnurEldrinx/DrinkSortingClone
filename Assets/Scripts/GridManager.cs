using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int column;
    [SerializeField] private float cellWidth;
    [SerializeField] private float cellHeight;
    [SerializeField] private GameObject cellContentPrefab;
    [SerializeField] private float cellContentScaleFactor;
    [SerializeField] private GridInstance gridInstancePrefab;
    [SerializeField] private bool useCellContentGraphic;
    public void GenerateGrid()
    {

        if (!useCellContentGraphic)
        {
            cellContentPrefab = null;
        }
        
        GridInstance gridParent = Instantiate(gridInstancePrefab);
        gridParent.transform.position = Vector3.zero;
        gridParent.gameObject.name = row + "x" + column;
        gridParent.SetRowAndColumnCount(row,column);

        for(int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {

                GameObject cell = new GameObject(i+"."+j);
                GridCell cellScript = cell.AddComponent<GridCell>();
                cellScript.row = i;
                cellScript.column = j;
                
                cell.transform.localScale = new Vector3(cellWidth,1,cellHeight);
                cell.transform.position = new Vector3(j * cellWidth,0,i * cellHeight);
                cell.transform.parent = gridParent.transform;
                gridParent.GetCells().Add(cell.transform);
                
                if (cellContentPrefab is not null)
                {
                    GameObject cellContent = Instantiate(cellContentPrefab);
                    cellContent.transform.position = cell.transform.position;
                    cellContent.transform.localScale *= cellContentScaleFactor;
                    cellContent.transform.parent = cell.transform;
                    cellScript.content = cellContent;
                }
                
            }
        }


        int centerCellIndex;
        GameObject gridCenter = new GameObject("GridCenter");

        if (gridParent.GetCells().Count % 2 == 1)
        {
            centerCellIndex = (gridParent.GetCells().Count - 1) / 2;
            gridCenter.transform.position = gridParent.GetCells()[centerCellIndex].transform.position;
            //Camera.main.transform.position = gridParent.GetCells()[centerCellIndex].transform.position + new Vector3(0,10,0);
            
        }
        else
        {
            centerCellIndex = gridParent.GetCells().Count / 2;
            Vector3 halfPosition = (gridParent.GetCells()[centerCellIndex - 1].transform.position + gridParent.GetCells()[centerCellIndex].transform.position) / 2;
            gridCenter.transform.position = halfPosition;
            //Camera.main.transform.position = new Vector3(halfPosition.x,10,halfPosition.z);
        }

        gridParent.transform.parent = gridCenter.transform;

    }
}
