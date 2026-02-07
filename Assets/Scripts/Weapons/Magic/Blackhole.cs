using UnityEngine;

public class Blackhole : SpellBehavior
{
    [Header("Settings")]
    public float speed = 8f;
    public float duration = 3.0f;
    public float pullRadius = 3.0f;
    public float pullForce = 5.0f;   
    public float damagePerSecond = 10f;

    private float damageTickRate = 0.5f;
    private float nextDamageTime = 0f;
    public override void Initialize(Vector2 targetPos, float dmg)
    {
        Debug.Log("Cast Blackhole!");
        base.Initialize(targetPos, dmg);
    }

    void Start()
    {
        // 일정 시간 후 자동으로 사라짐
        Destroy(gameObject, duration);
    }

    void FixedUpdate() // 물리 연산은 FixedUpdate
    {
        PullEnemies();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적에게 데미지 주는 로직
        }
    }

    void PullEnemies()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, pullRadius);

        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // 끌어당김
                Vector2 direction = (transform.position - enemy.transform.position).normalized;
                Vector2 distance = (transform.position - enemy.transform.position);

                // Rigidbody 힘으로 당기기 (물리 적용)
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // 거리가 가까울수록 더 세게 당기려면 거리 계산 추가 가능
                    //rb.AddForce(direction * pullForce);
                    rb.AddForce(distance * pullForce);
                }

                // 강제 이동
                // enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, transform.position, pullForce * Time.fixedDeltaTime);
            }
        }
    }

    // 도트 데미지 처리 (TriggerStay 사용)
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && Time.time >= nextDamageTime)
        {
            // 실제 데미지 적용 코드
            Debug.Log($"{other.name} 블랙홀 데미지 받음!");
            other.GetComponent<Enemy>()?.TakeDamage(damagePerSecond * damageTickRate);

            nextDamageTime = Time.time + damageTickRate;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0, 1, 0.3f); // 보라색
        Gizmos.DrawSphere(transform.position, pullRadius);
    }
}
