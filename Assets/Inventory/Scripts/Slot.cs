using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public int slot_Id;//空格ID 等于 物品ID
    public Item slot_Item;
    public Image slot_Img;
    public Text slot_Num;
    public GameObject ItemInSlot;
    public RectTransform slot_RectTransform;

    public void SetUpSlot(Item item)
    {
        slot_RectTransform.localScale = new Vector3(1,1,1);//修复一个第二次进来缩放的BUG
        if(item == null)
        {
            ItemInSlot.SetActive(false);
            return;
        }
        else
        {
            slot_Img.sprite = item.item_Img;
            slot_Num.text = item.item_Hold.ToString();
            slot_Item = item;
            //slotInfo = item.item_Info;
        }
    }
}
