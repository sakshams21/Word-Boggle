using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.IsProcessingWord) return;
        GameManager.Instance.IsDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.IsProcessingWord) return;
        GameManager.Instance.IsDragging = false;
        GameManager.Instance.ProcessWord_Start();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.Instance.IsProcessingWord) return;
        GameManager.Instance.IsDragging = false;
        GameManager.Instance.ProcessWord_Start();
    }
}
