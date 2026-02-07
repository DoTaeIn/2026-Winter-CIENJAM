using System;

[Serializable]
public struct CharacterParts<T>
{
    public T backArm;
    public T backLeg;
    public T frontArm;
    public T frontLeg;
    public T torso;

    public T this[CharacterPartType partType]
    {
        get => partType switch
        {
            CharacterPartType.BackArm => backArm,
            CharacterPartType.BackLeg => backLeg,
            CharacterPartType.FrontArm => frontArm,
            CharacterPartType.FrontLeg => frontLeg,
            CharacterPartType.Torso => torso,
            _ => default
        };
        set
        {
            switch (partType)
            {
                case CharacterPartType.BackArm:
                    backArm = value;
                    break;
                case CharacterPartType.BackLeg:
                    backLeg = value;
                    break;
                case CharacterPartType.FrontArm:
                    frontArm = value;
                    break;
                case CharacterPartType.FrontLeg:
                    frontLeg = value;
                    break;
                case CharacterPartType.Torso:
                    torso = value;
                    break;
            }
        }
    }

}