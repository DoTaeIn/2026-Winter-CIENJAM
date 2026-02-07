using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    // 상태 정의
    public enum State { Idle, Chase, Attack }

    [Header("Status")]
    public State currentState = State.Idle;
    public float moveSpeed = 3f;
    public float detectionRange = 6f; // 플레이어 감지 범위
    public float attackRange = 2.5f;  // 공격 사거리

    [Header("References")]
    public Transform player; // 플레이어 위치
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // 플레이어 미설정시 태그로 자동 찾기
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 상태에 따른 행동 분기
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Chase:
                HandleChase();
                break;
            case State.Attack:
                HandleAttack();
                break;
        }
    }

    // 각 상태별 행동 처리 함수들
    void HandleIdle()
    {
        // 대기 중 행동
        rb.linearVelocity = Vector2.zero;

        // 플레이어까지 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);
        // 가까우면 추격 상태로 전환
        if (distance <= detectionRange)
        {
            ChangeState(State.Chase);
        }
    }

    void HandleChase()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // 공격 범위 안에 들어오면 공격 상태로 전환
        if (distance <= attackRange)
        {
            ChangeState(State.Attack);
            return;
        }

        // 너무 멀어지면 대기로 전환
        if (distance > detectionRange * 1.5f) // 감지 범위보다 좀 더 멀어지면 포기
        {
            ChangeState(State.Idle);
            return;
        }

        // 플레이어 방향으로 이동
        MoveTowardsPlayer();
    }

    void HandleAttack()
    {
        // 공격 중에는 이동 멈춤
        rb.linearVelocity = Vector2.zero;

        float distance = Vector2.Distance(transform.position, player.position);

        // 사거리를 벗어나면 다시 추격
        if (distance > attackRange)
        {
            ChangeState(State.Chase);
        }

        // 실제 공격 동작
    }

    void MoveTowardsPlayer()
    {
        // 플레이어 방향 계산 (왼쪽인지 오른쪽인지)
        Vector2 direction = (player.position - transform.position).normalized;

        // X축 이동만 할 경우 (플랫포머)
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // 보는 방향 따라 스프라이트 뒤집기
        if (direction.x > 0) sr.flipX = false; // 오른쪽
        else if (direction.x < 0) sr.flipX = true; // 왼쪽
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        // 애니메이션 추후 적용

        Debug.Log($"상태 변경: {currentState} -> {newState}");
    }

    // Debug용 gizmo
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // 감지 범위(노란색)

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // 공격 범위(빠라간색)
    }
}