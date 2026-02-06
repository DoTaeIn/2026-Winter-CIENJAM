using UnityEngine;
using UnityEngine.Pool;

public class ArrowBehavior : MonoBehaviour
{
    Collider2D col;

    private IObjectPool<ArrowBehavior> _pool;
    private Rigidbody2D rb;
    private TrailRenderer trail;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        trail = GetComponent<TrailRenderer>();
    }

    // 화살 생성시 풀 정보 저장
    public void SetPool(IObjectPool<ArrowBehavior> pool)
    {
        _pool = pool;
    }

    // 화살을 풀에서 꺼낼 떄 호출됨
    void OnEnable()
    {
        // 일정 시간 후 자동으로 사라지게
        Invoke(nameof(ReturnToPool), 5.0f);

        // 물리 속도 초기화
        if (rb != null) rb.linearVelocity = Vector2.zero;

        // Trail Renderer가 있다면 궤적 지우기(아직은 필요X)
        if (trail != null) trail.Clear();
    }

    // 충돌시
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // 충돌한 적에게 피해 입히는 로직 추가
        }
        ReturnToPool();
    }

    // 풀로 반납하는 함수
    void ReturnToPool()
    {
        CancelInvoke(); // 예약된 자동 반납 취소

        // 이미 꺼진 상태면 반납 X
        if (!gameObject.activeSelf) return;

        // 풀에 자신 반납
        _pool.Release(this);
    }
}
