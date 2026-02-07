using System;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")] 
    [SerializeField] private int gold = 10;
    [SerializeField] private float hp = 10;
    [SerializeField] private float damage = 10;
    
    public float moveSpeed = 2f;
    public float idleTime = 2f;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    [Header("Sensors")]
    public Transform frontCheck;
    public Transform wallCheck;
    public float detectRange = 5f;
    
    private IState currentState;
    public IdleState idleState;
    public PatrolState patrolState;
    public ChaseState chaseState;
    
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform target;
    [HideInInspector] public int facingDir = 1;

    private UnityEvent onDeathEvent = new UnityEvent();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 

        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
    
        ChangeState(idleState);
        
    }

    private void OnEnable() => onDeathEvent.AddListener(OnDeath);
    private void OnDisable() => onDeathEvent.RemoveListener(OnDeath);
    
    
    public void SetEnemyStats(float hp, float damage)
    {
        this.hp = hp;
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

    private void OnDeath()
    {
        //Enable Particle System
        //Instantiate Gold Instance
        //player add "gold" variable
    }

}
