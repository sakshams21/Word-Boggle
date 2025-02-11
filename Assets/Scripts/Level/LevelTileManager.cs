using System.Collections.Generic;
using EasyButtons;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LevelTileManager : TileManagerBase
    {
        [SerializeField] private List<TileScript> Tiles_List;
        [SerializeField] private TileScript Tiles_Prefab;
        [SerializeField]private Transform TilesSpawn_Transform;
        [SerializeField] private ParseLevelData Ref_ParseLevelData;

        [SerializeField] private GridLayoutGroup LevelGrid;
        private Dictionary<Vector2Int, TileScript> _usedTiles = new();
        [Button]
        public void LoadLevel(int levelIndex)
        {
            var levelData = Ref_ParseLevelData.GetLevelData(levelIndex);
            int totalNumberOfTiles = levelData.gridSize.x * levelData.gridSize.y;
            LevelGrid.constraintCount = levelData.gridSize.y;

            for (int i = 0; i < totalNumberOfTiles; i++)
            {
                var item = Instantiate(Tiles_Prefab, TilesSpawn_Transform);
                item.gameObject.SetActive(true);
                item.IsAvailable = true;
                item.index = i;
                item.SetLetter(levelData.gridData[i].letter);
                Tiles_List.Add(item);
            }

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
            foreach (var item in _usedTiles)
            {
                wordToCheck += item.Value.GetLetter();
                totalScoreOfWord += item.Value.ScoreValue;
            }

            wordToCheck = wordToCheck.ToLower();

            //Check for validity of word
            GameManager_Endless.Instance.WordExistenceCheck(ref wordToCheck, totalScoreOfWord);
        }
    }
}