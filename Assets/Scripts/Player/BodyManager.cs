using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public event Action<BodyPartType> OnBodyPartBroken;
    public event Action<BodyPartType> OnBodyPartRestored;

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
            part.onPartDegradeEvent.AddListener(HandlePartDegraded);
    }

    private void OnDisable()
    {
        foreach (var part in bodyParts)
            part.onPartDegradeEvent.RemoveListener(HandlePartDegraded);
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

    // get dgrade level of body part
    public int GetDegradeLevel(BodyPartType type)
    {
        if (bodyPartDegradeLv.ContainsKey(type))
        {
            return bodyPartDegradeLv[type];
        }
        return 0; // 파괴된 부위면 0 취급
    }

    // return a random alive body part
    public BodyPartType? GetRandomActiveBodyPart()
    {
        // get alive body parts and make list
        List<BodyPartType> activeParts = bodyPartDegradeLv
            .Where(part => part.Value > 0)
            .Select(part => part.Key)
            .ToList();

        // if all parts broken? Dead
        if (activeParts.Count == 0)
        {
            return null;
        }

        // pick random active body part
        int randomIndex = UnityEngine.Random.Range(0, activeParts.Count);
        return activeParts[randomIndex];
    }

    public void ApplyDamageToRandom(float damage)
    {
        BodyPartType? targetPart = GetRandomActiveBodyPart();
        if (targetPart.HasValue)
        {
            ApplyDamageToPart(targetPart.Value, damage);
            Debug.Log($"적 공격! -> {targetPart.Value} 타격 성공");
        }
        else
        {
            Debug.Log("공격 실패: 이미 싸늘한 시체...");
        }
    }

    //public float GetHeight()
    //{
    //    return height;
    //}
}
