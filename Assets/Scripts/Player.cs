using UnityEngine;

public class Player : MonoBehaviour
{

    private Camera mainCamera;

    private bool dragging = false;

    private Transform draggingTarget; //container
    private Transform source; // container slot
    private Transform target; // container slot

    [SerializeField] private LayerMask draggableLayer; //container
    [SerializeField] private LayerMask slotLayer; //container slot
    [SerializeField] private LayerMask movementPlane;

    private Vector3 draggingOffset;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray origin = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0) && !dragging)
        {

            if (Physics.Raycast(origin, out hit, float.MaxValue, draggableLayer))
            {
                dragging = true;
                draggingTarget = hit.collider.transform;
               
                source = draggingTarget.parent;
                print("dragging start");

                Vector3 mousePos = Input.mousePosition;
                mousePos.z = mainCamera.WorldToScreenPoint(draggingTarget.transform.position).z;
                Vector3 draggingVector = mainCamera.ScreenToWorldPoint(mousePos);

                draggingOffset = draggingTarget.position - draggingVector;

            }

        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            print("dragging end");

            if (Physics.Raycast(origin, out hit, float.MaxValue, slotLayer))
            {

                if (hit.transform.GetChild(0).childCount == 0)
                {
                    draggingTarget.parent = hit.transform.GetChild(0);
                    draggingTarget.localPosition = Vector3.zero;
                    draggingTarget.GetComponent<BoxCollider>().enabled = false;
                }
                else
                {
                    draggingTarget.localPosition = Vector3.zero;
                }


            }
            else
            {
                draggingTarget.localPosition = Vector3.zero;

            }



            if (draggingTarget is not null)
            {
                draggingTarget = null;
            }

        }

        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(draggingTarget.transform.position).z;
            Vector3 draggingVector = mainCamera.ScreenToWorldPoint(mousePos);
            Vector3 movementVector = draggingVector + draggingOffset;
            draggingTarget.transform.position = movementVector;

            


        }


    }

    
}
