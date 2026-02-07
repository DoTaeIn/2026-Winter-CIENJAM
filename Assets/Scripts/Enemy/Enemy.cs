using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")] 
    [SerializeField] private int gold = 10;
    [SerializeField] private float currHp = 10;
    [SerializeField] private float maxHp = 10;
    [SerializeField] private float damage = 10;
    
    public float moveSpeed = 2f;
    public float idleTime = 2f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    
    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackDamage = 10f;
    public float attackCooldown = 2f;
    public float attackWindup = 0.5f;
    public float attackDuration = 1.0f;

    [Header("Attack Area")]
    public Transform attackPoint;       // 공격 판정 위치 (무기 끝 등)
    public float attackRadius = 0.5f;   // 공격 판정 크기

    [Header("Sensors")]
    public Transform frontCheck;
    public Transform wallCheck;
    public float detectRange = 5f;
    
    private IState currentState;
    public IdleState idleState;
    public PatrolState patrolState;
    public ChaseState chaseState;
    public AttackState attackState;
    public HitState hitState;
    
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform target;
    [HideInInspector] public int facingDir = 1;

    private UnityEvent onDeathEvent = new UnityEvent();
    
    [HideInInspector] public float lastAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 

        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        hitState = new HitState(this);
        
        ChangeState(idleState);
        
    }

    private void OnEnable() => onDeathEvent.AddListener(OnDeath);
    private void OnDisable() => onDeathEvent.RemoveListener(OnDeath);
    
    
    public void SetEnemyStats(float hp, float damage)
    {
        this.currHp = hp;
        this.maxHp = hp;
        this.damage = damage;
    }

    void Update()
    {
        if (currentState != null)
            currentState.Update();
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
    public void SetVelocity(float xVelocity)
    {
        rb.linearVelocity = new Vector2(xVelocity, rb.linearVelocity.y);
        
        if (xVelocity > 0 && facingDir == -1) Flip();
        else if (xVelocity < 0 && facingDir == 1) Flip();
    }
    
    public void Flip()
    {
        facingDir *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    public bool CheckEnvironment()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(frontCheck.position, Vector2.down, 1f, groundLayer);
        bool isCliff = (hit.collider == null);
        
        bool isWall = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 0.5f, groundLayer).collider != null;

        return isCliff || isWall;
    }
    
    public bool CheckPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, detectRange, playerLayer);
        if (hit.collider != null)
        {
            target = hit.transform;
            return true;
        }
        return false;
    }
    
    public void PerformAttack()
    {

        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRadius, playerLayer);

        if (hitPlayer != null)
        {

        }
    }
    
    public void TakeDamage(float damage)
    {

        currHp -= damage;
        
        if (currHp <= 0)
        {

            return;
        }
        
        ChangeState(hitState);
        
        // StartCoroutine(KnockBack()); 
    }

    private void OnDeath()
    {
        //Enable Particle System
        //Instantiate Gold Instance
        //player add "gold" variable
    }

}
