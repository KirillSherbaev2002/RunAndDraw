using UnityEngine;

public class Drawing : MonoBehaviour
{
    public Camera MainCam;
    public GameObject Brush;

    private LineRenderer currentLineRenderer;

    Vector3 lastPos;
    Vector3 mousePosCorrected;
    Vector3 mousePos;

    [SerializeField] private float distanceFromCamOfBrush;

    private void Update()
    {
        Draw();
        Vector3 mousePos = Input.mousePosition;
        print(mousePos);
    }

    void Draw()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            GettingMousePositionCorrected();

            if (mousePosCorrected != lastPos) 
            {
                AddPoint(mousePosCorrected);
                lastPos = mousePosCorrected;
            }
        }
        else
        {
            currentLineRenderer = null;
        }
    }

    void CreateBrush()
    {
        GameObject brushInstantiate = Instantiate(Brush);
        currentLineRenderer = brushInstantiate.GetComponent<LineRenderer>();

        GettingMousePositionCorrected();
    }

    void AddPoint(Vector3 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount-1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void GettingMousePositionCorrected()
    {
        var mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamOfBrush;
        //Correction needed because of the mistake when Z axis == 0 brush just sets to cam.position
        mousePosCorrected = MainCam.ScreenToWorldPoint(mousePosition);
    }
}
