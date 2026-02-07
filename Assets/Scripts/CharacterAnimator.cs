using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    static List<CharacterPartType> armLegTypes = new List<CharacterPartType>
    {
        CharacterPartType.BackArm,
        CharacterPartType.BackLeg,
        CharacterPartType.FrontArm,
        CharacterPartType.FrontLeg
    }; 
    
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
    };

    IAnimatableCharacter _character;
    CharacterParts<CharacterPart> _parts;

    bool _onAir;
    int legCount => (_parts[CharacterPartType.BackLeg].enabled ? 1 : 0) +
                      (_parts[CharacterPartType.FrontLeg].enabled ? 1 : 0);

    void Awake()
    {
        _character = GetComponent<IAnimatableCharacter>();

        if (_character == null)
            Debug.LogError($"{nameof(IAnimatableCharacter)} component not found on the GameObject.");
        if (animationAsset == null)
            Debug.LogError($"{nameof(animationAsset)} is not assigned.");

        SetParts();
    }

    void SetParts()
    {
        var children = GetComponentsInChildren<Transform>();
        foreach (CharacterPartType partType in Enum.GetValues(typeof(CharacterPartType)))
        {
            GameObject partObject = children.FirstOrDefault(c => c.name == partNames[partType])?.gameObject;
            if (partObject == null)
                Debug.LogError($"Part '{partNames[partType]}' not found in the hierarchy.");

            _parts[partType] = new CharacterPart
            {
                gameObject = partObject,
                animator = partObject.GetComponent<Animator>(),
                enabled = true,
            };
        }
    }

    void OnEnable()
    {
        _character.OnJump += OnJump;
        _character.OnLand += OnLand;
        _character.OnSwipeDown += OnSwipeDown;
        _character.OnSwipeUp += OnSwipeUp;
        _character.OnPartAttached += OnPartAttached;
        _character.OnPartDetached += OnPartDetached;
    }

    void OnDisable()
    {
        _character.OnJump -= OnJump;
        _character.OnLand -= OnLand;
        _character.OnSwipeDown -= OnSwipeDown;
        _character.OnSwipeUp -= OnSwipeUp;
        _character.OnPartAttached -= OnPartAttached;
        _character.OnPartDetached -= OnPartDetached;
    }

    void LateUpdate()
    {
        if (_character.moveDirection > 0) transform.localScale = Vector3.one;
        else if (_character.moveDirection < 0) transform.localScale = new Vector3(-1, 1, 1);

        bool move = _character.moveDirection != 0.0f;
        string torsoState = (legCount, _onAir) switch
        {
            (0, _) => "Stop",
            (1,false) when !move => "OneLegIdle",
            (2,false) when !move => "Idle",
            (1,false) => "Walk",
            (2,false)  => "Run",
            (_,true) => _character.isFalling ? "JumpDown" : "JumpUp",
            _ => "Undefined"
        };
        
        if (!IsPlaying(CharacterPartType.Torso, "Land") || legCount < 2)
        {
            Play(CharacterPartType.Torso, torsoState);
        }

        var torsoSyncParts = armLegTypes.Where(
            partType => !IsPlaying(partType, "FSwipeUp") &&
                         !IsPlaying(partType, "FSwipeDown") &&
                         !IsPlaying(partType, "BSwipeUp") &&
                         !IsPlaying(partType, "BSwipeDown")
            ).ToList();
        
        if (torsoState == "OneLegIdle")
            torsoSyncParts.ForEach(e => Play(e, "Idle"));
        else if(torsoState == "Stop")
            torsoSyncParts.ForEach(e => Play(e, move ? "Walk" : "Idle"));
        else
            torsoSyncParts.ForEach(e => SyncAnimationState(e, CharacterPartType.Torso));
    }
    void OnSwipeUp(CharacterPartType partType) => OnSwipe(partType, true);
    void OnSwipeDown(CharacterPartType partType) => OnSwipe(partType, false);

    void OnSwipe(CharacterPartType partType, bool isUp)
    {
        if (partType != CharacterPartType.BackArm && partType != CharacterPartType.FrontArm) return;
        bool isFrontArm = partType == CharacterPartType.FrontArm;
        string state = isUp
            ? (isFrontArm ? "FSwipeUp" : "BSwipeUp")
            : (isFrontArm ? "FSwipeDown" : "BSwipeDown");
        Play(CharacterPartType.BackArm, state , false);
        Play(CharacterPartType.FrontArm, state , false);
        UpdateAnimator(CharacterPartType.BackArm);
        UpdateAnimator(CharacterPartType.FrontArm);
    }


    void OnJump()
    {
        _onAir = true;
    }

    void OnLand()
    {
        Play(CharacterPartType.Torso, "Land");
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

    void Play(CharacterPartType partType, string stateName, bool dontPlayIfAlreadyPlaying = true) =>
        Play(partType, Animator.StringToHash(stateName), dontPlayIfAlreadyPlaying);

    void Play(CharacterPartType partType, int stateHash, bool dontPlayIfAlreadyPlaying = true)
    {
        if (!_parts[partType].enabled) return;
        var animator = _parts[partType].animator;
        if (!animator.HasState(0, stateHash)) return;

        var state = animator.GetCurrentAnimatorStateInfo(0);
        if (dontPlayIfAlreadyPlaying && state.shortNameHash == stateHash) return;

        animator.Play(stateHash);
    }

    void UpdateAnimator(CharacterPartType partType)
    {
        if (!_parts[partType].enabled) return;
        _parts[partType].animator.Update(0);
    }

    bool IsPlaying(CharacterPartType partType, string stateName, bool considerFinishedAsPlaying = false) =>
        IsPlaying(partType, Animator.StringToHash(stateName));
    bool IsPlaying(CharacterPartType partType, int stateHash, bool considerFinishedAsPlaying = false)
    {
        if (!_parts[partType].enabled) return false;
        var animator = _parts[partType].animator;
        if (!animator.HasState(0, stateHash)) return false;
        
        var state = animator.GetCurrentAnimatorStateInfo(0);
        if(!considerFinishedAsPlaying && state.normalizedTime >= 1f) return false;

        return state.shortNameHash == stateHash;
    }

    void SyncAnimationState(CharacterPartType syncTo, CharacterPartType syncFrom)
    {
        if (syncFrom == syncTo) return;
        if (!_parts[syncFrom].enabled || !_parts[syncTo].enabled) return;

        var fromState = _parts[syncFrom].animator.GetCurrentAnimatorStateInfo(0);
        var toAnimator = _parts[syncTo].animator;

        if (!toAnimator.HasState(0, fromState.shortNameHash)) return;

        toAnimator.Play(fromState.shortNameHash, 0, fromState.normalizedTime);
    }
    void SyncAnimationTiming(CharacterPartType syncTo, CharacterPartType syncFrom)
    {
        if (syncFrom == syncTo) return;
        if (!_parts[syncFrom].enabled || !_parts[syncTo].enabled) return;

        var toAnimator = _parts[syncTo].animator;
        var fromState = _parts[syncFrom].animator.GetCurrentAnimatorStateInfo(0);
        var toState = _parts[syncTo].animator.GetCurrentAnimatorStateInfo(0);
        
        toAnimator.Play(toState.shortNameHash, 0, fromState.normalizedTime);
    }
}