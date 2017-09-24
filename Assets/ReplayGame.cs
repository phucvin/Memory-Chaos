using System.Collections;
using UnityEngine;

public class ReplayGame : MonoBehaviour
{
    public ReplayBoard Board;
    public GameObject EndOverlay;

    public void Setup(ReplayData data)
    {
        gameObject.SetActive(true);
        Board.Setup(data);
    }

    public void End()
    {
        StartCoroutine(delayEnd());
    }

    private IEnumerator delayEnd()
    {
        yield return new WaitForSeconds(1f);
        EndOverlay.SetActive(true);
    }
}