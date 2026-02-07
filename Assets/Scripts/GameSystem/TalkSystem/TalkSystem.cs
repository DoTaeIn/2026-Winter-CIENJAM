using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.UI;


public class TalkSystem : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text contentText;
    public Image portraitImage;
    
    private Queue<DialogueLine> sentences = new Queue<DialogueLine>();
    
    public static TalkSystem instance;
    
    private Action onDialogueEndCallback;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        //StartDialogue();

    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
        }
        
        DialogueLine currentLine = sentences.Dequeue();
        
        nameText.text = currentLine.npc.name;
        contentText.text = currentLine.context;
        
        if(currentLine.npc.pfp != null)
            portraitImage.sprite = currentLine.npc.pfp;
    }
    
    public void StartDialogue(DialogueData data, Action onEnd = null)
    {
        onDialogueEndCallback = onEnd; // 할 일 저장
        
        UIManager.instance.TogglePanel("Talk");
        DisplayNextSentence();
    }

    private void EndDialogue()
    {
        UIManager.instance.TogglePanel("Talk");
        
        onDialogueEndCallback?.Invoke();
        onDialogueEndCallback = null;
    }
    
}
