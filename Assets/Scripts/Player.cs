using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{

    private Camera mainCamera;

    private bool dragging;

    private Transform draggingTarget; //container
    //private Transform source; // container slot
    private Transform target; // container slot

    [SerializeField] private LayerMask draggableLayer; //container
    [SerializeField] private LayerMask slotLayer; //container slot

    private Vector3 draggingOffset;

    private Tweener movementTween;

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
               
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = mainCamera.WorldToScreenPoint(draggingTarget.transform.position).z;
                Vector3 draggingVector = mainCamera.ScreenToWorldPoint(mousePos);

                draggingOffset = draggingTarget.position - draggingVector;

            }

        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            dragging = false;
            DOTween.Clear();

            if (Physics.Raycast(origin, out hit, float.MaxValue, slotLayer))
            {

                ContainerSlot containerSlot = hit.transform.GetComponent<ContainerSlot>();

                containerSlot.AcceptOrReject(draggingTarget.GetComponent<Container>());

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
            if (draggingTarget is null ) return;
            
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = mainCamera.WorldToScreenPoint(draggingTarget.transform.position).z;
            Vector3 draggingVector = mainCamera.ScreenToWorldPoint(mousePos);
            Vector3 movementVector = draggingVector + draggingOffset;
            //draggingTarget.transform.position = movementVector;

            
            movementTween = draggingTarget.DOMove(movementVector,0.05f).SetEase(Ease.InSine);

            
        }


    }

    
}
