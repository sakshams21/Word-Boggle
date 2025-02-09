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

    private void LoadWords()
    {
        _wordList = WordData_TextFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < _wordList.Length; i++)
        {
            _wordDictionary.TryAdd(_wordList[i].Trim().ToUpper(), i);
        }
    }

    public bool isWordValid(string word)
    {
        return _wordDictionary.ContainsKey(word);
    }

    public string PickRandomWord()
    {
        int randomIndex = Random.Range(0, _wordDictionary.Count);
        while (_usedWordDictionary.ContainsKey(_wordList[randomIndex]))
        {
            randomIndex = Random.Range(0, _wordDictionary.Count);
        }

        return _wordList[randomIndex];
    }

}
