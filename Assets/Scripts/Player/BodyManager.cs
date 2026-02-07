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
    
    //private float height; //raycast 용 키 높이, 다리 파괴되면 변동 가능
    

    private void Start()
    {
        // bodypart마다 초기화 & 이벤트 연결
        foreach (var part in bodyParts)
        {
            part.Init();
            part.OnPartBroken += HandlePartBroken; // 파괴 이벤트 연결
            part.OnPartRestore += HandlePartRestore; // 복구 이벤트 연결
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

    // 데미지 처리
    public void ApplyDamageToPart(BodyPartType type, float damage)
    {
        var part = bodyParts.Find(p => p.partType == type);
        if (part != null)
        {
            part.TakeDamage(damage);
        }
    }

    // 부위 파괴 시 처리
    private void HandlePartBroken(BodyPartType type)
    {

    }

    // 부위 복구 시 처리
    private void HandlePartRestore(BodyPartType type)
    {
        
    }

    // --- 기능 제공 API ---
    // 다리 상태에 따라 이동 속도 배율 반환
    public float GetMovementSpeedMultiplier()
    {
        bool leftLegOk = !GetPart(BodyPartType.LeftLeg).isBroken;
        bool rightLegOk = !GetPart(BodyPartType.RightLeg).isBroken;

        if (leftLegOk && rightLegOk) return 1.0f; // 둘다 멀쩡 : 100%
        if (leftLegOk || rightLegOk) return 0.5f; // 다리 하나 : 50% 속도
        return 0.1f; // 둘다 부상 : 10% 속도
        // 두손or한손 차이도 줄수있음
    }

    // 현재 사용 가능한 손의 개수 반환
    // 나중에 양손 무기 사용 가능한지 확인할 떄 사용 가능 
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
