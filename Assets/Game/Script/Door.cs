using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public float OpenDuration = 5f;
    public float OpenTargetY = -80f;

    private void Awake() {
        StartCoroutine(OpenDoorAnimation());
    }

    IEnumerator OpenDoorAnimation()
    {
        transform.DORotate(new Vector3(transform.eulerAngles.x,OpenTargetY,transform.eulerAngles.z),OpenDuration,RotateMode.Fast);
        yield return null;
    }
}
