using UnityEngine;
public abstract class SpellBehavior : MonoBehaviour
{
    protected Vector2 targetPosition;
    protected float spellDamage;

    public virtual void Initialize(Vector2 targetPos, float dmg)
    {
        targetPosition = targetPos;
        spellDamage = dmg;
    }
}