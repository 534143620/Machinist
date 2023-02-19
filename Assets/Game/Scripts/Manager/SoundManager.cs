using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.Audio;

public static class SoundManager
{
    public enum Sound{
        PlayerMove,
        PlayerAttack,
        PlayerHit,
        PlayerSlide,
        CollisionWithTheEnemy,
        ButtonEffect1,
        ButtonEffect2,
        PropCollect,
        GameWin,
        GameLost
    }
    private static Dictionary<Sound,float> soundTimerDictionary; //防止没播放完继续播放
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;


    public static void InitSoundManger()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }

    public static void PlaySound(Sound sound){
        if(CanPlaySound(sound)){
            if(oneShotGameObject == null){
                oneShotGameObject = new GameObject("one Shot Sound");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

                AudioMixer mixer = Resources.Load<AudioMixer>("AudioMusic/AudioMixer");
                AudioMixerGroup  mixerGroups = mixer.FindMatchingGroups("VFX")[0];
                if (mixerGroups != null)
                {
                    oneShotAudioSource.outputAudioMixerGroup = mixerGroups;
                }
                else
                {
                    Debug.LogWarning("No VFX mixer group found!");
                }

            }
            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    public static void StopSound()
    {
        if(oneShotAudioSource)
        {
            oneShotAudioSource.Stop();
        }
    }

    //可优化
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMove:
                if(soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 0.3f;
                    if(lastTimePlayed + playerMoveTimerMax < Time.time){
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }else{
                        return false;
                    }
                }else{
                    return true;
                }

        }
    }


    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach (var item in GameAssets.i.soundAudioClipArray)
        {
            if (item.sound == sound)
            {
                return item.audioClip;
            }
        }
        Debug.Log("Sound" + sound + " not found");
        return null;
    }

    public static void AddButtonSounds(this Button_UI button,Sound sound = 0 )
    {
        if(sound == 0)
        {
            sound = Sound.ButtonEffect1;
        }
        button.MouseDownOnceFunc +=() =>SoundManager.PlaySound(sound);
    }
}
