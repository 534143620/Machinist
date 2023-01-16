using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
   public VisualEffect footStep;

   public void Update_FootStep(bool state)
   {
        if (state)
            footStep.Play();
        else
            footStep.Stop();
   }
}
