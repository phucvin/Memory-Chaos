using System.Collections;
using UnityEngine;

public class Game : MonoBehaviour
{
    public Timer Timer;
    public WinOverlay WinOverlay;
    public GameObject LoseOverlay;
    public CanvasGroup Board;
    
    public void Win(ReplayData replayData)
    {
        stop();
        replayData.EndTimeInSeconds = Timer.ElapsedSeconds;
        WinOverlay.Setup(Timer.ElapsedSeconds, replayData);
        StartCoroutine(delayActivate(WinOverlay.gameObject));
    }

    public void Lose()
    {
        stop();
        StartCoroutine(delayActivate(LoseOverlay));
    }

    private void stop()
    {
        Timer.StopRunning();
        Board.interactable = false;
    }

    private IEnumerator delayActivate(GameObject gameObj)
    {
        yield return new WaitForSeconds(0.5f);
        gameObj.SetActive(true);
    }
}