using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
   public VisualEffect footStep;
   public VisualEffect AttackVFX;
   public ParticleSystem BeingHitVFX;
   public VisualEffect BeingHitSplashVFX;
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

      //敌人血飞溅效果   
      //TODO飞溅的效果可以用对象池来实现，不然敌人一直受伤害,你会一直有新的预制体产生
      Vector3 splashPos = transform.position;
      splashPos.y += 2.0f;
      VisualEffect newSplashVFX = Instantiate(BeingHitSplashVFX,splashPos,Quaternion.identity);
      newSplashVFX.Play();
      Destroy(newSplashVFX.gameObject,5.0f);

   }
}
