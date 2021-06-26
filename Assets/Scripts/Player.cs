using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    public GameObject StartCanvas;
    public GameObject GameOverCanvas;
    public GameObject YouWon;
    void Update()
    {
        if (StartCanvas.activeSelf == false && GameOverCanvas.activeSelf == false && YouWon.activeSelf == false)
        {
            transform.position += speed * Time.deltaTime;
        }
    }
}
