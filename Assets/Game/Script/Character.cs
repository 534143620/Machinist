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

    //Enemy
    public bool isPlayer = true;
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Transform targetPlayer;

    //Player slides
    private float attackStartTime;
    public float attackSlideDuration = 0.1f;
    public float attackSlideSpeed = 1.2f;


    //state
    public enum CharacterState
    {
        Normal,Attacking
    }
    public CharacterState currentState;

    private void Awake() {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        
        if(isPlayer == false){
            _navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            targetPlayer = GameObject.FindWithTag("Player").transform;
            _navMeshAgent.speed = MoveSpeed;
        }else{
             _playerInput = GetComponent<PlayerInput>();
        }

    }

    private void CalculatePlayerMovement()
    {
        if(_playerInput.MouseButtonDown && _cc.isGrounded){
            SwitchStateTo(CharacterState.Attacking);
            return;
        }

        _movementVelocity.Set(_playerInput.HorizontalInput,0f,_playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0,-45f,0) * _movementVelocity; //保证跟相机的方向统一
        _animator.SetFloat("Speed",_movementVelocity.magnitude);
        _movementVelocity *= MoveSpeed * Time.deltaTime;

        if(_movementVelocity != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_movementVelocity);
        _animator.SetBool("AirBorne",!_cc.isGrounded);
        
    }

    private void CalculateEnemyMovement()
    {   
        if (Vector3.Distance(targetPlayer.position,transform.position) >= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.SetDestination(targetPlayer.position);
            _animator.SetFloat("Speed",0.2f);
        }
        else
        {
            _navMeshAgent.SetDestination(transform.position);
            _animator.SetFloat("Speed",0f);
            SwitchStateTo(CharacterState.Attacking);
        }

    }

    private void FixedUpdate() {
        switch (currentState)
        {
            case CharacterState.Normal:
                if(isPlayer)
                    CalculatePlayerMovement();
                else
                    CalculateEnemyMovement();
                break;
            case CharacterState.Attacking:
                //TODO
                if(isPlayer)
                {
                    _movementVelocity = Vector3.zero;

                    if(Time.time < attackStartTime + attackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero,lerpTime);
                    }

                }
                break;
        }

        if(isPlayer){
            if(_cc.isGrounded == false)
                _verticalVelocity = Gravity;
            else
                _verticalVelocity = Gravity * 0.3f;
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _cc.Move(_movementVelocity);    
        }
    }

    private void SwitchStateTo(CharacterState newState){
        //clear cache
        if(isPlayer)
        {
            _playerInput.MouseButtonDown = false;
        }
        //Exiting state
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                break;
        }
        //Entering state
        switch (newState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if(!isPlayer) //敌人需要朝着玩家旋转
                {
                    Quaternion newRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
                    transform.rotation = newRotation;
                }
                _animator.SetTrigger("Attack");
                if(isPlayer)
                    attackStartTime = Time.time;

                AttackAnimationEnds();
                break;
        }

        currentState = newState;

        Debug.Log("转换状态");

    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }
}
