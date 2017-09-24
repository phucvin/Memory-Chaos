using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animation), typeof(Image))]
public class ReplayTile : MonoBehaviour
{
    public Text Text;
    public string SpawnClip = string.Empty;
    public string CorrectClip = string.Empty;
    public string HintClip = string.Empty;

    private List<Sprite> _images = null;

    private Image _image;
    private Animation _animation;

    public void Awake()
    {
        _image = GetComponent<Image>();
        _animation = GetComponent<Animation>();
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
        StartCoroutine(delaySpawn(index));
    }

    public void Correct()
    {
        Text.text = string.Empty;
        _animation.Play(CorrectClip);
    }

    public void Hint()
    {
        _animation.Play(HintClip);
    }

    private IEnumerator delaySpawn(int index)
    {
        while (_animation.IsPlaying(CorrectClip)) yield return null;

        if (_images != null)
        {
            Text.text = string.Empty;
            _image.sprite = _images[index];
        }
        else
        {
            Text.text = (index + 1).ToString();
        }
        _animation.Play(SpawnClip);
    }
}