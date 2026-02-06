using UnityEngine;

[CreateAssetMenu(fileName = "New Melee", menuName = "Weapon/Melee")]
public class MeleeWeaponData : WeaponData
{   
    public float attackRadius = 1.0f;
    public bool isSingleAttack = true; // 단일 타겟 공격 여부

    public override void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer)
    {
        // 근접 공격 구현
        //Vector2 attackDirection = (targetPos - (Vector2)firePos.position).normalized;
        //Vector2 attackCenter = (Vector2)firePos.position + attackDirection * attackRange;
        Debug.Log("Melee attack!");
        if (isSingleAttack)
        {
            // 단일 타겟 공격 로직
        }
        else
        {
            Collider2D[] hitEnemies;
            //melee
            //foreach (Collider2D enemy in hitEnemies)
            //{
            //    // 적에게 데미지 적용
            //}
        }
    }
}
