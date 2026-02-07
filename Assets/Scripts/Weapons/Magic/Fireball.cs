using UnityEngine;

public class Fireball : SpellBehavior
{
    [Header("Settings")]
    public float speed = 8f;
    public float explodeRadius = 1.0f;

    private float damage;
    public override void Initialize(Vector2 targetPos, float dmg)
    {
        Debug.Log("Cast Fireball!");

        damage = dmg;

        base.Initialize(targetPos, dmg);
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, explodeRadius);

            foreach (Collider2D enemy in enemies)
            {
                    // 적에게 데미지 주는 로직
                    other.GetComponent<Enemy>()?.TakeDamage(damage);
                    // explosion effect can be implemented here
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
