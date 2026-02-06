using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Basic Settings")]
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

    private void Awake()
    {
        // [누락된 부분] 리지드바디 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>(); 

        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
    
        ChangeState(idleState);
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
        // [수정된 부분] 방향에 따라 센서를 바꿀 필요가 없습니다!
        // Flip()을 하면 frontCheck 센서도 같이 돌아서 항상 '앞'을 가리킵니다.
        
        RaycastHit2D hit = Physics2D.Raycast(frontCheck.position, Vector2.down, 1f, groundLayer);
        bool isCliff = (hit.collider == null);

        // 벽 체크는 방향(facingDir)이 필요합니다 (Ray를 쏘는 방향 때문)
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
    

}
