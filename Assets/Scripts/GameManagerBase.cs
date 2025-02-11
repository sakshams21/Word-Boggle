using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBase : MonoBehaviour
{
    public static GameManagerBase BaseInstance;
    public TileManagerBase Ref_TileManager;
    public UserInterfaceManager Ref_UserInterfaceManager;
    [SerializeField] private TextAsset WordData_TextFile;

    #region Events

    public event Action OnProcessEnd;
    public event Action OnWordCheckSuccess;

    #endregion

    protected string[] WordList;
    protected readonly Dictionary<string, int> WordDictionary = new Dictionary<string, int>();
    

    public bool IsDragging = true;
    public bool IsProcessingWord { get; private set; }

    public int TotalScore { get; private set; }


    protected void Initialize()
    {
        BaseInstance = this;
        IsProcessingWord = false;
        LoadWords();
    }


    #region Word Manager

    protected void LoadWords()
    {
        WordList = WordData_TextFile.text.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < WordList.Length; i++)
        {
            WordDictionary.TryAdd(WordList[i].Trim(), i);
        }
    }


    /// <summary>
    /// Checks word in the dictionary and notifies the UI
    /// </summary>
    /// <param name="word"></param>
    /// <param name="wordScore"></param>
    public void WordExistenceCheck(ref string word, int wordScore)
    {
        bool wordStatus = WordDictionary.ContainsKey(word);
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