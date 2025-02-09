using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    public int TileId;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameManager.Instance.StartTile(TileId);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameManager.Instance.AddTile(TileId);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameManager.Instance.EndTile();
    }
}
