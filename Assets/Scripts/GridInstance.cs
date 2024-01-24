using System.Collections.Generic;
using UnityEngine;

public class GridInstance : MonoBehaviour
{
    [SerializeField] private List<Transform> cells;

    public void InsertCell(Transform cell)
    {
        cells.Add(cell);
    }

    public List<Transform> GetCells()
    {
        return cells;
    }

    public List<Transform> GetNeighboursOf(Transform cell)
    {
        List<Transform> neighbours = new List<Transform>();

        //if(Physics.Raycast(cell.position,Vector3.forward,float.MaxValue))
        // Ray to 4 directions;

        return neighbours;
    }
    
}
