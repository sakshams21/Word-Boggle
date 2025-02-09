using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class TileManager : MonoBehaviour
{
    [SerializeField] private TileScript[] Tiles_Go;

    private List<(int, int)> InteractedTiles = new List<(int, int)>();

    private List<int> _interactedTiles = new();

    private void Awake()
    {
        MapToPositionToTiles();
    }
    private void MapToPositionToTiles()
    {
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                Tiles_Go[count].TilePos = new(i, j);
                Tiles_Go[count].index = count;
                Tiles_Go[count].SetLetter((char)('A' + Random.Range(0, 26)));
                count++;
            }
        }
    }

    public void StartTile((int x, int y) id, int index)
    {
        InteractedTiles.Clear();
        InteractedTiles.Add(id);

        _interactedTiles.Clear();
        _interactedTiles.Add(index);
    }

    public void AddTile((int x, int y) id, int index)
    {
        if (!InteractedTiles.Contains(id))
        {
            InteractedTiles.Add(id);
        }

        if (!_interactedTiles.Contains(index))
        {
            _interactedTiles.Add(index);
        }
    }

    public void EndTile()
    {
        string str = "";

        foreach (var item in _interactedTiles)
        {
            str += Tiles_Go[item].GetLetter();
        }
        print(str);
    }

    public void CheckWord()
    {

    }
}