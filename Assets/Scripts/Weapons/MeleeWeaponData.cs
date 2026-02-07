using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapon/Melee")]
public class MeleeWeaponData : WeaponData
{   
    public float attackRadius = 1.0f;

    [Header("Hitbox Settings")]
    public Vector2 hitboxSize = new Vector2(1.0f, 2.0f); // 가로, 세로 크기
    public float offsetDistance = 2.0f;

    //public GameObject weaponPrefab;

    public override void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer)
    {
        hitboxSize.x = attackRadius;
        bool isRight = targetPos.x > firePos.position.x;
        // 방향 벡터 설정 (오른쪽이면 (1,0), 왼쪽이면 (-1,0))
        Vector2 facingDir = isRight ? Vector2.right : Vector2.left;

        // 공격 중심점 계산 
        // firePos 위치에서 왼쪽이나 오른쪽으로 offsetDistance만큼만 이동
        Vector2 attackCenter = (Vector2)firePos.position + (facingDir * offsetDistance);

        float angle = 0f;

        // (중심점, 사이즈, 각도, 레이어)
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCenter, hitboxSize, angle, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"{enemy.name} 베임! (좌우 고정 Hitbox)");

            // 여기에 실제 데미지 주는 코드 추가
            // var health = enemy.GetComponent<EnemyHealth>();
            // if(health != null) health.TakeDamage(damage);
        }
    }
}
