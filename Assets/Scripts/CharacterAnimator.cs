using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [Serializable]
    class CharacterPart
    {
        public GameObject gameObject;
        public Animator animator;
        public bool enabled;
    }

    public CharacterAnimationAsset animationAsset;
    public CharacterParts<string> partNames = new CharacterParts<string>
    {
        backArm = "Back Arm",
        backLeg = "Back Leg",
        frontArm = "Front Arm",
        frontLeg = "Front Leg",
        torso = "Torso"
    };
    
    ICharacterAnimatable _character;
    CharacterParts<CharacterPart> _parts;

    bool _onAir;
    
    void Awake()
    {
        _character = GetComponent<ICharacterAnimatable>();
        
        if (_character == null)
            Debug.LogError($"{nameof(ICharacterAnimatable)} component not found on the GameObject.");
        if (animationAsset == null)
            Debug.LogError($"{nameof(animationAsset)} is not assigned.");
        
        SetParts();
    }

    void SetParts()
    {
        foreach (CharacterPartType partType in Enum.GetValues(typeof(CharacterPartType)))
        {
            GameObject partObject = transform.Find(partNames[partType])?.gameObject;
            if (partObject == null)
                Debug.LogError($"Part '{partNames[partType]}' not found in the hierarchy.");
            
            _parts[partType] = new CharacterPart
            {
                animator = partObject.GetComponent<Animator>(),
                enabled = true,
            };
        }
    }

    void OnEnable()
    {
        _character.OnJump += OnJump;
        _character.OnLand += OnLand;
        _character.OnPartAttached += OnPartAttached;
        _character.OnPartDetached += OnPartDetached;
    }

    void OnDisable()
    {
        _character.OnJump -= OnJump;
        _character.OnLand -= OnLand;
        _character.OnPartAttached -= OnPartAttached;
        _character.OnPartDetached -= OnPartDetached;
    }

    void Update()
    {
        if (_onAir)
        {
            
        }
    }

    void OnJump()
    {
        _onAir = true;
        
    }
    void OnLand()
    {
        _onAir = false;
    }
    void OnPartAttached(CharacterPartType partType)
    {
        var part = _parts[partType];
        part.enabled = true;
        
        part.gameObject.SetActive(true);
    }
    void OnPartDetached(CharacterPartType partType)
    {
        var part = _parts[partType];
        part.enabled = false;
        
        part.gameObject.SetActive(false);
    }

    void Play(CharacterPartType partType, string stateName)
    {
        if (!_parts[partType].enabled) return;
        
        
        var animator = _parts[partType].animator;
        animator.Play(stateName);
    }

    void SyncAnimationState(CharacterPartType syncTo, CharacterPartType syncFrom)
    {
        if (syncFrom == syncTo) return;
        if (!_parts[syncFrom].enabled || !_parts[syncTo].enabled) return;

        var fromState = _parts[syncFrom].animator.GetCurrentAnimatorStateInfo(0);
        var toAnimator = _parts[syncTo].animator;
        
        if (!toAnimator.HasState(0, fromState.fullPathHash)) return;

        toAnimator.Play(fromState.fullPathHash, 0, fromState.normalizedTime);
    }
}
