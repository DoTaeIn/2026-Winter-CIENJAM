using System;

public interface IAnimatableCharacter
{
    public bool isFalling { get; }
    public float moveDirection { get; }
    
    public event Action OnJump;
    public event Action OnLand;

    public event Action<CharacterPartType> OnSwipeDown;
    public event Action<CharacterPartType> OnSwipeUp;
    public event Action<CharacterPartType> OnPartAttached;
    public event Action<CharacterPartType> OnPartDetached;
}