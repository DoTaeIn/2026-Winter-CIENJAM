using UnityEngine;

public class CookAction : NPCAction
{
    public CookAction(NPCController npcController) : base(npcController)
    {}

    public override void PerformPositive()
    {
        //PLayer part random Heal
    }

    public override void PerformNegative()
    {
        //PLayer part random Damage
    }
    
    
}
