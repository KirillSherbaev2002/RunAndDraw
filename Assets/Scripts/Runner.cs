using UnityEngine;

public class Runner : MonoBehaviour
{
    public GameObject DeadBody;
    public GameObject[] CanvasesReplacable;
    public GameObject Salut;
    public Score score;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obsticle"))
        {
            Destroy(gameObject);
            Instantiate(DeadBody, transform.position, transform.rotation);
        }
        if (other.CompareTag("YouWon"))
        {
            CanvasesReplacable[0].SetActive(false);
            CanvasesReplacable[1].SetActive(true);
            Instantiate(Salut, transform.position, transform.rotation);
        }
        if (other.CompareTag("Gem"))
        {
            score.GemCollected();
            Destroy(other.gameObject);
        }
    }
}
