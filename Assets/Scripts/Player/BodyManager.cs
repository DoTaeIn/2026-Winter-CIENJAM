using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;


public class BodyManager : MonoBehaviour
{

    [Header("Body Parts")]
    public List<BodyPart> bodyParts = new List<BodyPart>();
    private Dictionary<BodyPartType, int> bodyPartDegradeLv = new Dictionary<BodyPartType, int>()
    {
        { BodyPartType.LeftArm, 4 },
        { BodyPartType.RightArm, 4 },
        { BodyPartType.LeftLeg, 4 },
        { BodyPartType.RightLeg, 4 },
        { BodyPartType.Head, 4 }
    };
    
    //private float height; //raycast �� Ű ����, �ٸ� �ı��Ǹ� ���� ����
    

    private void Start()
    {
        // bodypart���� �ʱ�ȭ & �̺�Ʈ ����
        foreach (var part in bodyParts)
        {
            part.Init();
            part.OnPartBroken += HandlePartBroken; // �ı� �̺�Ʈ ����
            part.OnPartRestore += HandlePartRestore; // ���� �̺�Ʈ ����
        }
    }

    private void OnEnable()
    {
        foreach (var part in bodyParts)
            part.onPartBrokenEvent.AddListener(HandlePartDegraded);
    }

    private void OnDisable()
    {
        foreach (var part in bodyParts)
            part.onPartBrokenEvent.RemoveListener(HandlePartDegraded);
    }

    private void HandlePartDegraded(BodyPartType type)
    {
        int lv = bodyPartDegradeLv[type] - 1;
        UIManager.instance.DegradePart(type, lv);
    }

    // ������ ó��
    public void ApplyDamageToPart(BodyPartType type, float damage)
    {
        var part = bodyParts.Find(p => p.partType == type);
        if (part != null)
        {
            part.TakeDamage(damage);
        }
    }

    // ���� �ı� �� ó��
    private void HandlePartBroken(BodyPartType type)
    {
        OnBodyPartBroken?.Invoke(type);
    }

    // ���� ���� �� ó��
    private void HandlePartRestore(BodyPartType type)
    {
        OnBodyPartRestored?.Invoke(type);
    }

    // --- ��� ���� API ---
    // �ٸ� ���¿� ���� �̵� �ӵ� ���� ��ȯ
    public float GetMovementSpeedMultiplier()
    {
        bool leftLegOk = !GetPart(BodyPartType.LeftLeg).isBroken;
        bool rightLegOk = !GetPart(BodyPartType.RightLeg).isBroken;

        if (leftLegOk && rightLegOk) return 1.0f; // �Ѵ� ���� : 100%
        if (leftLegOk || rightLegOk) return 0.5f; // �ٸ� �ϳ� : 50% �ӵ�
        return 0.1f; // �Ѵ� �λ� : 10% �ӵ�
        // �μ�or�Ѽ� ���̵� �ټ�����
    }

    // ���� ��� ������ ���� ���� ��ȯ
    // ���߿� ��� ���� ��� �������� Ȯ���� �� ��� ���� 
    public int GetWorkingArmCount()
    {
        int count = 0;
        if (!GetPart(BodyPartType.LeftArm).isBroken) count++;
        if (!GetPart(BodyPartType.RightArm).isBroken) count++;
        return count;
    }


    // Getters
    public BodyPart GetPart(BodyPartType type)
    { 
        return bodyParts.Find(p => p.partType == type); 
    }

    //public float GetHeight()
    //{
    //    return height;
    //}
}
