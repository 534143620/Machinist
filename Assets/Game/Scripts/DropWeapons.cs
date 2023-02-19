using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapons : MonoBehaviour
{
    public List<GameObject> Weapons;

    public void DropSwords()
    {
        foreach (var item in Weapons)
        {
            item.AddComponent<Rigidbody>();
            item.AddComponent<BoxCollider>();
            item.transform.parent = null;
        }
    }
}
