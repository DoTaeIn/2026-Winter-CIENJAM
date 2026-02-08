using UnityEngine;

public abstract class ItemEffect
{
    public BodyManager bodyManager;

    public ItemEffect(BodyManager bodyManager)
    {
        this.bodyManager = bodyManager;
    } 
    public abstract void DoEffect(float heal);
}
