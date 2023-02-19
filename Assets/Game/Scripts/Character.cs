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
    public float attackSlideDuration = 0.02f;
    public float attackSlideSpeed = 1f;

    public float SlideSpeed = 9.0f;
    //Health
    private Health _health;
    private DamageCaster _damageCaster;

    //无敌状态
    public bool isInvincible;
    public float invincibleDuration = 1f;

    private float attackAnimationDuration;

    private Vector3 impactOnCharacter;
    //状态
    public enum CharacterState
    {
        Normal,Attacking,Dead,BeingHit,Slide,Spawn,Dance
    }

    public CharacterState currentState;
    //Material animation
    private MaterialPropertyBlock _materialPropertyBlock;
    private SkinnedMeshRenderer _skinnedMeshRenderer;

    //角色身上的物品
    public GameObject ItemToDrop;
    public int Coin;
    //敌人生成
    public float SpawnDuration = 2.0f;
    private float currentSpawnTime;

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
            SwitchStateTo(CharacterState.Spawn);
        }else{
             _playerInput = GetComponent<PlayerInput>();
        }

    }

    private void CalculatePlayerMovement()
    {
        if(_playerInput.MouseButtonDown && _cc.isGrounded){
            SwitchStateTo(CharacterState.Attacking);
            return;
        }else if(_playerInput.SpaceKeyDown && _cc.isGrounded){
            SwitchStateTo(CharacterState.Slide);
            return;
        }else if (_playerInput.B_KeyDown && _cc.isGrounded)
        {
            SwitchStateTo(CharacterState.Dance);
            return;
        }

        _movementVelocity.Set(_playerInput.HorizontalInput,0f,_playerInput.VerticalInput);
        _movementVelocity.Normalize();
        _movementVelocity = Quaternion.Euler(0,-45f,0) * _movementVelocity; //保证跟相机的方向统一
        _animator.SetFloat("Speed",_movementVelocity.magnitude);
        if(_movementVelocity.magnitude >= 1f)
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerMove);
        }else
        {
            SoundManager.StopSound();
        }
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
                        //实现加速冲刺的效果
                        float timePassed = Time.time - attackStartTime;
                        float lerpTime = timePassed / attackSlideDuration;
                        _movementVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero,lerpTime);
                    }
                    if(_playerInput.MouseButtonDown && _cc.isGrounded)
                    {
                        string currentClipName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        //大于等于1表示动画播放完一次了
                        Debug.Log("当前播放的动画 + " + currentClipName);
                        attackAnimationDuration = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        if(currentClipName != "LittleAdventurerAndie_ATTACK_03" && attackAnimationDuration > 0.5f && attackAnimationDuration < 0.7f)
                        {
                            _playerInput.MouseButtonDown = false;
                            SwitchStateTo(CharacterState.Attacking);

                            //CalculatePlayerMovement();
                        }
                    }

                }
                break;
            case CharacterState.Dead:
                return;
            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                _movementVelocity = transform.forward * SlideSpeed * Time.deltaTime;
                break;
            case CharacterState.Spawn:
                currentSpawnTime -= Time.deltaTime;
                if(currentSpawnTime <= 0)
                {
                    SwitchStateTo(CharacterState.Normal);
                }
                break;
            case CharacterState.Dance:
                if(_playerInput.HorizontalInput == 1 || _playerInput.VerticalInput == 1)
                {
                    _animator.SetBool("Dance",false);
                    SwitchStateTo(CharacterState.Normal);
                }
                break;
        }

        CheckBeingHit();

        if(isPlayer){
            if(_cc.isGrounded == false)
                _verticalVelocity = Gravity;
            else
                _verticalVelocity = Gravity * 0.3f;
            _movementVelocity += _verticalVelocity * Vector3.up * Time.deltaTime;
            _cc.Move(_movementVelocity);
            _movementVelocity = Vector3.zero;
            Debug.Log("玩家当前状态"+ currentState);
        }else
        {
            if(currentState != CharacterState.Normal)
            {
                _cc.Move(_movementVelocity);
                _movementVelocity = Vector3.zero;
            }
            Debug.Log("敌人当前状态"+ currentState);
        }
    }

    private void CheckBeingHit()
    {
        if(impactOnCharacter.magnitude  > 0.2f)
        {
            _movementVelocity = impactOnCharacter * Time.deltaTime;
        }
        impactOnCharacter = Vector3.Lerp(impactOnCharacter, Vector3.zero,Time.deltaTime * 5);
    }

    public void SwitchStateTo(CharacterState newState){
        //clear cache
        if(isPlayer)
        {
            ClearPlayerChache();
        }
        //Exiting state
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attacking:
                if(_damageCaster != null)
                    _damageCaster.DisableDamageCaster();
                if(isPlayer)
                    GetComponent<PlayerVFXManager>().StopBlade();
                break;
            case CharacterState.Dead:
                return;

            case CharacterState.BeingHit:
                break;
            case CharacterState.Slide:
                break;
            case CharacterState.Spawn:
                isInvincible = false;
                break;
            case CharacterState.Dance:
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
                {
                    attackStartTime = Time.time;
                    RotateToCursor();
                }
                break;
            case CharacterState.Dead:
                _cc.enabled = false;
                _animator.SetTrigger("Dead");
                StartCoroutine(MaterialDissolve());
                break;
            case CharacterState.BeingHit:
                _animator.SetTrigger("BeingHit");
                if(isPlayer)
                {
                    isInvincible = true;
                    StartCoroutine(DelayCancelInvincible());
                }
                break;
            case CharacterState.Slide:
                _animator.SetTrigger("Slide");
                SoundManager.PlaySound(SoundManager.Sound.PlayerSlide);
                break;
            case CharacterState.Spawn:
                isInvincible = true;
                currentSpawnTime = SpawnDuration;
                StartCoroutine(MaterialAppear());
                break;
            case CharacterState.Dance:
                _animator.SetBool("Dance",true);
                GetComponent<PlayerVFXManager>().isPlayerDancingVFX(true);
                SoundManager.PlaySound(SoundManager.Sound.PlayerDancingMusic);
                break;
        }

        currentState = newState;

        Debug.Log("转换状态" + currentState );

    }

    public void ClearPlayerChache()
    {
        _playerInput.ClearCache();
        SoundManager.StopSound();
        GetComponent<PlayerVFXManager>().isPlayerDancingVFX(false);
    }

    public void AttackAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void BeingHitAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void SlideAnimationEnds()
    {
        SwitchStateTo(CharacterState.Normal);
    }

    public void ApplyDamage(int damage, Vector3 attackerPos = new Vector3())
    {
        if (isInvincible)
        {
            return;
        }
        if(_health != null)
        {
            _health.ApplyDamage(damage);
        }
        if(!isPlayer)
        {
            GetComponent<EnemyVFXManager>().PlayBeingHitVFX(attackerPos);
        }
        StartCoroutine(MaterialBlink());

        // if(currentState == CharacterState.Dead)
        //     return;
        if (isPlayer)
        {
            SwitchStateTo(CharacterState.BeingHit);
            ApplyImpact(attackerPos, 10f);
            SoundManager.PlaySound(SoundManager.Sound.PlayerHit);
        }
        else
        {
            ApplyImpact(attackerPos, 2.5f);
            CameraShake.Instance.ShakeCamera();
        }
    }

    IEnumerator DelayCancelInvincible()
    {
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    private void ApplyImpact(Vector3 attackerPos, float force)
    {
        //我减你 方向是 你->我
        Vector3 impactDir = transform.position - attackerPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharacter = impactDir * force;
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
        yield return new WaitForSeconds(2);

        float dissolveTimeDuration = 2f;
        float currentDissolveTime = 0;
        float dissolveHight_start = 20f;
        float dissolveHight_target = -10f;
        float dissolveHight;

        _materialPropertyBlock.SetFloat("_enableDissolve",1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
        //实现线性的溶解
        while(currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHight = Mathf.Lerp(dissolveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height",dissolveHight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        DropItem();
        // this.gameObject.SetActive(false);
    }

    public void DropItem()
    {
        if (ItemToDrop != null)
        {
            Instantiate(ItemToDrop, transform.position, Quaternion.identity);
        }
    }

    public void RotateToTarget()
    {
        if (currentState != CharacterState.Dead)
        {
            transform.LookAt(targetPlayer, Vector3.up);
        }
    }

    IEnumerator MaterialAppear()
    {
        float dissolveTimeDuration = SpawnDuration;
        float currentDissolveTime = 0;
        float dissolveHight_start = -10f;
        float dissolveHight_target = 20f;
        float dissolveHight;

        _materialPropertyBlock.SetFloat("_enableDissolve", 1f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);

        while (currentDissolveTime < dissolveTimeDuration)
        {
            currentDissolveTime += Time.deltaTime;
            dissolveHight = Mathf.Lerp(dissolveHight_start, dissolveHight_target, currentDissolveTime / dissolveTimeDuration);
            _materialPropertyBlock.SetFloat("_dissolve_height", dissolveHight);
            _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            yield return null;
        }

        _materialPropertyBlock.SetFloat("_enableDissolve", 0f);
        _skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
    //拾取道具
    public void PickUpItem(Prop item)
    {
        void AddHealth(int health)
        {
            _health.AddHealth(health);
            GetComponent<PlayerVFXManager>().PlayHealVFX(transform.position);
        }
        void AddCoin(int coin)
        {
            Coin += coin;
        }
        switch (item.Type)
        {
            case Prop.PropType.Heal:
                AddHealth(item.Value);
                break;
            case Prop.PropType.Coin:
                AddCoin(item.Value);
                break;
        }
    }

    private void RotateToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitResult;

        if (Physics.Raycast(ray, out hitResult, 1000, 1 << LayerMask.NameToLayer("CursorTest")))
        {
            Vector3 cursorPos = hitResult.point;
            transform.rotation = Quaternion.LookRotation(cursorPos - transform.position, Vector3.up);
        }
    }
}
