using System;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
    public Vector2 normalOffset = new Vector2(0f, 0.75f);
    public Vector2 normalSize = new Vector2(0.5f, 1.5f);
    public Vector2 noLegOffset = new Vector2(0f, 1.0625f);
    public Vector2 noLegSize = new Vector2(0.5f, 0.875f);
    
    IAnimatableCharacter _character;
    CapsuleCollider2D _collider;
    
    bool _frontLegEnabled = true;
    bool _backLegEnabled = true;

    void Awake()
    {
        _character = GetComponent<IAnimatableCharacter>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    void OnEnable()
    {
        _character.OnPartAttached += OnPartAttached;
        _character.OnPartDetached += OnPartDetached;
    }

    void OnDisable()
    {
        _character.OnPartAttached -= OnPartAttached;
        _character.OnPartDetached -= OnPartDetached;
    }
    void OnPartAttached(CharacterPartType partType)
    {
        if (partType == CharacterPartType.FrontLeg)
            _frontLegEnabled = true;
        else if (partType == CharacterPartType.BackLeg)
            _backLegEnabled = true;
        UpdateCollider();
    }

    void OnPartDetached(CharacterPartType partType)
    {
        if (partType == CharacterPartType.FrontLeg)
            _frontLegEnabled = false;
        else if (partType == CharacterPartType.BackLeg)
            _backLegEnabled = false;
        UpdateCollider();
    }

    void UpdateCollider()
    {
        _collider.size = _frontLegEnabled || _backLegEnabled ? normalSize : noLegSize;
        _collider.offset = _frontLegEnabled || _backLegEnabled ? normalOffset : noLegOffset;
    }
}
