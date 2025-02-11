using UnityEngine;
using System.IO;
using System.Collections.Generic;
using EasyButtons;
using System;


public class ParseLevelData : MonoBehaviour
{

    [SerializeField] private TextAsset LevelData_TextAsset;

    public Root Data;

    [Button]
    private void Test()
    {
        // Debug.Log(LevelData_TextAsset.text);
        // Data = JsonConvert.DeserializeObject<Root>(LevelData_TextAsset.text);
        Data = JsonUtility.FromJson<Root>(LevelData_TextAsset.text);
    }

}


[Serializable]
public class Datum
{
    public int bugCount;
    public int wordCount;
    public int timeSec;
    public int totalScore;
    public Vector2Int gridSize;
    public int levelType;
    public List<GridDatum> gridData;
}
[Serializable]
public class GridDatum
{
    public int tileType;
    public string letter;
}
[Serializable]
public class GridSize
{
    public int x;
    public int y;
}
[Serializable]
public class Root
{
    public List<Datum> data;
}