using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public abstract class WeaponData : ScriptableObject
{
    public string weaponName = "Sword";
    public float damage = 10f;
    public float range = 1.5f;
    public float attackRate = 1f;

    public abstract void Attack(Transform firePos, Vector2 targetPos, LayerMask enemyLayer);
}
