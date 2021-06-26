using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text PointsText;
    private float points;

    void Awake()
    {
        points = PlayerPrefs.GetFloat("pointsTotal");
    }

    void Update()
    {
        PointsText.text = points.ToString();
    }

    public void GemCollected()
    {
        points++;
        PlayerPrefs.SetFloat("pointsTotal", points);
    }
}
