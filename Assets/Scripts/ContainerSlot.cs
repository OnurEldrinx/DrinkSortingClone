using UnityEngine;

public class ContainerSlot : MonoBehaviour
{
    private Transform pivot;

    private Container currentContainer;

    private void Start()
    {
        pivot = transform.GetChild(0);
    }

    public bool IsEmpty()
    {
        return pivot.childCount <= 0;
    }

    public void AcceptOrReject(Container container)
    {
        if (pivot.childCount <= 0)
        {
            container.transform.parent = pivot;
            container.transform.localPosition = Vector3.zero;
            container.GetComponent<BoxCollider>().enabled = false;

            currentContainer = container;
            

        }
        else
        {
            container.transform.localPosition = Vector3.zero;

        }
    }

    public GridInstance GetGrid()
    {
        return transform.parent.parent.GetComponent<GridInstance>();
    }

}
