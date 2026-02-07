using UnityEngine;
using System;

public class CharacterAnimationTester : MonoBehaviour, IAnimatableCharacter
{
    public bool isFalling { get; private set; }
    public float moveDirection { get; private set; }

    public event Action OnJump;
    public event Action OnLand;

    public event Action<CharacterPartType> OnPartAttached;
    public event Action<CharacterPartType> OnPartDetached;

    void Update()
    {
        // Simple input handling for testing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFalling = true;
            OnJump?.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isFalling = false;
            OnLand?.Invoke();
        }

        moveDirection = Input.GetAxis("Horizontal");
    }
}
