using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    private void Start()
    {
        GameManager.Instance.OnWordCheckSuccess += Reset;
    }

    void OnDestroy()
    {
        GameManager.Instance.OnWordCheckSuccess -= Reset;
    }

    private void Reset()
    {
        gameObject.SetActive(false);

        gameObject.SetActive(true);
    }

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
