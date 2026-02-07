using UnityEngine;
using static MagicWeaponData;

[CreateAssetMenu(fileName = "New Staff", menuName = "Weapon/Staff")]
public class MagicWeaponData : WeaponData
{
    public enum MagicType { Projectile, InstantArea }

    [Header("Magic Settings")]
    public MagicType magicType;
    public GameObject spellPrefab; // 마법 프리펩에는 SpellBehavior 스크립트가 있어야 함!!
    public float manaCost = 10f;
    public float spellRange = 5.0f;

    public override void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer)
    {
        Debug.Log("Staff attack!");
        // meleeWeaponData의 Attack()과 거의 유사할듯?
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
