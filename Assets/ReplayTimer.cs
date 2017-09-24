using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ReplayTimer : MonoBehaviour
{
    private Text _text;

    private StringBuilder _sb = new StringBuilder();

    public void Awake()
    {
        _text = GetComponent<Text>();
        _text.text = "Time: 0.00";
    }

    public void Set(float elapsedSeconds)
    {
        _sb.Length = 0;
        _sb.AppendFormat("Time: {0:0.00}", elapsedSeconds);
        _text.text = _sb.ToString();
    }
}