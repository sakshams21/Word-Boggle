using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using Random = UnityEngine.Random;
public class EndlessTileManager : TileManagerBase
{
    [SerializeField] protected TileScript[] Tiles_Go;

    private Dictionary<Vector2Int, TileScript> _tilesData = new();
    private Dictionary<Vector2Int, TileScript> _usedTiles = new();

    private void Start()
    {
        GameManager.Instance.OnProcessEnd += ResetSelection;
        GameManager.Instance.OnWordCheckSuccess += AfterWordCheck;
        MapToPositionToTiles();
    }


    private void OnDisable()
    {
        GameManager.Instance.OnProcessEnd -= ResetSelection;
        GameManager.Instance.OnWordCheckSuccess -= AfterWordCheck;
    }

    private void AfterWordCheck()
    {
        //replace all letters with new one
        foreach (KeyValuePair<Vector2Int, TileScript> item in _usedTiles)
        {
            item.Value.SetScore(Random.Range(0, 3));
            item.Value.SetLetter(((char)('A' + Random.Range(0, 26))).ToString());
        }
    }

    public void MapToPositionToTiles()
    {
        int count = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                var item = Tiles_Go[count];
                item.TilePos = new(x, y);
                item.index = count;
                item.IsAvailable = true;
                item.SetScore(Random.Range(0, 3));
                item.SetLetter(((char)('A' + Random.Range(0, 26))).ToString());
                _tilesData[new Vector2Int(x, y)] = item;
                count++;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            PlaceRandomWord();
        }

        GameManager.Instance.DisableLoadingScreen();
    }

    #region User Interacted Methods

    public override void StartTile(Vector2Int pos, int index)
    {
        _usedTiles.Clear();
        _usedTiles.TryAdd(pos, Tiles_Go[index]);
    }

    public override void AddTile(Vector2Int pos, int index)
    {
        _usedTiles.TryAdd(pos, Tiles_Go[index]);
    }

    public override void EndTile()
    {
        string wordToCheck = "";
        int totalScoreOfWord = 0;
        foreach (var item in _usedTiles)
        {
            wordToCheck += item.Value.GetLetter();
            totalScoreOfWord += item.Value.ScoreValue;
        }

        //Check for validity of word
        GameManager.Instance.WordExistenceCheck(ref wordToCheck, totalScoreOfWord, 0);
    }



    private void ResetSelection()
    {
        _usedTiles.Clear();
    }

    #endregion

    #region Word Placement
    /// <summary>
    /// Keep calling this method till we have at least X words int the grid
    /// where X will be the target for that level
    /// </summary>
    [Button]
    private void PlaceRandomWord()
    {
        string randomWord = GameManager.Instance.PickRandomWord();

        if (!IsEnoughSpaceAvailableOnGrid(randomWord.Length)) return;

        int randomTileIndex;
        do { randomTileIndex = Random.Range(0, 16); }
        while (!Tiles_Go[randomTileIndex].IsAvailable);

        Vector2Int startPos = Tiles_Go[randomTileIndex].TilePos;
        Tiles_Go[randomTileIndex].IsAvailable = false;
        int randomDirection = Random.Range(0, 4);

        if (CanPlaceWordInGrid(ref randomWord, startPos, randomDirection, out List<Vector2Int> wordPath))
        {
            PlaceWordInGrid(ref randomWord, ref wordPath);
        }
    }


    /// <summary>
    /// This method checks if the connected word length tiles are free or not, if not pick another word 
    /// </summary>
    /// <param name="word">the random word selected</param>
    /// <param name="startPo"></param>
    /// <param name="direction"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    private bool CanPlaceWordInGrid(ref string word, Vector2Int startPo, int direction, out List<Vector2Int> path)
    {
        int incrementX = (direction == 0) ? 1 : (direction == 2) ? -1 : 0;
        int incrementY = (direction == 1) ? -1 : (direction == 3) ? 1 : 0;
        int remainingWord = word.Length;
        int count = 0;
        Vector2Int newPos = Vector2Int.zero;
        int maxTries = 20;
        path = new List<Vector2Int>();

        while (remainingWord >= 1)
        {
            if (maxTries <= 0)
            {
                return false;
            }

            newPos.x = startPo.x + incrementX * count;
            newPos.y = startPo.y + incrementY * count;

            if (newPos.x < 0 || newPos.x >= 4 || newPos.y < 0 || newPos.y >= 4 || _usedTiles.ContainsKey(newPos) || path.Contains(newPos))
            {
                //one step backward
                startPo.x += incrementX * (count - 1);
                startPo.y += incrementY * (count - 1);

                //change direction
                direction = CycleValue(direction, 0, 3);

                //change direction modifiers
                incrementX = (direction == 0) ? 1 : (direction == 2) ? -1 : 0;
                incrementY = (direction == 1) ? -1 : (direction == 3) ? 1 : 0;

                count = 1;
            }
            else
            {
                count++;
                path.Add(newPos);
                remainingWord--;
            }
            maxTries--;
        }

        PlaceWordInGrid(ref word, ref path);
        return true;
    }

    private void PlaceWordInGrid(ref string word, ref List<Vector2Int> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            _tilesData[path[i]].SetLetter(word[i].ToString());
            _usedTiles.TryAdd(path[i], _tilesData[path[i]]);
            _tilesData[path[i]].IsAvailable = false;
        }
    }

    private int CycleValue(int current, int min, int max)
    {
        if (current >= max) return min;
        return current + 1;
    }

    private bool IsEnoughSpaceAvailableOnGrid(int wordCount)
    {
        int availCount = 0;
        foreach (TileScript item in Tiles_Go)
        {
            if (item.IsAvailable)
            {
                availCount++;
            }
        }

        return availCount > wordCount;
    }

    #endregion

}