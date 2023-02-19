using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOrb : MonoBehaviour
{
    public int Damage = 10;
    public float Speed = 2.0f;
    public ParticleSystem HitVFX;
    private Rigidbody _rb;
    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        _rb.MovePosition(transform.position + transform.forward * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        Character cc = other.gameObject.GetComponent<Character>();
        if(cc != null && cc.isPlayer)
        {
            cc.ApplyDamage(Damage);
        }
        Instantiate(HitVFX,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
