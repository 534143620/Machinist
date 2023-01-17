using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
   public VisualEffect footStep;
   public void BurstFootStep()
   {    
        //footStep.SendEvent("OnPlay"); //效果与Play相同
        footStep.Play();
   }
}
