using UnityEngine;

public class Fireball : SpellBehavior
{
    [Header("Settings")]
    public float speed = 8f;

    public override void Initialize(Vector2 targetPos, float dmg)
    {
        Debug.Log("Cast Fireball!");
        base.Initialize(targetPos, dmg);
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = dir * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // 적에게 데미지 주는 로직
            
            //폭발 이펙트?
            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {

            Destroy(gameObject);
        }
    }
}
