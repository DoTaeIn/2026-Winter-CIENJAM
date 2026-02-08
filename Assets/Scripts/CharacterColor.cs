using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [SerializeField]
    Color _color = Color.white;
    
    [CreateProperty]
    public Color color
    {
        get => _color;
        set
        {
            _color = value;
            SetColor();
        }
    }
    
    public List<string> partNames = new List<string>
    {
        "Back Arm",
        "Back Leg",
        "Front Arm",
        "Front Leg",
        "Torso",
        "Head",
    };

    List<SpriteRenderer> _renderers;

    void Awake()
    {
        InitRenderers();
    }

    void InitRenderers()
    {
        _renderers = new List<SpriteRenderer>();
        var children = GetComponentsInChildren<Transform>();
        foreach (string partName in partNames)
        {
            SpriteRenderer renderer = children.FirstOrDefault(c => c.name == partName)?.GetComponent<SpriteRenderer>();
            if (renderer != null)
                _renderers.Add(renderer);
        }
    }
    void OnValidate()
    {
        InitRenderers();
        SetColor();
    }

    void SetColor() => _renderers.ForEach(r => r.color = _color);
}
