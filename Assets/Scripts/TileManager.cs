using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;
public class TileManager : MonoBehaviour
{
    [SerializeField] private TileScript[] Tiles_Go;

    private List<Vector2Int> InteractedTiles = new List<Vector2Int>();

    private Dictionary<Vector2Int, TileScript> TilesData = new();
    private Dictionary<Vector2Int, TileScript> UsedTiles = new();

    private List<int> _interactedTiles = new();

    public Vector2Int startingPos_TEST;
    public string WORD_TEST;

    private void Awake()
    {
        MapToPositionToTiles();
    }
    private void MapToPositionToTiles()
    {
        int count = 0;
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                Tiles_Go[count].TilePos = new(x, y);
                Tiles_Go[count].index = count;
                //Tiles_Go[count].SetLetter((char)('A' + Random.Range(0, 26)));
                Tiles_Go[count].SetLetter(x + ":" + y);
                TilesData[new Vector2Int(x, y)] = Tiles_Go[count];
                count++;
            }
        }
    }

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
        string str = "";

        foreach (var item in UsedTiles)
        {
            str += item.Value.GetLetter();
        }
        print(str);
    }

    public void CheckWord()
    {

    }


    [EasyButtons.Button]
    public void PlaceRandomWord()
    {
        string randomWord = GameManager.Instance.PickRandomWord();
        print(randomWord);
        int randomTileIndex = -10;


        do { randomTileIndex = Random.Range(0, 16); }
        while (Tiles_Go[randomTileIndex].IsAvailable);

        Vector2Int startPos = Tiles_Go[randomTileIndex].TilePos;
        // print("Start Tile:" + startPos.x + ":" + startPos.y);
        Tiles_Go[randomTileIndex].IsAvailable = false;
        int randomDirection = Random.Range(0, 4);//0:right, 1:top, 2:left , 3:down
        print("CAN? :" + CanPlaceWord2(ref WORD_TEST, startingPos_TEST, randomDirection));
        // int wordLength = randomWord.Length;
        // while (wordLength > 0)
        // {
        //     int randomDirection = Random.Range(0, 4);//0:right, 1:top, 2:left , 3:down
        //     print("Direction: " + randomDirection);
        //     (int, bool) stepData = CanPlaceWord(ref randomWord, startPos, randomDirection);
        //     if (stepData.Item2 && stepData.Item1 > 0)
        //     {
        //         PlaceWord(ref randomWord, startPos, randomDirection, stepData.Item1);
        //         wordLength -= stepData.Item1;
        //     }
        // }

    }



    /// <summary>
    /// This method checks if the connected word length tiles are free or not, if not pick another word 
    /// </summary>
    /// <param name="word"></param>
    /// <param name="startPo"></param>
    /// <param name="initialDirection"></param>
    /// <returns></returns>

    private bool CanPlaceWord(ref string word, Vector2Int startPo, int initialDirection)
    {
        int intial = initialDirection;
        //4 is used because i am taking only 4 directions for now, can be changed to 8 if counting all the diagonal directions

        int increament_x = (initialDirection == 0) ? 1 : (initialDirection == 2) ? -1 : 0;
        int increament_y = (initialDirection == 1) ? -1 : (initialDirection == 3) ? 1 : 0;
        int remainingWord = 0;

        for (int i = 0; i < word.Length;)
        {
            int new_X = startPo.x + increament_x * i;
            int new_Y = startPo.y + increament_y * i;

            if (new_X < 0 || new_X >= 4 || new_Y < 0 || new_Y >= 4 || UsedTiles.ContainsKey(new Vector2Int(new_X, new_Y)))
            {
                i--;
                remainingWord -= i;
                int newDirection = CycleValue(initialDirection++, 0, 3);
                //means a full cycle has been done and no path found
                if (intial == newDirection)
                    return false;
                increament_x = (newDirection == 0) ? 1 : (newDirection == 2) ? -1 : 0;
                increament_y = (newDirection == 1) ? -1 : (newDirection == 3) ? 1 : 0;
                startPo.x = startPo.x + increament_x * i;
                startPo.y = startPo.y + increament_y * i;
            }
            else
            {
                i++;
                print("nEXT Tile:" + new_X + ":" + new_Y);
            }
        }
        return true;
    }

    private bool CanPlaceWord2(ref string word, Vector2Int startPo, int direction)
    {
        int intial = direction;

        int increament_x = (direction == 0) ? 1 : (direction == 2) ? -1 : 0;
        int increament_y = (direction == 1) ? -1 : (direction == 3) ? 1 : 0;
        int remainingWord = word.Length;
        int count = 0;
        List<Vector2Int> path = new();
        Vector2Int newPos = Vector2Int.zero;
        int maxTries = 20;

        print($"Starting Path:({startPo})");

        while (remainingWord >= 1)
        {
            if (maxTries <= 0)
            {
                Debug.Log("PATH= " + string.Join(", ", path));
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
                print("direction Change: " + direction);

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

        Debug.Log("PATH= " + string.Join(", ", path));
        return false;
    }

    private void PlaceWord(ref string word, Vector2Int startPo, int direction, int noOfSteps)
    {
        int increament_x = (direction == 0) ? 1 : (direction == 2) ? -1 : 0;
        int increament_y = (direction == 1) ? -1 : (direction == 3) ? 1 : 0;

        Vector2Int newPos = Vector2Int.zero;
        for (int i = 0; i < noOfSteps; i++)
        {
            newPos.x = startPo.x + increament_x * i;
            newPos.y = startPo.y + increament_y * i;
            print("nEXT Tile:" + newPos.x + ":" + newPos.y);
            TilesData[newPos].SetLetter(word[i].ToString());
            UsedTiles.TryAdd(newPos, TilesData[newPos]);
        }
    }

    private int CycleValue(int current, int min, int max)
    {
        if (current >= max) return min;
        return current + 1;
    }

}