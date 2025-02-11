using System;
using System.Collections.Generic;
using EasyButtons;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;
using Random = UnityEngine.Random;
public class TileManager : MonoBehaviour
{
    [SerializeField] protected TileScript[] Tiles_Go;

    protected Dictionary<Vector2Int, TileScript> TilesData = new();
    protected Dictionary<Vector2Int, TileScript> UsedTiles = new();

    protected virtual void Initial()
    {
        GameManager.Instance.OnProcessEnd += ResetSelection;
    }


    protected virtual void Disable()
    {
        GameManager.Instance.OnProcessEnd -= ResetSelection;
    }

    public void MapToPositionToTiles()
    {
        int count = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                Tiles_Go[count].TilePos = new(x, y);
                Tiles_Go[count].index = count;
                Tiles_Go[count].IsAvailable = true;
                Tiles_Go[count].SetScore(Random.Range(0, 3));
                Tiles_Go[count].SetLetter(((char)('A' + Random.Range(0, 26))).ToString());
                TilesData[new Vector2Int(x, y)] = Tiles_Go[count];
                count++;
            }
        }

        for (int i = 0; i < 2; i++)
        {
            PlaceRandomWord();
        }

    }

    #region User Interacted Methods

    public void StartTile(Vector2Int pos, int index)
    {
        UsedTiles.Clear();
        UsedTiles.TryAdd(pos, Tiles_Go[index]);
    }

    public void AddTile(Vector2Int pos, int index)
    {
        UsedTiles.TryAdd(pos, Tiles_Go[index]);
    }

    public void EndTile()
    {
        string wordToCheck = "";
        int totalScoreOfWord = 0;
        foreach (var item in UsedTiles)
        {
            wordToCheck += item.Value.GetLetter();
            totalScoreOfWord += item.Value.ScoreValue;
        }

        wordToCheck = wordToCheck.ToLower();

        //Check for validity of word
        GameManager.Instance.WordExistenceCheck(ref wordToCheck, totalScoreOfWord);
    }



    private void ResetSelection()
    {
        UsedTiles.Clear();
    }

    #endregion

    #region Word Placement
    /// <summary>
    /// Keep calling this method till we have atleast X words int the grid
    /// where X will be the target for that level
    /// </summary>
    [Button]
    public bool PlaceRandomWord()
    {

        string randomWord = GameManager.Instance.PickRandomWord().ToUpper();

        if (!IsEnoughSpaceAvailableOnGrid(randomWord.Length)) return false;

        int randomTileIndex;
        do { randomTileIndex = Random.Range(0, 16); }
        while (!Tiles_Go[randomTileIndex].IsAvailable);

        Vector2Int startPos = Tiles_Go[randomTileIndex].TilePos;
        Tiles_Go[randomTileIndex].IsAvailable = false;
        int randomDirection = Random.Range(0, 4);

        if (CanPlaceWordInGrid(ref randomWord, startPos, randomDirection, out List<Vector2Int> wordPath))
        {
            PlaceWordInGrid(ref randomWord, ref wordPath);
            return true;
        }
        else
        {
            return false;
        }

    }



    /// <summary>
    /// This method checks if the connected word length tiles are free or not, if not pick another word 
    /// </summary>
    /// <param name="word">the random word selected</param>
    /// <param name="startPo"></param>
    /// <param name="initialDirection">0:right, 1:top, 2:left , 3:down(starts from right and goes anti clockwise)</param>
    /// <returns></returns>
    private bool CanPlaceWordInGrid(ref string word, Vector2Int startPo, int direction, out List<Vector2Int> path)
    {
        int increament_x = (direction == 0) ? 1 : (direction == 2) ? -1 : 0;
        int increament_y = (direction == 1) ? -1 : (direction == 3) ? 1 : 0;
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

            newPos.x = startPo.x + increament_x * count;
            newPos.y = startPo.y + increament_y * count;

            if (newPos.x < 0 || newPos.x >= 4 || newPos.y < 0 || newPos.y >= 4 || UsedTiles.ContainsKey(newPos) || path.Contains(newPos))
            {
                //one step backward
                startPo.x = startPo.x + increament_x * (count - 1);
                startPo.y = startPo.y + increament_y * (count - 1);

                //change direction
                direction = CycleValue(direction++, 0, 3);

                //change direction modifiers
                increament_x = (direction == 0) ? 1 : (direction == 2) ? -1 : 0;
                increament_y = (direction == 1) ? -1 : (direction == 3) ? 1 : 0;

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
            TilesData[path[i]].SetLetter(word[i].ToString());
            UsedTiles.TryAdd(path[i], TilesData[path[i]]);
            TilesData[path[i]].IsAvailable = false;
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
        foreach (var item in Tiles_Go)
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