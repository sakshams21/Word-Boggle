using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager_Endless.Instance.IsProcessingWord) return;
        GameManager_Endless.Instance.IsDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager_Endless.Instance.IsProcessingWord) return;
        GameManager_Endless.Instance.IsDragging = false;
        GameManager_Endless.Instance.ProcessWord_Start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager_Endless.Instance.IsProcessingWord) return;
        GameManager_Endless.Instance.IsDragging = false;
        GameManager_Endless.Instance.ProcessWord_Start();
    }
}
