using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EasyButtons;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
namespace Level
{
    public class LevelTileManager : TileManagerBase
    {
        [SerializeField] private List<TileScript> Tiles_List;
        [SerializeField] private TileScript Tiles_Prefab;
        [SerializeField] private Transform TilesSpawn_Transform;
        [SerializeField] private ParseLevelData Ref_ParseLevelData;

        [SerializeField] private GridLayoutGroup LevelGrid;
        private Dictionary<Vector2Int, TileScript> _usedTiles = new();

        public int TargetTotalWords;

        private int _currentTotalCorrectWords;
        private int _currentLevel = 0;

        private int _maxLevel;

        private void Start()
        {
            LoadLevel(0);
            _maxLevel = Ref_ParseLevelData.GetMaxLevel();
            GameManager.Instance.OnWordCheckSuccess += LevelTargetCheck;
        }

        void OnDestroy()
        {
            GameManager.Instance.OnWordCheckSuccess -= LevelTargetCheck;
        }


        private void LevelTargetCheck()
        {
            _currentTotalCorrectWords++;
            if (_currentTotalCorrectWords >= TargetTotalWords)
            {
                _currentLevel++;
                _currentLevel = Mathf.Clamp(_currentLevel, 0, _maxLevel);
                StartCoroutine(ResetLevel_Coro());

            }
        }

        private IEnumerator ResetLevel_Coro()
        {
            GameManager.Instance.StartLoadingScreen();
            yield return new WaitForSeconds(0.2f);
            ClearLevel();
            yield return new WaitForSeconds(0.2f);
            LoadLevel(_currentLevel);
        }

        private void ClearLevel()
        {
            for (int i = 0; i < Tiles_List.Count; i++)
            {
                Destroy(Tiles_List[i].gameObject);
            }
            Tiles_List.Clear();
        }

        [Button]
        public void LoadLevel(int levelIndex)
        {
            GameManager.Instance.StartLoadingScreen();

            _currentLevel = levelIndex;
            var levelData = Ref_ParseLevelData.GetLevelData(_currentLevel);
            int totalNumberOfTiles = levelData.gridSize.x * levelData.gridSize.y;
            LevelGrid.constraintCount = levelData.gridSize.y;
            int bugCount = 0;
            _currentTotalCorrectWords = 0;
            TargetTotalWords = levelData.wordCount;

            for (int i = 0; i < totalNumberOfTiles; i++)
            {
                TileScript item = Instantiate(Tiles_Prefab, TilesSpawn_Transform);
                item.gameObject.SetActive(true);
                item.TilePos = Vector2Int.one * i;
                item.SetScore(Random.Range(0, 3));
                item.IsAvailable = true;
                if (bugCount < levelData.bugCount)
                {
                    bool isBug = Random.Range(0, 2) == 1;
                    if (isBug)
                        bugCount++;
                    item.Bonus = isBug;
                }
                item.index = i;
                item.SetLetter(levelData.gridData[i].letter);
                Tiles_List.Add(item);
            }
            GameManager.Instance.DisableLoadingScreen();
        }


        public override void StartTile(Vector2Int pos, int index)
        {
            _usedTiles.Clear();
            _usedTiles.TryAdd(pos, Tiles_List[index]);
        }

        public override void AddTile(Vector2Int pos, int index)
        {
            _usedTiles.TryAdd(pos, Tiles_List[index]);
        }

        public override void EndTile()
        {
            string wordToCheck = "";
            int totalScoreOfWord = 0;
            int bugCount = 0;
            foreach (var item in _usedTiles)
            {
                wordToCheck += item.Value.GetLetter();
                totalScoreOfWord += item.Value.ScoreValue;
                if (item.Value.Bonus) bugCount++;
            }

            //Check for validity of word
            GameManager.Instance.WordExistenceCheck(ref wordToCheck, totalScoreOfWord, bugCount);
        }
    }
}