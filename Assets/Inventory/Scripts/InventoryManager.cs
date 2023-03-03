using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Inventory myInventory;
    public GameObject slotGrid;
    //public GameObject slotPrefab;
    public GameObject emptySlot;
    public ItemTooltip itemTooltip;

    public List<GameObject> slots = new List<GameObject>();

    public static Dictionary<string, System.Action<Item>> itemOperations;

    private void Awake() {
        if(instance != null)
            Destroy(this);
        instance = this;
        itemOperations = new Dictionary<string, System.Action<Item>>()
        {
            {"Potion_Red", UsePotionRed},
        };
    }

    private void OnEnable() {
        RefreshItem();
    }

    //刷新背包系统
    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        for (int i = 0; i < instance.myInventory.itemList.Count; i++)
        {
            if(instance.myInventory.itemList[i] != null && instance.myInventory.itemList[i].item_Hold <= 0)
                instance.myInventory.itemList[i] = null;
            //CreateNewItem(instance.myInventory.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slot_Id = i;
            instance.slots[i].GetComponent<Slot>().SetUpSlot(instance.myInventory.itemList[i]);
        }
    }

    public static void UsePotionRed(Item item)
    {
        //纯sb的写法
        GameObject.FindWithTag("Player").GetComponent<Health>().AddHealth(10);
        item.item_Hold -= 1;
    }

    // 根据物品名称获取对应操作函数，并调用
    public static void UseProf(Item item)
    {
        var name = item.item_Name;
        if (itemOperations.ContainsKey(name))
        {
            itemOperations[name](item);
        }
        else
        {
            Debug.Log("不支持该物品操作");
        }
    }
}

