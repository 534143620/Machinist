using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalInput;
    public float VerticalInput;
    // Update is called once per frame
    void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput =  Input.GetAxisRaw("Vertical");
    }

    private void OnDisable() {
        HorizontalInput = 0;
        VerticalInput = 0;
    }
}
