using System.Collections;
using System.Collections.Generic;
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

    public void GenerateGrid()
    {

        GridInstance gridParent = Instantiate(gridInstancePrefab);
        gridParent.transform.position = Vector3.zero;
        gridParent.gameObject.name = "Grid " + row + "x" + column;


        for(int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {

                GameObject cell = new GameObject("Cell "+i+"."+j);
                GameObject cellContent = Instantiate(cellContentPrefab);
                cell.transform.localScale = new Vector3(cellWidth,1,cellHeight);

                cell.transform.position = new Vector3(j * cellWidth,0,i * cellHeight);
                cell.transform.parent = gridParent.transform;
                gridParent.GetCells().Add(cell.transform);
                cellContent.transform.position = cell.transform.position;
                cellContent.transform.localScale *= cellContentScaleFactor;
                cellContent.transform.parent = cell.transform;

                /*GameObject cell = Instantiate(cellContentPrefab);
                cell.transform.localScale *= cellContentScaleFactor;

                cell.transform.position = new Vector3(j * cellWidth, 0, i * cellHeight);
                cell.transform.parent = gridParent.transform;
                gridParent.GetCells().Add(cell.transform);
                //cellContent.transform.position = cell.transform.position;
                //cellContent.transform.localScale *= cellContentScaleFactor;
                //cellContent.transform.parent = cell.transform;*/

            }
        }


        /*int centerCellIndex;

        if (gridParent.GetCells().Count % 2 == 1)
        {
            centerCellIndex = (gridParent.GetCells().Count - 1) / 2;
            Camera.main.transform.position = gridParent.GetCells()[centerCellIndex].transform.position + new Vector3(0,10,0);
        }
        else
        {
            centerCellIndex = gridParent.GetCells().Count / 2;
            Vector3 halfPosition = (gridParent.GetCells()[centerCellIndex - 1].transform.position + gridParent.GetCells()[centerCellIndex].transform.position) / 2;
            Camera.main.transform.position = new Vector3(halfPosition.x,10,halfPosition.z);
        }*/

    }
}
