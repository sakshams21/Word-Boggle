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


    private bool _bonus;
    private bool _blocked;

    public (int x, int y) TilePos;
    public int index;

    private void Start()
    {
        GameManager.Instance.OnProcessEnd += Clear;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnProcessEnd -= Clear;
    }

    #region Pointer Handlers
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_blocked || GameManager.Instance.IsProcessingWord) return;

        Selected(true);
        GameManager.Instance.Ref_TileManager.StartTile(TilePos, index);
        print(TilePos.x + ":" + TilePos.y);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_blocked || GameManager.Instance.IsProcessingWord) return;

        Selected(true);
        GameManager.Instance.Ref_TileManager.AddTile(TilePos, index);
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

    public void SetLetter(char c)
    {
        Letter_Text.text = c.ToString();
    }

    public string GetLetter()
    {
        return Letter_Text.text;
    }


}
