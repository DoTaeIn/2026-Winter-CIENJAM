using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC Data", menuName = "ScriptableObjects/NPC Data")]
public class NPC : ScriptableObject
{
    [HideInInspector] public int id;
    public string name;
    public Sprite pfp;
}
