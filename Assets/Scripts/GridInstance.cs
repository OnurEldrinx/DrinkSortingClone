using System.Collections.Generic;
using UnityEngine;

public class GridInstance : MonoBehaviour
{
    [SerializeField] private List<Transform> cells;
    public int rows;
    public int columns;
    private List<List<Transform>> gridMatrix;
    public bool tableGrid;
    
    private void Start()
    {
        if (tableGrid)
        {
            CreateGridMatrix();
        }
        
    }

    private void CreateGridMatrix()
    {
        gridMatrix = new List<List<Transform>>();


        int indexHead = 0;
        for (int i = 0; i < rows; i++)
        {

            List<Transform> tempRow = new List<Transform>();

            for (int j = 0; j < columns; j++)
            {
                tempRow.Add(cells[indexHead]);
                indexHead++;
                //gridMatrix.SetValue(cells[indexHead],i,j);
            }
            
            gridMatrix.Add(tempRow);

        }
        
        
    }

    public void SetRowAndColumnCount(int r,int c)
    {
        rows = r;
        columns = c;
    }
    
    public void InsertCell(Transform cell)
    {
        cells.Add(cell);
    }

    public void InsertToGridMatrix(Transform cell,int r,int c)
    {
        gridMatrix[r][c] = cell;
    }
    

    public List<Transform> GetCells()
    {
        return cells;
    }

    public Transform GetCell(int row,int col)
    {
        return gridMatrix[row][col];
    }

    public List<Transform> GetNeighborsOf(Transform cell)
    {
        List<Transform> neighbors = new List<Transform>();
        if (cell.TryGetComponent(out GridCell cellScript))
        {            
            // Check above neighbor
            if (cellScript.row - 1 >= 0)
                neighbors.Add(gridMatrix[cellScript.row - 1][cellScript.column]);
            // Check below neighbor
            if (cellScript.row + 1 < rows)
                neighbors.Add(gridMatrix[cellScript.row + 1][cellScript.column]);
            // Check left neighbor
            if (cellScript.column - 1 >= 0)
                neighbors.Add(gridMatrix[cellScript.row][cellScript.column - 1]);
            // Check right neighbor
            if (cellScript.column + 1 < columns)
                neighbors.Add(gridMatrix[cellScript.row][cellScript.column + 1]);
        }
        
        
        return neighbors;

    }
    
   
    
}
