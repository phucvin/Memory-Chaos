using UnityEngine;
using UnityEngine.UI;

public class WinOverlay : MonoBehaviour
{
    public Game Game;
    public Text ElapsedSecondsText;
    public ReplayGame ReplayGame;

    private ReplayData _replayData;

    public void Setup(float elapsedSeconds, ReplayData replayData)
    {
        ElapsedSecondsText.text = string.Format("{0:0.00}s", elapsedSeconds);
        _replayData = replayData;
    }

    public void Replay()
    {
        Game.gameObject.SetActive(false);
        ReplayGame.Setup(_replayData);
    }
}