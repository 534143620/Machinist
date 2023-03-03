using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;
    public bool MouseButtonDown;
    public bool SpaceKeyDown;
    public bool B_KeyDown;
    public bool Z_KeyDown;

    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject()) return;
        if(!MouseButtonDown && Time.timeScale != 0)
        {
            MouseButtonDown = Input.GetMouseButtonDown(0);
        }
        if(!SpaceKeyDown && Time.timeScale != 0)
        {
            SpaceKeyDown = Input.GetKeyDown(KeyCode.Space);
        }
        if(!B_KeyDown && Time.timeScale != 0)
        {
            B_KeyDown = Input.GetKeyDown(KeyCode.B);
        }
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput =  Input.GetAxisRaw("Vertical");
    }

    private void OnDisable() {
        ClearCache();
    }

    public void ClearCache()
    {
        MouseButtonDown = false;
        SpaceKeyDown = false;
        B_KeyDown = false;
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
