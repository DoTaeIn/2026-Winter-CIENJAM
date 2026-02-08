using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BodyManager : MonoBehaviour
{

    [Header("Body Parts")] 
    public List<BodyPart> bodyParts = new List<BodyPart>();
    private List<BodyPart> bodyPartsDegradeLv;

    private PlayerMove move;

    public event Action<BodyPartType> OnBodyPartBroken;
    public event Action<BodyPartType> OnBodyPartRestored;

    //private float height; //raycast �� Ű ����, �ٸ� �ı��Ǹ� ���� ����

    private void Awake()
    {
        move =  GetComponent<PlayerMove>();
    }

    private void Start()
    {
        // bodypart���� �ʱ�ȭ & �̺�Ʈ ����
        foreach (var part in bodyParts)
        {
            part.Init();
            part.OnPartBroken += HandlePartBroken; // �ı� �̺�Ʈ ����
            part.OnPartRestore += HandlePartRestore; // ���� �̺�Ʈ ����
        }
        
        bodyPartsDegradeLv = GetComponentsInChildren<BodyPart>().ToList(); 
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

    private void HandlePartDegraded(BodyPartType type, float lv)
    {
        if (lv == 0)
        {
            CharacterPartType charPart = type switch
            {
                BodyPartType.LeftArm  => CharacterPartType.FrontArm,
                BodyPartType.RightArm => CharacterPartType.BackArm,
                BodyPartType.LeftLeg  => CharacterPartType.FrontLeg,
                BodyPartType.RightLeg => CharacterPartType.BackLeg,
                BodyPartType.Head     => CharacterPartType.Torso,
            };
            
            move.DetachPart(charPart);
        }

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

        if (leftLegOk && rightLegOk) return 1.0f;
        if (leftLegOk || rightLegOk) return 0.5f;
        return 0.1f;

    }


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
    
    // return a random alive body part
    public BodyPartType? GetRandomActiveBodyPart()
    {
        List<BodyPartType> activeParts = bodyPartsDegradeLv
            .Where(k => !k.isBroken)
            .Select(k => k.partType) // (주의) 여기 변수명이 partType인지 type인지 확인하세요!
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
