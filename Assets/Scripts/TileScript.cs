using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileScript : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private Image MainTile_Image;
    [SerializeField] private TextMeshProUGUI Letter_Text;
    [SerializeField] private GameObject[] ScoreIndicators_Go;
    [SerializeField] private GameObject Bonus_Go;
    [SerializeField] private GameObject Blocked_Go;
    public Vector2Int TilePos;

    public bool IsAvailable;
    public int index;

    public int ScoreValue { get; private set; }

    private bool _blocked;
    private bool _bonus;

    private void Start()
    {
        GameManagerBase.BaseInstance.OnProcessEnd += ResetSelection;
    }

    private void OnDestroy()
    {
        GameManagerBase.BaseInstance.OnProcessEnd -= ResetSelection;
    }

    #region Pointer Handlers
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_blocked || GameManager_Endless.Instance.IsProcessingWord) return;

        Selected(true);
        GameManagerBase.BaseInstance.Ref_TileManager.StartTile(TilePos, index);
        print(TilePos.x + ":" + TilePos.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_blocked || GameManager_Endless.Instance.IsProcessingWord) return;

        Selected(true);
        GameManagerBase.BaseInstance.Ref_TileManager.AddTile(TilePos, index);
        print(TilePos.x + ":" + TilePos.y);
    }
    #endregion


    private void Selected(bool status)
    {
        MainTile_Image.color = status ? Color.green : Color.white;
    }

    public void Clear()
    {
        Letter_Text.text = "";
        Selected(false);

        foreach (var item in ScoreIndicators_Go)
        {
            item.SetActive(false);
        }

        Bonus_Go.SetActive(false);
        Blocked_Go.SetActive(false);
    }

    private void ResetSelection()
    {
        Selected(false);
    }

    public void SetLetter(string str)
    {
        Letter_Text.text = str;
    }

    public string GetLetter()
    {
        return Letter_Text.text;
    }

    public void SetScore(int scoreValue)
    {
        ScoreValue = scoreValue;
        for (int i = 0; i < scoreValue; i++)
        {
            ScoreIndicators_Go[i].SetActive(true);
        }
    }

}
