using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
   public VisualEffect footStep;
   public VisualEffect AttackVFX;
   public ParticleSystem BeingHitVFX;
   public void BurstFootStep()
   {    
      //footStep.SendEvent("OnPlay"); //效果与Play相同
      footStep.Play();
   }

   public void PlayAttackVFX()
   {
      AttackVFX.Play();
   }

   public void PlayBeingHitVFX(Vector3 attackerPos)
   {
      Vector3 forceForward = transform.position - attackerPos;
      forceForward.Normalize();
      forceForward.y = 0;
      BeingHitVFX.transform.rotation = Quaternion.LookRotation(forceForward);
      BeingHitVFX.Play();
   }
}
