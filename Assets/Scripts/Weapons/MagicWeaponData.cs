using UnityEngine;
using static MagicWeaponData;

[CreateAssetMenu(fileName = "New Staff", menuName = "Weapon/Staff")]
public class MagicWeaponData : WeaponData
{
    public enum MagicType { Projectile, InstantArea }

    [Header("Hitbox Settings")]
    public Vector2 hitboxSize = new Vector2(1.0f, 1.0f); // 가로, 세로 크기
    public float offsetDistance = 1.0f;

    [Header("Magic Settings")]
    public MagicType magicType;
    public GameObject spellPrefab; // 마법 프리펩에는 SpellBehavior 스크립트가 있어야 함!!
    public float manaCost = 10f;
    public float spellRange = 5.0f;

    public override void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer)
    {
        Debug.Log("Staff attack!");
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
            Debug.Log($"{enemy.name} 맞음! (좌우 고정 Hitbox)");
            enemy.GetComponent<Enemy>()?.TakeDamage(damage);
        }
    }
    public void CastSpell(Transform firePos, Vector2 targetPos)
    {
        // check mana here

        if (magicType == MagicType.InstantArea) // blackhole
        {
            SpawnInstantSpell(firePos, targetPos);
        }
        else // fireball, ice floor
        {
            SpawnProjectileSpell(firePos, targetPos);
        }
    }
    private void SpawnInstantSpell(Transform firePos, Vector2 targetPos)
    {
        // calc distance and consider spell range
        Vector2 direction = targetPos - (Vector2)firePos.position;
        float distance = direction.magnitude; // distance
        if (distance > spellRange)
        {
            Debug.Log("Out of range!");
            return;
        }

        Vector2 spawnLocation = (Vector2)firePos.position + (direction.normalized * distance);
        GameObject spellObject = Instantiate(spellPrefab, spawnLocation, Quaternion.identity);
        spellObject.GetComponent<SpellBehavior>().Initialize(targetPos, damage);
    }

    private void SpawnProjectileSpell(Transform firePos, Vector2 targetPos)
    {
        GameObject spell = Instantiate(spellPrefab, firePos.position, Quaternion.identity);
        //Vector2 dir = (targetPos - (Vector2)firePos.position).normalized;
        //spell.GetComponent<Rigidbody2D>().linearVelocity = dir * 10f;
        spell.GetComponent<SpellBehavior>().Initialize(targetPos, damage);
    }
}
