using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "GameAssets")]
public class GameAssets : ScriptableObject
{

    private static GameAssets _i;

    public static GameAssets i {
        get{
            if(_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }
    // public static GameAssets GetGameAssets()
    // {
    //     return Resources.Load<GameAssets>("GameAssets");
    // }
    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip{
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }
}
