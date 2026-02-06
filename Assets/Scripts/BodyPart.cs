using UnityEngine;
using System;

public enum BodyPartType { LeftArm, RightArm, LeftLeg, RightLeg } // Head, Torso 추가 고려


public class BodyPart : MonoBehaviour
{
    public BodyPartType partType; // 어떤 부위인지
    float MaxHp = 50.0f;
    private float currHp;
    public bool isBroken;

    public event Action<BodyPartType> OnPartBroken;
    public event Action<BodyPartType> OnPartRestore;

    public void Init()
    {
        currHp = MaxHp;
        isBroken = false;
    }

    // 데미지 받기
    public void TakeDamage(float dmg)
    {
        if (isBroken) return;
        else
        {
            currHp -= dmg;
            if(currHp <= 0)
            {
                currHp = 0;
                isBroken = true;
                OnPartBroken?.Invoke(partType);
            }
        }
    }

    // 회복
    void healHp(float amount)
    {
        if (isBroken) 
        {
            isBroken = false;
            // 부위 복구 시 필요한 다른 로직
        }
        else
        {
            currHp += amount;
        }
    }

    // 부위 액션
    void PerformAction()
    {

    }
}
