using UnityEngine;

public abstract class NPCAction
{
    public NPCController npcController;
    
    public NPCAction(NPCController npcController)
    {
        this.npcController = npcController;
    }
    
    public abstract void PerformPositive();
    public abstract void PerformNegative();
    
}
