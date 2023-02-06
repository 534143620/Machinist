using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    private void OnEnable() {
        EventHandler.OpenTheDoor += OnCallOpenTheDoorEvent;
    }
    private void OnDisable() {
        EventHandler.OpenTheDoor -= OnCallOpenTheDoorEvent;
    }
    public float OpenDuration = 5f;
    public float OpenTargetY = -80f;

    public void OnCallOpenTheDoorEvent()
    {
        OpenDoorAnimation();
    }

    public void OpenDoorAnimation()
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x,OpenTargetY,transform.eulerAngles.z),OpenDuration,RotateMode.Fast);
    }
}
