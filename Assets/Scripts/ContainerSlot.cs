using System.Collections.Generic;
using UnityEngine;

public class ContainerSlot : MonoBehaviour
{

    private Container currentContainer;
    public GridInstance grid;

    private void Start()
    {
        grid = transform.parent.parent.GetComponent<GridInstance>();
    }

    public bool IsEmpty()
    {
        return currentContainer is null;
    }

    public void AcceptOrReject(Container container)
    {
        Transform t = transform;
        Transform containerTransform = container.transform;
        if (currentContainer is null)
        {
            
            Transform cell = transform.parent;

            containerTransform.parent = null;
            containerTransform.position = t.position;
            containerTransform.rotation = t.rotation;
            container.DisableCollider();
            currentContainer = container;

            
            List<Transform> neighbors = grid.GetNeighborsOf(cell);

            foreach (var n in neighbors)
            {
               n.gameObject.SetActive(false);
            }

        }
        else
        {
            containerTransform.localPosition = Vector3.zero;
        }
        
    }
    
    
    
    
    

    public GridInstance GetGrid()
    {
        return grid;
    }

}
