using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Prop : MonoBehaviour
{
    public enum PropType
    {
        Heal,Coin
    }

    public PropType Type;
    public int Value = 20;
    public ParticleSystem  CollectedVFX;

    private void Start() {
        if(Type == PropType.Coin){
           transform.DORotate(new Vector3(0, 360, 0), 2f, RotateMode.WorldAxisAdd).SetLoops(-1,LoopType.Restart);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<Character>().PickUpItem(this);
            if(CollectedVFX != null)
                Instantiate(CollectedVFX,transform.position,Quaternion.identity);
            SoundManager.PlaySound(SoundManager.Sound.PropCollect);
            transform.DOKill();
            Destroy(gameObject);
        }
    }
}
