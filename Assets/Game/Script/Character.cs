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
    //Health
    private Health _health;
    private DamageCaster _damageCaster;

    //状态
    public enum CharacterState
    {
        Normal,Attacking,Dead,BeingHit
    }

    public CharacterState currentState;
    //Material animation
    private MaterialPropertyBlock _materialPropertyBlock;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    //角色身上的物品
    public GameObject ItemToDrop;

    private void Awake() {
        _cc = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _damageCaster = GetComponentInChildren<DamageCaster>();
        _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        _skinnedMeshRenderer.GetPropertyBlock(_materialPropertyBlock);

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
                    if(Time.time < attackStartTime + attackSlideDuration)
                    {
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero,lerpTime);
                    }

                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break;
        }

        if(isPlayer){
            if(_cc.isGrounded == false)
                _verticalVelocity = Gravity;
            else
                _verticalVelocity = Gravity * 0.3f;
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _cc.Move(_movementVelocity);
            _movementVelocity = Vector3.zero;
        }
    }

    public void SwitchStateTo(CharacterState newState){
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
                // if(_damageCaster != null)
                //     DisableDamageCaster();
                break;
            case CharacterState.Dead:
                break;
            case CharacterState.BeingHit:
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
            case CharacterState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
             case CharacterState.BeingHit:
                _animator.SetTrigger("BeingHit");
                break;
        }

        currentState = newState;

        Debug.Log("转换状态");

    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if(_health != null)
        {
            _health.ApplyDamage(damage);
        }
        if(!isPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackerPos);
        }
        StartCoroutine(MaterialBlink());

        if(isPlayer && currentState != CharacterState.Dead)
        {
            SwitchStateTo(CharacterState.BeingHit);
        }
    }

    public void EnableDamageCaster()
    {
        _damageCaster.EnableDamageCaster();
    }

    public void DisableDamageCaster()
    {
        _damageCaster.DisableDamageCaster();
    }

    IEnumerator MaterialBlink()
    {
        _materialPropertyBlock.SetFloat("_blink",0.4f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        yield return new WaitForSeconds(0.2f);

        _materialPropertyBlock.SetFloat("_blink",0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

    }

    IEnumerator MaterialDissolve()
    {
        yield return new WaitForSeconds(2.0f);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float dissolveHeight_start = 20f;
        float dissolveHeight_end = -10f;
        float dissolveHeight;

        _materialPropertyBlock.SetFloat("_enableDissolve",1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        //实现线性的溶解
        while(currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHeight = Mathf.Lerp(dissolveHeight_start,dissolveHeight_end,currentDissolveTime / dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height",dissolveHeight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        DropItem();
        this.gameObject.SetActive(false);
    }

    public void DropItem()
    {
        if(ItemToDrop != null)
        {
            Instantiate(ItemToDrop,transform.position,Quaternion.identity);
        }
    }

}
