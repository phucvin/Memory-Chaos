using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public Game Game;
    public Timer Timer;
    public PrevTile PrevTile;
    public Tile Tile;

    public GridLayoutGroup GridLayout;

    private BoardInfo _info;
    private List<Sprite> _images;

    private List<Tile> _tiles = new List<Tile>();

    public int NextTileIndex { get; private set; }

    private int _maxOnBoard;
    private List<int> _indexes = new List<int>();
    private System.Random _rand = new System.Random();

    private float _lastTimeCorrectTileInSeconds;

    private float _startTimeInSeconds;
    private List<ReplayEvent> _replayEvents = new List<ReplayEvent>();

    public void Awake()
    {
        _info = G.BoardInfo;

        _maxOnBoard = _info.MaxTileOnBoard;
    }

    public IEnumerator Start()
    {
        Tile.gameObject.SetActive(false);
        if (_info.Images != null && _info.Images.Count > 0)
        {
            _images = new List<Sprite>(_info.Images.Count);
            for (int i = 0, n = _info.Images.Count; i < n; ++i)
            {
                var loading = G.H.LoadSpriteAsync(_info.Images[i]);
                yield return loading;
                _images.Add(loading.GetAsset<Sprite>());
            }
        }
        Tile.gameObject.SetActive(true);

        PrevTile.SetImages(_images);
        GridLayout.cellSize = new Vector2(_info.TileCellSize, _info.TileCellSize);
        Tile.SetCellSize(_info.TileCellSize);

        Timer.StartRunning();
        _startTimeInSeconds = Time.realtimeSinceStartup;
        _lastTimeCorrectTileInSeconds = Time.realtimeSinceStartup;
        PrevTile.Set(-1);
        NextTileIndex = 0;
        generateIndexes();

        Tile.SetImages(_images);
        Tile.Setup(_indexes[0]);
        Tile.GetComponent<CanvasGroup>().alpha = 0f;
        _tiles.Add(Tile);
        for (int i = 1; i < _info.MaxTileOnBoard; ++i)
        {
            var aTile = Instantiate(Tile);
            aTile.transform.SetParent(this.transform, false);
            _tiles.Add(aTile);
        }

        for (int i = 0; i < _info.MaxTileOnBoard; ++i)
        {
            var aTile = _tiles[i];
            aTile.GetComponent<CanvasGroup>().alpha = 1f;
            aTile.SetImages(_images);
            aTile.Setup(_indexes[i]);
            yield return new WaitForSeconds(0.015f);
        }
    }

    public void Update()
    {
        if (_lastTimeCorrectTileInSeconds > 0 && Time.realtimeSinceStartup - _lastTimeCorrectTileInSeconds >= 2f)
        {
            findTileOfIndex(NextTileIndex).Hint();
            _lastTimeCorrectTileInSeconds = Time.realtimeSinceStartup;

            _replayEvents.Add(new ReplayEvent
            {
                TimeInSeconds = (Time.realtimeSinceStartup - _startTimeInSeconds) / 1f,
                Type = 2,
            });
        }
    }

    public void TileClick(Tile tile)
    {
        if (tile.Index == NextTileIndex)
        {
            PrevTile.Set(tile.Index);
            _lastTimeCorrectTileInSeconds = Time.realtimeSinceStartup;

            tile.Correct();

            _replayEvents.Add(new ReplayEvent
            {
                TimeInSeconds = (Time.realtimeSinceStartup - _startTimeInSeconds) / 1f,
                Type = 1,
            });

            if (++NextTileIndex >= _info.End)
            {
                _lastTimeCorrectTileInSeconds = 0;
                Game.Win(generateReplayData());
            }
            else if (_maxOnBoard < _info.End)
            {
                tile.Setup(_indexes[_maxOnBoard++]);
            }
        }
        else
        {
            tile.Incorrect();
            _lastTimeCorrectTileInSeconds = 0f;
            findTileOfIndex(NextTileIndex).Hint();
            Game.Lose();
        }
    }

    private void generateIndexes()
    {
        if (_info.FixedIndexes != null && _info.FixedIndexes.Count > 0)
        {
            _indexes.AddRange(_info.FixedIndexes);
            return;
        }

        int i = 0;
        while (i < _info.End)
        {
            int j = 0;
            for (; j < _info.MaxTileOnBoard && j < (_info.End - i); ++j)
            {
                _indexes.Add(i + j);
            }
            shuffleIndexes(i, i + j - 1);
            i += _info.MaxTileOnBoard;
        }
    }

    private void shuffleIndexes(int startIndex, int endIndex)
    {
        for (int i = startIndex; i <= endIndex; ++i)
        {
            int j = _rand.Next(startIndex, endIndex + 1);
            var tmp = _indexes[i];
            _indexes[i] = _indexes[j];
            _indexes[j] = tmp;
        }
    }

    private Tile findTileOfIndex(int index)
    {
        for (int i = 0, n = _tiles.Count; i < n; ++i)
        {
            var tile = _tiles[i];
            if (tile.Index == index)
            {
                return tile;
            }
        }

        return null;
    }

    private ReplayData generateReplayData()
    {
        return new ReplayData()
        {
            BoardInfo = _info,
            Indexes = new List<int>(_indexes),
            Events = new List<ReplayEvent>(_replayEvents),
        };
    }
}