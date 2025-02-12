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
    private void Awake()
    {
        Data = JsonUtility.FromJson<Root>(LevelData_TextAsset.text);
    }

    public int GetMaxLevel()
    {
        return Data.data.Count;
    }

    public LevelData GetLevelData(int index)
    {
        return Data.data[index];
    }

}


[Serializable]
public class LevelData
{
    public int bugCount;
    public int wordCount;
    public int timeSec;
    public int totalScore;
    public Vector2Int gridSize;
    public int levelType;
    public List<TileData> gridData;
}
[Serializable]
public class TileData
{
    public int tileType;
    public string letter;
}
[Serializable]
public class Root
{
    public List<LevelData> data;
}