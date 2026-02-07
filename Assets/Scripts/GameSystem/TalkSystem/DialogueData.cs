using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Header("Speaker Info")] public NPC npc;

    [Header("Context")]
    [TextArea(3, 5)]
    public string context;
}

[CreateAssetMenu(fileName = "New Dialogue Data", menuName = "Scriptable Objects/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public int dialogueId;
    public List<DialogueLine> lines;
}
