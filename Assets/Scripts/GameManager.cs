using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TileManagerBase Ref_TileManager;
    public UserInterfaceManager Ref_UserInterfaceManager;
    [SerializeField] private TextAsset WordData_TextFile;

    [SerializeField] private GameObject Loading_Go;
    [SerializeField] private GameObject PauseMenu_Go;

    #region Events

    public event Action OnProcessEnd;
    public event Action OnWordCheckSuccess;

    #endregion

    protected string[] WordList;
    protected readonly Dictionary<string, int> WordDictionary = new Dictionary<string, int>();

    private Dictionary<string, int> _usedRandomWordDictionary = new Dictionary<string, int>();

    private Dictionary<string, int> _usedWordDictionary = new Dictionary<string, int>();

    public bool IsDragging = true;
    public bool IsProcessingWord { get; private set; }

    public int TotalScore { get; private set; }


    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        IsProcessingWord = false;
        LoadWords();
        StartLoadingScreen();
    }

    void OnDestroy()
    {
        CancelInvoke(nameof(ProcessWord_End));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartingMenu()
    {
        StartLoadingScreen();
        SceneManager.LoadSceneAsync(0);
    }

    #region Word Manager

    protected void LoadWords()
    {
        WordList = WordData_TextFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < WordList.Length; i++)
        {
            WordList[i] = WordList[i].Trim().ToUpper();
            WordDictionary.TryAdd(WordList[i], i);
        }
    }


    /// <summary>
    /// Checks word in the dictionary and notifies the UI
    /// </summary>
    /// <param name="word"></param>
    /// <param name="wordScore"></param>
    public void WordExistenceCheck(ref string word, int wordScore, int numOfBugs)
    {
        bool wordStatus = WordDictionary.ContainsKey(word) && !_usedWordDictionary.ContainsKey(word);
        Ref_UserInterfaceManager.WordDisplay(ref word, wordStatus);
        //update UI for score

        if (wordStatus)
        {
            TotalScore += wordScore;
            TotalScore += numOfBugs * 5;
            OnWordCheckSuccess?.Invoke();
            _usedWordDictionary.TryAdd(word, TotalScore);
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


    public string PickRandomWord()
    {
        int randomIndex = UnityEngine.Random.Range(0, WordDictionary.Count);
        while (_usedRandomWordDictionary.ContainsKey(WordList[randomIndex]))
        {
            randomIndex = UnityEngine.Random.Range(0, WordDictionary.Count);
        }

        _usedRandomWordDictionary.TryAdd(WordList[randomIndex], randomIndex);
        return WordList[randomIndex];
    }

    public void StartLoadingScreen()
    {
        Loading_Go.SetActive(true);
    }

    public void DisableLoadingScreen()
    {
        DOVirtual.DelayedCall(1f, () => { Loading_Go.SetActive(false); });

    }
}