using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterController _cc;
    public float MoveSpeed = 5.0f;
    private Vector3 _movementVelocity; //移动速度
    private PlayerInput _playerInput;
    private float _verticalVelocity;
    public float Gravity = -9.8f;

    private Animator _animator;
    private void Awake() {
        _cc = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
    }

    private void CalculatePlayerMovement()
    {
        _movementVelocity.Set(_playerInput.HorizontalInput,0f,_playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0,-45f,0) * _movementVelocity; //保证跟相机的方向统一
        _animator.SetFloat("Speed",_movementVelocity.magnitude);
        _movementVelocity *= MoveSpeed * Time.deltaTime;

        if(_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        _animator.SetBool("AirBorne",!_cc.isGrounded);
        
    }

    private void FixedUpdate() {
        CalculatePlayerMovement();

        if(_cc.isGrounded == false)
            _verticalVelocity = Gravity;
        else
            _verticalVelocity = Gravity * 0.3f;
        _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
        _cc.Move(_movementVelocity);    
    }
}
