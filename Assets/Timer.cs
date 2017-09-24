using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class Timer : MonoBehaviour
{
    private Text _text;

    private bool _isRunning = false;
    private float _startSecond;
    private float _stopSecond;

    private StringBuilder _sb = new StringBuilder();

    public float ElapsedSeconds
    {
        get
        {
            if (_isRunning)
            {
                return Time.realtimeSinceStartup - _startSecond;
            }
            else
            {
                return _stopSecond - _startSecond;
            }
        }
    }

    public void Awake()
    {
        _text = GetComponent<Text>();
        _text.text = "Time: 0.00";
    }

    public void StartRunning()
    {
        _startSecond = Time.realtimeSinceStartup;
        _isRunning = true;
    }

    public void Update()
    {
        if (!_isRunning) return;

        updateText();
    }

    public void StopRunning()
    {
        _stopSecond = Time.realtimeSinceStartup;
        _isRunning = false;
        updateText();
    }

    private void updateText()
    {
        _sb.Length = 0;
        _sb.AppendFormat("Time: {0:0.00}", ElapsedSeconds);
        _text.text = _sb.ToString();
    }
}