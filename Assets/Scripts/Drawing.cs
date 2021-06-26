using UnityEngine;

public class Drawing : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera MainCam;
    public GameObject Brush;
    private GameObject brushInstantiated;

    [Header("TouchResults")]
    private LineRenderer currentLineRenderer;

    [Header("Vectors")]
    private Vector3 lastPos;
    private Vector3 mousePos;
    private Vector3 mousePosCorrected;

    [Header("ValuesCorrectable")]
    [SerializeField] private float distanceFromCamOfBrush;
    [SerializeField] private float[] brushlimitsHorizontal = new float[2];
    [SerializeField] private float[] brushlimitsVertical = new float[2];

    [Header("Connection")]
    public Relocation relocation;

    private void Awake()
    {
        relocation = FindObjectOfType<Relocation>();
    }

    private void Update()
    {
        Draw();
    }
    private void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(brushInstantiated);

            CreateBrush();
            //While user touch/mousePos starts create Brushes 
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            //While user touch/mousePos is active
            GetMousePositionCorrection();

            if (mousePosCorrected != lastPos)
            {
                AddPoint(mousePosCorrected);
                lastPos = mousePosCorrected;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            relocation.RelocateRunners();
            //When touch/mousePos is unpressed
        }
    }

    #region screenBrushControl
    private void CreateBrush()
    {
        brushInstantiated = Instantiate(Brush);
        //Set as a child to this gameObject to not stop on the instantiated place 

        currentLineRenderer = brushInstantiated.GetComponent<LineRenderer>();
        //To instantiate Brush and get LineRenderer from it

        GetMousePositionCorrection();
        //Set "mousePosCorrected" to the values of touch/mousePos, correcting Z axis to "distanceFromCamOfBrush" value
    }

    private void AddPoint(Vector3 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
        //Add point and set it to the position of touch/mousePos
    }

    private void GetMousePositionCorrection()
    {
        mousePos = Input.mousePosition;
        //Correction needed because of the mistake when Z axis == 0 brush just sets to cam.position

        CheckForTheLimits();
        //Check and set any touch/mousePos outside of drawing zone to its border values 

        mousePos.z = distanceFromCamOfBrush;
        //Correction needed because of the mistake when Z axis == 0 brush just sets to cam.position

        mousePosCorrected = MainCam.ScreenToWorldPoint(mousePos);
        //Set touch/mousePos from cam scale to World scale  
    }

    private void CheckForTheLimits()
    {
        if (mousePos.x < brushlimitsHorizontal[0])
            mousePos.x = brushlimitsHorizontal[0];

        if (mousePos.x > brushlimitsHorizontal[1])
            mousePos.x = brushlimitsHorizontal[1];

        if (mousePos.y < brushlimitsVertical[0])
            mousePos.y = brushlimitsVertical[0];

        if (mousePos.y > brushlimitsVertical[1])
            mousePos.y = brushlimitsVertical[1];

        //Compare all of the touch/mousePos values to fit with limits 
    }
    #endregion
}
