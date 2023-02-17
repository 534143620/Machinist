using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private CinemachineVirtualCamera CinemachineVirtualCamera;
    public float ShakeIntensity = 1f;
    public float ShakeTime = 0.2f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Awake() {

        if(CameraShake.Instance == null ){
            CameraShake.Instance = this;
        }else if(CameraShake.Instance != this){
            Destroy(this);
        }

        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    public void ShakeCamera(){
        CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = ShakeIntensity;

        timer = ShakeTime;
    }

    void StopShake(){
        CinemachineBasicMultiChannelPerlin _cbmcp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0){

            timer -= Time.deltaTime;

            if(timer <= 0){
                StopShake();
            }
        }
    }
}
