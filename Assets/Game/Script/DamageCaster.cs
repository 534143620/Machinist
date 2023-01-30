using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 伤害施加者
/// </summary>
public class DamageCaster : MonoBehaviour
{
    private Collider _damageCasterCollider;
    public int Damage = 30;
    public string TargetTag;
    private List<Collider> _damagedTargetList;

    private void Awake() {
        _damageCasterCollider = GetComponent<Collider>();
        _damageCasterCollider.enabled = false;
        _damagedTargetList = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == TargetTag &&!_damagedTargetList.Contains(other))
        {
            Character targetCC = other.GetComponent<Character>();
            if(targetCC != null)
            {
                targetCC.ApplyDamage(Damage,transform.parent.position);

                PlayerVFXManager playerVFXManager = transform.parent.GetComponent<PlayerVFXManager>();
                if(playerVFXManager)
                {
                    RaycastHit hit;
                    Vector3 orginlPos = transform.position + ( -_damageCasterCollider.bounds.extents.z) * transform.forward;
                    bool isHit = Physics.BoxCast(orginlPos,_damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation,_damageCasterCollider.bounds.extents.z,1<<6);
                    if(isHit)
                    {
                        playerVFXManager.PlaySlash(hit.point + new Vector3(0,0.5f,0));
                    }

                }
            }

            _damagedTargetList.Add(other);
        }
    }

    public void EnableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = true;

    }

    public void DisableDamageCaster()
    {
        _damagedTargetList.Clear();
        _damageCasterCollider.enabled = false;
    }

    // private void OnDrawGizmos() {
    //     if(_damageCasterCollider == null)
    //          _damageCasterCollider = GetComponent<Collider>();
        
    //     RaycastHit hit;

    //     Vector3 orginlPos = transform.position + ( -_damageCasterCollider.bounds.extents.z) * transform.forward;

    //     bool isHit = Physics.BoxCast(orginlPos,_damageCasterCollider.bounds.extents / 2, transform.forward, out hit, transform.rotation,_damageCasterCollider.bounds.extents.z,1<<6);
        
    //     if(isHit)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawWireSphere(hit.point,0.3f);
    //     }
    // }

}
