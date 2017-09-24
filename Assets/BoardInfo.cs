using System;
using System.Collections.Generic;

[Serializable]
public class BoardInfo
{
    public int MaxTileOnBoard = 35;
    public int End = 50;
    public int TileCellSize = 50;
    public List<string> Images = null;
    public List<int> FixedIndexes = null;
}