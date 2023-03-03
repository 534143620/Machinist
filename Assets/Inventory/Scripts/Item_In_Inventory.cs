using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Item_In_Inventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    public Inventory myInventory;
    private int currentItemID;//当前物品ID

    private const float DOUBLE_CLICL_TIME = 0.2f;
    private float lastClickTime;

    public void Awake() {
        if (transform.GetComponent<Button_UI>() != null) {
            transform.GetComponent<Button_UI>().MouseOverOnceTooltipFunc = () => InventoryManager.instance.itemTooltip.ShowTooltip(
                transform.parent.GetComponent<Slot>().slot_Item
            );
            transform.GetComponent<Button_UI>().MouseOutOnceTooltipFunc = () => InventoryManager.instance.itemTooltip.HideTooltip();
            transform.GetComponent<Button_UI>().ClickFunc = () => DoubleClick();
        }
    }

    private void DoubleClick() {
        float timeSinceLastClick = Time.time - lastClickTime;
        if(timeSinceLastClick <= DOUBLE_CLICL_TIME){
            //双击
            InventoryManager.instance.itemTooltip.HideTooltip();
            InventoryManager.UseProf(transform.parent.GetComponent<Slot>().slot_Item);
            InventoryManager.RefreshItem();
        }
        lastClickTime = Time.time;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.instance.itemTooltip.HideTooltip();
        originalParent = transform.parent;
        currentItemID = originalParent.GetComponent<Slot>().slot_Id;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;//射线阻挡关闭
        transform.GetComponent<Canvas>().overrideSorting = true;
        transform.GetComponent<Canvas>().sortingOrder = 20;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        InventoryManager.instance.itemTooltip.HideTooltip();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventoryManager.instance.itemTooltip.HideTooltip();
        transform.GetComponent<Canvas>().overrideSorting = false;
        var pointerCurrentRaycast = eventData.pointerCurrentRaycast.gameObject;

        if(pointerCurrentRaycast == null)
        {
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }
        if (pointerCurrentRaycast.name == "Img_Item")//判断下面物体名字是：Item Image 那么互换位置
        {
            //pointerCurrentRaycast 蓝药水
            transform.SetParent(pointerCurrentRaycast.transform.parent.parent);
            transform.position = pointerCurrentRaycast.transform.parent.parent.position;
            //itemList的物品存储位置改变
            var temp = myInventory.itemList[currentItemID];
            myInventory.itemList[currentItemID] = myInventory.itemList[pointerCurrentRaycast.GetComponentInParent<Slot>().slot_Id];
            myInventory.itemList[pointerCurrentRaycast.GetComponentInParent<Slot>().slot_Id] = temp;
            //pointerCurrentRaycast.transform.GetComponent<Slot>().slot_Item = temp;
            
            pointerCurrentRaycast.transform.parent.position = originalParent.position;
            pointerCurrentRaycast.transform.parent.SetParent(originalParent);

            //pointerCurrentRaycast.GetComponent<Slot>().slot_Item = myInventory.itemList[currentItemID];

            GetComponent<CanvasGroup>().blocksRaycasts = true;//射线阻挡开启，不然无法再次选中移动的物品
            InventoryManager.RefreshItem();
            return;
        }
        else if(pointerCurrentRaycast.name == "Slot(Clone)")
        {
            //否则直接挂在检测到到Slot下面
            transform.SetParent(pointerCurrentRaycast.transform);
            transform.position = pointerCurrentRaycast.transform.position;
            //itemList的物品存储位置改变
            myInventory.itemList[pointerCurrentRaycast.GetComponent<Slot>().slot_Id] = myInventory.itemList[currentItemID];
            // pointerCurrentRaycast.GetComponent<Slot>().slot_Item = myInventory.itemList[currentItemID];
            if(pointerCurrentRaycast.GetComponent<Slot>().slot_Id != currentItemID)
                myInventory.itemList[currentItemID] = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            InventoryManager.RefreshItem();
            return;
        } else{
            //否则直接挂在检测到到Slot下面
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }


    }
}
