using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterAnimationAsset", menuName = "SO/Character Animation Asset")]
public class CharacterAnimationAsset : ScriptableObject
{
    public CharacterParts<AnimationClip> idle;
    public CharacterParts<AnimationClip> walk;
    public CharacterParts<AnimationClip> run;
    public CharacterParts<AnimationClip> jumpUp;
    public CharacterParts<AnimationClip> jumpDown;
    public CharacterParts<AnimationClip> land;
}