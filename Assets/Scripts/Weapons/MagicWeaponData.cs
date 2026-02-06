using UnityEngine;

[CreateAssetMenu(fileName = "New Staff", menuName = "Weapon/Staff")]
public class MagicWeaponData : WeaponData
{
    [Header("Magic Settings")]
    public GameObject spellPrefab; // 마법 프리펩에는 SpellBehavior 스크립트가 있어야 함!!
    public float manaCost = 10f;
    
    public override void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer)
    {
        Debug.Log("Staff attack!");
        // meleeWeaponData의 Attack()과 거의 유사할듯?
    }
    public void CastSpell()
    {
        Debug.Log("Staff spell cast!");
        // spellPrefab을 instantiate
        // 그 spellPrefab의 CastSpell() 메서드 호출
        // 로직 구현
    }
}
