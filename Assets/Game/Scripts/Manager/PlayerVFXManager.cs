using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect footStep;

    public ParticleSystem Blade01;
    public ParticleSystem Blade02;
    public ParticleSystem Blade03;

    public VisualEffect Slash;
    public VisualEffect HealVFX;

    public ParticleSystem[] DancingSystem;

    public void Update_FootStep(bool state)
    {
        if (state)
            footStep.Play();
        else
        {
            footStep.Stop();
        }

    }

    public void isPlayerDancingVFX(bool state)
    {
        foreach (ParticleSystem system in DancingSystem)
        {
            if (state)
            {
                system.Play();
            }
            else
            {
                system.Stop();
            }
        }
    }

    public void PlayBlade01()
    {
        Blade01.Play();
        SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
    }
    public void PlayBlade02()
    {
        Blade02.Play();
        SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
    }
    public void PlayBlade03()
    {
        Blade03.Play();
        SoundManager.PlaySound(SoundManager.Sound.PlayerAttack);
    }
    private const int BladeCount = 3;

    public void StopBlade()
    {
        for (int i = 1; i <= BladeCount; i++)
        {
            var blade = (ParticleSystem)GetType().GetField($"Blade0{i}").GetValue(this);
            blade.Simulate(0);
            blade.Stop();
        }
    }

    public void PlaySlash(Vector3 pos)
    {
       PlayEffect(Slash, pos);
    }

    public void PlayHealVFX(Vector3 pos)
    {
        PlayEffect(HealVFX,pos);
    }

    public void PlayEffect(VisualEffect effect, Vector3 pos)
    {
        effect.transform.position = pos;
        effect.Play();
    }
}
