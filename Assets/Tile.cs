using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animation), typeof(Button), typeof(Image))]
public class Tile : MonoBehaviour
{
    public Text Text;
    public Board Board;
    public Game Game;
    public string SpawnClip = string.Empty;
    public string CorrectClip = string.Empty;
    public string IncorrectClip = string.Empty;
    public string HintClip = string.Empty;

    private List<Sprite> _images = null;
    private int _index = -1;

    private Image _image;
    private Animation _animation;

    public int Index { get { return _index; } }

    public void Awake()
    {
        _image = GetComponent<Image>();
        _animation = GetComponent<Animation>();
        GetComponent<Button>().onClick.AddListener(click);
    }

    public void SetCellSize(float size)
    {
        Text.fontSize = Mathf.RoundToInt(size * 23f / 50f);
    }

    public void SetImages(List<Sprite> images)
    {
        _images = images;
    }

    public void Setup(int index)
    {
        _index = index;
        StartCoroutine(delaySpawn());
    }

    public void Correct()
    {
        _index = -1;
        Text.text = string.Empty;
        _animation.Play(CorrectClip);
    }

    public void Incorrect()
    {
        _index = -1;
        _animation.Play(IncorrectClip);
    }

    public void Hint()
    {
        _animation.Play(HintClip);
    }

    private IEnumerator delaySpawn()
    {
        while (_animation.IsPlaying(CorrectClip)) yield return null;

        if (_images != null)
        {
            Text.text = string.Empty;
            _image.sprite = _images[_index];
        }
        else
        {
            Text.text = (_index + 1).ToString();
        }
        _animation.Play(SpawnClip);
    }

    private void click()
    {
        if (_index < 0) return;

        Board.TileClick(this);
    }
}