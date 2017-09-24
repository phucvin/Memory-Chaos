using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PrevTile : MonoBehaviour
{
    public Text Text;

    private Image _image;

    private List<Sprite> _images;

    public void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetImages(List<Sprite> images)
    {
        _images = images;
    }

    public void Set(int prevIndex)
    {
        if (prevIndex >= 0)
        {
            if (_images != null)
            {
                _image.sprite = _images[prevIndex];
                Text.text = string.Empty;
            }
            else
            {
                Text.text = (prevIndex + 1).ToString();
            }
        }
        else
        {
            Text.text = string.Empty;
        }
    }
}