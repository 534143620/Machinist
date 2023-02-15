using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMap : MonoBehaviour
{
    public Transform player;
    private void LateUpdate() {
        Vector3 newPostion = player.position;
        newPostion.y = transform.position.y;
        transform.position = newPostion;

        //transform.rotation = Quaternion.Euler(90f,player.eulerAngles.z,0f);
    }
}
