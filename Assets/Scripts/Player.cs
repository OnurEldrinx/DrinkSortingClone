using UnityEngine;
using DG.Tweening;

public class Player : Singleton<Player>
{

    private Camera mainCamera;

    private bool dragging;

    private Transform draggingTarget; //container
    //private Transform source; // container slot
    private Transform target; // container slot

    [SerializeField] private LayerMask draggableLayer; //container
    [SerializeField] private LayerMask slotLayer; //container slot

    private Vector3 draggingOffset;

    public bool animationOnPlay;

    public bool downgradeResolution;

    private int resX;
    private int resY;
    
    private void Awake()
    {
        
        if (PlayerPrefs.HasKey("resX"))
        {
            resX = PlayerPrefs.GetInt("resX");
            resY = PlayerPrefs.GetInt("resY");
        }
        else
        {
            PlayerPrefs.SetInt("resX", Screen.currentResolution.width);
            PlayerPrefs.SetInt("resY", Screen.currentResolution.height);
            PlayerPrefs.SetInt("hZ", Screen.currentResolution.refreshRate);
            resX = PlayerPrefs.GetInt("resX");
            resY = PlayerPrefs.GetInt("resY");

        }

        if (downgradeResolution)
        {
            Screen.SetResolution((int)(resX * 0.75f),(int)(resY * 0.75f), FullScreenMode.FullScreenWindow);
        }
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        
    }

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

            
            draggingTarget.DOMove(movementVector,0.05f).SetEase(Ease.InSine);

            
        }


    }

    
}
