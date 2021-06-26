using UnityEngine;
using System.Collections;
using System;

public class Relocation : MonoBehaviour
{
    [Header("GameObjects")]
    public Camera RelocationCam;
    public GameObject Brush;
    private GameObject brushInstantiated;
    private GameObject[] Runners;
    public GameObject TextToMakeLineLonger;

    public GameObject GameOverCanvas;
    public GameObject GameCanvas;

    [Header("TouchResults")]
    private LineRenderer currentLineRenderer;

    [Header("Vectors")]
    private Vector3 lastPos;
    private Vector3 mousePos;
    private Vector3 mousePosCorrected;

    [Header("ValuesCorrectable")]
    [SerializeField] private float distanceFromCamOfBrush;
    [SerializeField] private float RunnerPositionY;
    [SerializeField] private float[] brushlimitsHorizontal = new float[2];
    [SerializeField] private float[] brushlimitsVertical = new float[2];

    private void Update()
    {
        Draw();
        CheckOnGameOver();
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
            //When touch/mousePos is unpressed
        }
    }

    #region sceneBrushControl
    private void CreateBrush()
    {
        brushInstantiated = Instantiate(Brush);
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

        mousePosCorrected = RelocationCam.ScreenToWorldPoint(mousePos);
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

    public void RelocateRunners()
    {
        float NumberOfVector3InArray = currentLineRenderer.positionCount;
        //Total number of Vector3's need to make a choice between them

        Runners = GameObject.FindGameObjectsWithTag("Runner");
        //Find all runners on the scene

        if (NumberOfVector3InArray > Runners.Length)
        //If there was more Vector3's than players make a relocation
        {
            TextToMakeLineLonger.SetActive(false); 
            //Notification to make line more curve deactivated

            float positionsPerRunner = NumberOfVector3InArray / Runners.Length;
            //Frequency of Vector3's per runner helps (if there is more than one Vector3 per player) set a distance between 

            float summOfPositionsPerRunner = 0;
            //Summ of frequencies already apllied 

            for (int i = 0; i < Runners.Length; i++)
            {

                Vector3 currentCoordinates = currentLineRenderer.GetPosition((int)Math.Round(summOfPositionsPerRunner));
                //getting Vector3 using integer value - sum of the applied frequencies

                summOfPositionsPerRunner += positionsPerRunner;
                //Needed to not lose the values less than 1 between 2 respawns (not using sum 2.5 + 2.5 = 2 + 2) (using sum 2.5 + 2.5 = 5)

                Runners[i].transform.position = new Vector3(currentCoordinates.x, RunnerPositionY, currentCoordinates.z);
                //Set the transform.position for runners using x,z from array and y from the inspector 
            }
        }
        else
        {
            TextToMakeLineLonger.SetActive(true);
            //Notification to make line more curve activated
        }
    }

    private void CheckOnGameOver()
    {
        Runners = GameObject.FindGameObjectsWithTag("Runner");
        if (Runners.Length == 0)
        {
            GameCanvas.SetActive(false);
            GameOverCanvas.SetActive(true);
        }
    }
}
