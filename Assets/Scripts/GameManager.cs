using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Script References")]
    public static GameManager Instance;
    public TileManager Ref_TileManager;
    public UserInterfaceManager Ref_UserInterfaceManager;


    [SerializeField] private TextAsset WordData_TextFile;

    #region Events
    public event Action OnProcessEnd;
    public event Action OnWordCheckSuccess;
    #endregion

    private string[] _wordList;
    private Dictionary<string, int> _wordDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> _usedWordDictionary = new Dictionary<string, int>();

    public bool IsDragging = true;
    public bool IsProcessingWord { get; private set; }

    public int TotalScore { get; private set; }


    private void Awake()
    {
        IsProcessingWord = false;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWords();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Ref_TileManager.MapToPositionToTiles();
    }


    #region Word Manager


    private void LoadWords()
    {
        _wordList = WordData_TextFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < _wordList.Length; i++)
        {
            _wordDictionary.TryAdd(_wordList[i].Trim(), i);
        }
    }

    public bool IsWordValid(string word)
    {
        return _wordDictionary.ContainsKey(word);
        //add the word to the usedone so its not picked
    }

    public string PickRandomWord()
    {
        int randomIndex = UnityEngine.Random.Range(0, _wordDictionary.Count);
        while (_usedWordDictionary.ContainsKey(_wordList[randomIndex]))
        {
            randomIndex = UnityEngine.Random.Range(0, _wordDictionary.Count);
        }

        _usedWordDictionary.TryAdd(_wordList[randomIndex], randomIndex);
        return _wordList[randomIndex];
    }


    /// <summary>
    /// Checks word in the dictionary and notifies the UI
    /// </summary>
    /// <param name="word"></param>
    public void WordExistenceCheck(ref string word, int wordScore)
    {
        bool wordStatus = _wordDictionary.ContainsKey(word);
        Ref_UserInterfaceManager.WordDisplay(ref word, wordStatus);
        //update UI for score

        if (wordStatus)
        {
            TotalScore += wordScore;
            OnWordCheckSuccess?.Invoke();
            Ref_UserInterfaceManager.UpdateScore(TotalScore);
        }
    }
    #endregion


    public void ProcessWord_Start()
    {
        Ref_TileManager.EndTile();
        IsProcessingWord = true;
        Invoke(nameof(ProcessWord_End), 0.2f);
    }

    public void ProcessWord_End()
    {
        IsProcessingWord = false;
        OnProcessEnd?.Invoke();
        Ref_UserInterfaceManager.Reset_WordDisplay();
    }
}
