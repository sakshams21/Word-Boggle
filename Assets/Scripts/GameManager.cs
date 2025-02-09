using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private TextAsset WordData_TextFile;

    private string[] _wordList;
    private Dictionary<string, int> _wordDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> _usedWordDictionary = new Dictionary<string, int>();

    private List<int> InteractedTiles;

    void Awake()
    {
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
        int randomIndex = Random.Range(0, _wordDictionary.Count);
        string word = _wordList[randomIndex];
        while (_usedWordDictionary.ContainsKey(_wordList[randomIndex]))
        {
            randomIndex = Random.Range(0, _wordDictionary.Count);
        }
        return _wordList[randomIndex];
    }
    #endregion

    #region Tile Manager

    public void StartTile(int id)
    {
        InteractedTiles.Clear();
        InteractedTiles.Add(id);
    }

    public void AddTile(int id)
    {
        if (!InteractedTiles.Contains(id))
        {
            InteractedTiles.Add(id);
        }

    }

    public void EndTile()
    {
        Debug.Log("Tiles touched: " + string.Join(", ", InteractedTiles));
    }

    #endregion



}
