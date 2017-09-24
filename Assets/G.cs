using AssetBundles;
using System.Collections.Generic;
using UnityEngine;

public static class G
{
    public static BoardInfo BoardInfo = new BoardInfo
    {
        MaxTileOnBoard = 6,
        End = 10,
        TileCellSize = 120,
        Images = new List<string> { "ee.1", "ee.2", "ee.3", "ee.4", "ee.5", "ee.6", "ee.7", "ee.8", "ee.9", "ee.10" },
        FixedIndexes = new List<int> { 0, 4, 1, 2, 3, 5, 7, 8, 9, 6 }
    };

    public static class H
    {
        public static AssetBundleLoadAssetOperation LoadSpriteAsync(string path)
        {
            var parts = path.Split('.');
            return AssetBundleManager.LoadAssetAsync(parts[0], parts[1], typeof(Sprite));
        }
    }
}