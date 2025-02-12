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

    private string[] _wordList;
    private readonly Dictionary<string, int> _wordDictionary = new Dictionary<string, int>();

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

    private void LoadWords()
    {
        _wordList = WordData_TextFile.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < _wordList.Length; i++)
        {
            _wordList[i] = _wordList[i].Trim().ToUpper();
            _wordDictionary.TryAdd(_wordList[i], i);
        }
    }


    /// <summary>
    /// Checks word in the dictionary and notifies the UI
    /// </summary>
    /// <param name="word">word to check on</param>
    /// <param name="wordScore">total score of all the letters selected</param>
    /// <param name="numOfBugs">total number of bugs found in the selected letters </param>
    public void WordExistenceCheck(ref string word, int wordScore, int numOfBugs)
    {
        bool wordStatus = _wordDictionary.ContainsKey(word) && !_usedWordDictionary.ContainsKey(word);
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

    /// <summary>
    /// This method invokes after finger lift off from the screen
    /// </summary>

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

    /// <summary>
    /// Picks a random word which has not been used before
    /// </summary>
    public string PickRandomWord()
    {
        int randomIndex = UnityEngine.Random.Range(0, _wordDictionary.Count);
        while (_usedRandomWordDictionary.ContainsKey(_wordList[randomIndex]))
        {
            randomIndex = UnityEngine.Random.Range(0, _wordDictionary.Count);
        }

        _usedRandomWordDictionary.TryAdd(_wordList[randomIndex], randomIndex);
        return _wordList[randomIndex];
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