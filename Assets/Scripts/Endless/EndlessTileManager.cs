using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EndlessTileManager : TileManager
{
    private void Start()
    {
        base.Initial();
        GameManager.Instance.OnWordCheckSuccess += AfterWordCheck;
    }

    private void OnDisable()
    {
        base.Disable();
        GameManager.Instance.OnWordCheckSuccess -= AfterWordCheck;
    }

    private void AfterWordCheck()
    {
        //replace all letters with new one
        foreach (var item in UsedTiles)
        {
            item.Value.SetScore(Random.Range(0, 3));
            item.Value.SetLetter(((char)('A' + Random.Range(0, 26))).ToString());
        }
    }
}