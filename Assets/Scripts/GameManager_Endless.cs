using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Endless : GameManagerBase
{
    public static GameManager_Endless Instance;
   
    [Header("Script References")]
    public EndlessTileManager Ref_EndlessTileManager;

    private Dictionary<string, int> _usedWordDictionary = new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            base.Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Ref_EndlessTileManager.MapToPositionToTiles();
    }


    #region Word Manager

    public string PickRandomWord()
    {
        int randomIndex = UnityEngine.Random.Range(0, WordDictionary.Count);
        while (_usedWordDictionary.ContainsKey(WordList[randomIndex]))
        {
            randomIndex = UnityEngine.Random.Range(0, WordDictionary.Count);
        }

        _usedWordDictionary.TryAdd(WordList[randomIndex], randomIndex);
        return WordList[randomIndex];
    }
    #endregion
}
