using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        //StartDialogue();

    }

    public void StartDialogue(DialogueData data)
    {
        sentences.Clear();
        
        foreach (DialogueLine line in data.lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            return;
        }
        
        DialogueLine currentLine = sentences.Dequeue();
        
        nameText.text = currentLine.npc.name;
        contentText.text = currentLine.context;
        
        if(currentLine.npc.pfp != null)
            portraitImage.sprite = currentLine.npc.pfp;
    }
    
}
