using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public enum PropType
    {
        Heal,Coin
    }

    public PropType Type;
    public int Value = 20;

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            Destroy(gameObject);
        }
    }
}
