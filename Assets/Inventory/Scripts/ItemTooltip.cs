using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using System;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private RectTransform canvasRectTransform;

    private Image Img_Item;
    private Text Txt_Name;
    private Text Txt_Info;
    private Text Txt_Value;
    private RectTransform Img_Tooltip;

    void Awake()
    {
        Img_Item = transform.Find("Img_Item").GetComponent<Image>();
        Txt_Name = transform.Find("Txt_Name").GetComponent<Text>();
        Txt_Info = transform.Find("Txt_Info").GetComponent<Text>();
        Txt_Value = transform.Find("Txt_Value").GetComponent<Text>();
        Img_Tooltip = transform.GetComponent<RectTransform>();
        HideTooltip();
    }

    void Update()
    {
        // Vector2 localPoint;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        // transform.localPosition = localPoint;

        //Canvans无需设置为Screen Space - Camera
        Vector2 mousePos = Input.mousePosition;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePos, mainCamera, out localPoint);
        transform.localPosition = localPoint;

        //必须修改锚点，不然会导致闪烁的BUG
        Vector2 anchoredPosition = transform.GetComponent<RectTransform>().anchoredPosition;
        if (anchoredPosition.x + Img_Tooltip.rect.width > canvasRectTransform.rect.width) {
            anchoredPosition.x = canvasRectTransform.rect.width - Img_Tooltip.rect.width;
        }
        if (anchoredPosition.y - Img_Tooltip.rect.height > canvasRectTransform.rect.height) {
            anchoredPosition.y = canvasRectTransform.rect.height + Img_Tooltip.rect.height;
        }
        transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
    }

    public void ShowTooltip(Item item) {
        gameObject.SetActive(true);
        transform.SetAsLastSibling();
        Img_Item.sprite = item.item_Img;
        Txt_Name.text = item.item_Name;
        Txt_Info.text = item.item_Info;
        Txt_Value.text = "Value = " + item.item_value;
        Update();
    }

    public void HideTooltip() {
        gameObject.SetActive(false);
    }
}
