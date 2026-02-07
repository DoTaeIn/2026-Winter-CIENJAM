using UnityEngine;

[CreateAssetMenu(fileName = "New Bow", menuName = "Weapon/Bow")]
public class RangedWeaponData : WeaponData
{
    public float projectileSpeed = 30f;

    public override void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer)
    {
        Debug.Log("Bow attack!");

        var arrowController = ArrowPoolManager.Instance.GetArrow();
        GameObject arrow = arrowController.gameObject;

        arrow.GetComponent<ArrowBehavior>().dmg = damage;

        arrow.transform.position = firePos.position;
        // 마우스 방향 계산
        Vector2 direction = (targetPos - (Vector2)firePos.position).normalized;
        // 화살 회전 (날아가는 방향을 보도록)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
        // 화살 속도 적용
        arrow.GetComponent<Rigidbody2D>().linearVelocity = direction * projectileSpeed;

    }
}

