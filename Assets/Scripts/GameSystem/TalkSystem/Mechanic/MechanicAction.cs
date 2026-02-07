using UnityEngine;

public class MechanicAction : NPCAction
{
    public MechanicAction(NPCController npcController) : base(npcController)
    {}

    public override void PerformNegative()
    {
        npcController.PlayDialogue("Negative");
    }

    public override void PerformPositive()
    {
        PlayerMove pl;
        
        //Money & Body check if money then buy
        npcController.PlayDialogue("Positive");
        
    }
    
    
}
