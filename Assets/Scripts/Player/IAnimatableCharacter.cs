using System;

public interface IAnimatableCharacter
{
    public bool isFalling { get; }
    public float moveDirection { get; }

    public event Action OnJump;
    public event Action OnLand;

    public event Action<BodyPartType> OnPartAttached;
    public event Action<BodyPartType> OnPartDetached;
}