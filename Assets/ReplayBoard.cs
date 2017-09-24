using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplayBoard : MonoBehaviour
{
    public ReplayGame Game;
    public ReplayTimer Timer;
    public PrevTile PrevTile;
    public ReplayTile Tile;
    public GridLayoutGroup GridLayout;

    private ReplayData _data;
    private List<Sprite> _images;
    private float _startTimeInSeconds = 0f;
    private int _nextEventIndex = 0;
    private int _nextTileIndex = 0;
    
    private Dictionary<int, ReplayTile> _tileMap = new Dictionary<int, ReplayTile>();

    public void Setup(ReplayData data)
    {
        _data = data;

        StartCoroutine(createBoard());
    }

    public void Update()
    {
        if (_data == null || _startTimeInSeconds <= 0) return;

        if (_nextEventIndex < _data.Events.Count)
        {
            var nextEvent = _data.Events[_nextEventIndex];
            float elapsedSeconds = Time.realtimeSinceStartup - _startTimeInSeconds;
            if (elapsedSeconds >= nextEvent.TimeInSeconds)
            {
                elapsedSeconds = nextEvent.TimeInSeconds;
                _startTimeInSeconds = Time.realtimeSinceStartup - elapsedSeconds;
                
                switch (nextEvent.Type)
                {
                    case 1:
                        _tileMap[_nextTileIndex].Correct();
                        if ((_data.BoardInfo.MaxTileOnBoard + _nextTileIndex) < _data.BoardInfo.End)
                        {
                            int nextIndexForThisTile = _data.Indexes[_data.BoardInfo.MaxTileOnBoard + _nextTileIndex];
                            _tileMap[_nextTileIndex].Setup(nextIndexForThisTile);
                            _tileMap[nextIndexForThisTile] = _tileMap[_nextTileIndex];
                        }
                        ++_nextTileIndex;
                        PrevTile.Set(_nextTileIndex - 1);
                        break;

                    case 2:
                        _tileMap[_nextTileIndex].Hint();
                        break;
                }
                ++_nextEventIndex;
            }
            Timer.Set(elapsedSeconds);
        }
        else
        {
            Timer.Set(_data.EndTimeInSeconds);
            Game.End();
        }
    }

    private IEnumerator createBoard()
    {
        Tile.gameObject.SetActive(false);
        if (_data.BoardInfo.Images != null && _data.BoardInfo.Images.Count > 0)
        {
            _images = new List<Sprite>(_data.BoardInfo.Images.Count);
            for (int i = 0, n = _data.BoardInfo.Images.Count; i < n; ++i)
            {
                var loading = G.H.LoadSpriteAsync(_data.BoardInfo.Images[i]);
                yield return loading;
                _images.Add(loading.GetAsset<Sprite>());
            }
        }
        Tile.gameObject.SetActive(true);

        _startTimeInSeconds = Time.realtimeSinceStartup;

        PrevTile.SetImages(_images);
        PrevTile.Set(-1);

        GridLayout.cellSize = new Vector2(_data.BoardInfo.TileCellSize, _data.BoardInfo.TileCellSize);
        Tile.SetCellSize(_data.BoardInfo.TileCellSize);

        var _tiles = new List<ReplayTile>();
        
        Tile.SetImages(_images);
        Tile.Setup(_data.Indexes[0]);
        Tile.GetComponent<CanvasGroup>().alpha = 0f;
        _tileMap[_data.Indexes[0]] = Tile;
        _tiles.Add(Tile);
        for (int i = 1; i < _data.BoardInfo.MaxTileOnBoard; ++i)
        {
            var aTile = Instantiate(Tile);
            aTile.transform.SetParent(this.transform, false);
            _tileMap[_data.Indexes[i]] = aTile;
            _tiles.Add(aTile);
        }

        for (int i = 0; i < _data.BoardInfo.MaxTileOnBoard; ++i)
        {
            var aTile = _tiles[i];
            aTile.GetComponent<CanvasGroup>().alpha = 1f;
            aTile.SetImages(_images);
            aTile.Setup(_data.Indexes[i]);
            yield return new WaitForSeconds(0.015f);
        }
    }
}