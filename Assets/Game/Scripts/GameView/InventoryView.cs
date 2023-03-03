using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class InventoryView : MonoBehaviour,IDragHandler
{
    private RectTransform currentRect;
    private Button_UI btn_Close;
    

    private void Awake() {
        currentRect = GetComponent<RectTransform>();
        transform.Find("Btn_Close").GetComponent<Button_UI>().ClickFunc = () => gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentRect.anchoredPosition += eventData.delta / 2 ;
    }
}
