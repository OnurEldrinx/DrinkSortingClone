using System.Collections.Generic;
using UnityEngine;

public class GridCell:MonoBehaviour
{
    public int row;
    public int column;
    public GameObject content;
    
    public List<Transform> GetNeighbors(Transform[,] gridMatrix)
    {
        List<Transform> neighbors = new List<Transform>();
        
        int rows = gridMatrix.GetLength(0);
        int cols = gridMatrix.GetLength(1);

        // Check above neighbor
        if (row - 1 >= 0)
            neighbors.Add(gridMatrix[row - 1,column]);
        // Check below neighbor
        if (row + 1 < rows)
            neighbors.Add(gridMatrix[row + 1,column]);
        // Check left neighbor
        if (column - 1 >= 0)
            neighbors.Add(gridMatrix[row,column - 1]);
        // Check right neighbor
        if (column + 1 < cols)
            neighbors.Add(gridMatrix[row,column + 1]);

        return neighbors;

    }

}
