using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[System.Serializable]
public struct DialogueBranch
{
    public string key;
    public DialogueData data;
}

public enum NPCActionType
{
    Choice,
    Random
}

public class NPCController : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private List<DialogueBranch> dialogueBranches;
    public NPCActionType  npcActionType;
    private NPCAction currentAction;
    private MechanicAction mechanicAction;
    private CookAction  cookAction;
    
    [Header("Setting")]
    [SerializeField] [Range(0, 100)] private int positiveRate;
    private bool isFirstTalk = true;

    private void Awake()
    {
        mechanicAction = new MechanicAction(this);
        cookAction = new CookAction(this);
    }

    public void StartInteraction(bool isShop)
    {
        if (isFirstTalk)
        {
            Debug.Log("첫 만남 대화 시작");
            PlayDialogue("First", () => 
            {
                isFirstTalk = false;
                CheckAction(isShop);
            });
        }
        else
        {
            CheckAction(isShop);
        }
    }
    
    private void CheckAction(bool isShop)
    {

        if (isShop)
        {
            
        }
        else
        {
            if (npcActionType == NPCActionType.Random)
            {
                int rand = UnityEngine.Random.Range(0, 100);

                if (rand < positiveRate)
                {
                    PlayDialogue("Success", () => currentAction.PerformPositive());
                }
                else // 실패 (Negative)
                {
                    PlayDialogue("Fail", () => currentAction.PerformNegative());
                }
            }
            else
            {
                PlayDialogue("Prechoice", () =>
                {
                    UIManager.instance.TogglePanel("Choice");
                });
            }
        }
    }
    
    
    public void PlayDialogue(string key, Action onEndAction = null)
    {
        DialogueBranch branch = dialogueBranches.Find(x => x.key == key);
        
        if (branch.data != null)
        {
            TalkSystem.instance.StartDialogue(branch.data, onEndAction);
        }
        else
        {
            Debug.LogError($"'{key}' 키를 가진 대사가 없습니다. 바로 다음 단계로 넘어갑니다.");
            onEndAction?.Invoke(); 
        }
    }
    
}
