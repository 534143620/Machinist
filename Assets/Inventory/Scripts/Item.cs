using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="Inventory/New Item")]
public class Item : ScriptableObject
{
    public string item_Name;
    public Sprite item_Img;
    public int item_Hold;
    [TextArea]
    public string item_Info;
    public int item_value;

    public bool isEquip;
}
