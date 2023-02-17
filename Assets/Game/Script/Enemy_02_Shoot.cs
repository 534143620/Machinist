using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_02_Shoot : MonoBehaviour
{
    public GameObject DamageOrb;
    public Transform ShootingPoint;
    private Character cc;
    private void Awake() {
        cc = GetComponent<Character>();
    }
    public void ShootTheDamageOrb()
    {
        Instantiate(DamageOrb,ShootingPoint.position,Quaternion.LookRotation(ShootingPoint.forward));
    }

    void Update()
    {
        if(cc != null)
        {
            cc.RotateToTarget();
        }
    }
}
