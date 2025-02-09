using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TileManager Ref_TileManager;
    public UserInterfaceManager Ref_UserInterfaceManager;

    [SerializeField] private TextAsset WordData_TextFile;

    public event Action OnProcessEnd;


    private string[] _wordList;
    private Dictionary<string, int> _wordDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> _usedWordDictionary = new Dictionary<string, int>();

    public bool IsDragging = true;
    public bool IsProcessingWord = false;


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


    #region Word Manager


    private void LoadWords()
    {
        _wordList = WordData_TextFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < _wordList.Length; i++)
        {
            _wordDictionary.TryAdd(_wordList[i].Trim().ToUpper(), i);
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
        string word = _wordList[randomIndex];
        while (_usedWordDictionary.ContainsKey(_wordList[randomIndex]))
        {
            randomIndex = UnityEngine.Random.Range(0, _wordDictionary.Count);
        }
        return _wordList[randomIndex];
    }
    #endregion


    public void ProcessWord_Start()
    {
        Ref_TileManager.EndTile();
        IsProcessingWord = true;
        Invoke(nameof(ProcessWord_End), 2f);
    }

    public void ProcessWord_End()
    {
        IsProcessingWord = false;
        OnProcessEnd?.Invoke();
    }



}
