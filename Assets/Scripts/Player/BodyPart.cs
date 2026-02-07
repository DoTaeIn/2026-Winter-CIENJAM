using UnityEngine;
using System;
using UnityEngine.Events;

public enum BodyPartType { LeftArm, RightArm, LeftLeg, RightLeg, Head } // Head, Torso �߰� ����


public class BodyPart : MonoBehaviour
{
    public BodyPartType partType; // � ��������
    float MaxHp = 50.0f;
    private float prevHp;
    private float currHp;
    public bool isBroken;

    public event Action<BodyPartType> OnPartBroken;
    public event Action<BodyPartType> OnPartRestore;

    public UnityEvent<BodyPartType> onPartDegradeEvent = new UnityEvent<BodyPartType>();

    public void Init()
    {
        currHp = MaxHp;
        isBroken = false;
    }

    // ������ �ޱ�
    public void TakeDamage(float dmg)
    {
        if (isBroken) return;
        else
        { 
            prevHp = currHp;
            currHp = prevHp - dmg;
            if(currHp <= 0)
            {
                currHp = 0;
                isBroken = true;
                OnPartBroken?.Invoke(partType);
            }
        }
        if(currHp / 10 != prevHp / 10) onPartDegradeEvent?.Invoke(partType);
    }

    // ȸ��
    void healHp(float amount)
    {
        if (isBroken) 
        {
            isBroken = false;
            // ���� ���� �� �ʿ��� �ٸ� ����
        }
        else
        {
            currHp += amount;
        }
    }

    // ���� �׼�
    void PerformAction()
    {

    }
}
