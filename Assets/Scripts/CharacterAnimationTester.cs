using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class CharacterAnimationTester : MonoBehaviour, IAnimatableCharacter
{
    public bool isFalling { get; private set; }
    public float moveDirection { get; private set; }

    public event Action OnJump;
    public event Action OnLand;
    public event Action<CharacterPartType> OnSwipeDown;
    public event Action<CharacterPartType> OnSwipeUp;

    public event Action<CharacterPartType> OnPartAttached;
    public event Action<CharacterPartType> OnPartDetached;


    public float speed;
    public bool isDetaching = true;

    
    Rigidbody2D _rigidbody;
    bool _onAir;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = 0f;
        if (Keyboard.current.leftArrowKey.isPressed) moveDirection = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) moveDirection = 1f;
        _rigidbody.linearVelocityX = moveDirection * speed;
        
        isFalling = _rigidbody.linearVelocityY < 0f;
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) _rigidbody.linearVelocityY = 10f;
        
        Action<CharacterPartType> func = isDetaching ? OnPartDetached : OnPartAttached;
        if (Keyboard.current.aKey.wasPressedThisFrame) func?.Invoke(CharacterPartType.FrontArm);
        if (Keyboard.current.sKey.wasPressedThisFrame) func?.Invoke(CharacterPartType.BackArm);
        if (Keyboard.current.dKey.wasPressedThisFrame) func?.Invoke(CharacterPartType.FrontLeg);
        if (Keyboard.current.fKey.wasPressedThisFrame) func?.Invoke(CharacterPartType.BackLeg);

        if (Keyboard.current.qKey.wasPressedThisFrame) OnSwipeUp?.Invoke(CharacterPartType.FrontArm);
        if (Keyboard.current.wKey.wasPressedThisFrame) OnSwipeDown?.Invoke(CharacterPartType.FrontArm);
        if (Keyboard.current.eKey.wasPressedThisFrame) OnSwipeDown?.Invoke(CharacterPartType.BackArm);
        if (Keyboard.current.rKey.wasPressedThisFrame) OnSwipeUp?.Invoke(CharacterPartType.BackArm);

        GroundCheck();
    }

    void GroundCheck()
    {
        List<ContactPoint2D> contacts = new();
        _rigidbody.GetContacts(contacts);

        bool checkOnAir = true;
        foreach (var contact in contacts) 
            if (contact.normal.y > 0.5f)
            {
                checkOnAir = false;
                break;
            }

        if (checkOnAir != _onAir)
        {
            _onAir = checkOnAir;
            if (_onAir) OnJump?.Invoke();
            else OnLand?.Invoke();
        }
    }
}
