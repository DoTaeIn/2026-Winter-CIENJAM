using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIObj
{
    public string name;
    public GameObject Obj;
    
}

public enum ImageType
{
    Head,
    Arm,
    Leg
}

[System.Serializable]
public class BodyImgs
{
    public ImageType partType;
    public List<Sprite> sprites;    
}

public class UIManager : MonoBehaviour
{
    
    public static UIManager instance;
    
    public List<BodyImgs> bodyImgs;

    [Header("BodyPart Imgs")] 
    [SerializeField] private Image head;
    [SerializeField] private Image Larm;
    [SerializeField] private Image Rarm;
    [SerializeField] private Image Lleg;
    [SerializeField] private Image Rleg;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        
        
    }
    
    [SerializeField] private List<UIObj> UIs;
    
    public void TogglePanel(string name)
    {
        GameObject go = FindPanel(name).Obj;
        if (go != null)
        {
            go.SetActive(!go.activeSelf);
        }
    }
    
    private UIObj FindPanel(string name)
    {
        foreach (UIObj ui in UIs)
            if (ui.name == name)
                return ui;
        return null;
    }

    public void DegradePart(BodyPartType type, int degradeLv)
    {
        degradeLv = Math.Clamp(degradeLv, 0, 4);

        // 1. BodyPartType을 ImageType(Arm, Leg, Head)으로 변환
        ImageType targetImageType = type switch
        {
            BodyPartType.LeftArm or BodyPartType.RightArm => ImageType.Arm,
            BodyPartType.LeftLeg or BodyPartType.RightLeg => ImageType.Leg,
            BodyPartType.Head => ImageType.Head // 나머지는 Head로 가정
        };

        // 2. 변환된 타입으로 리스트에서 검색
        BodyImgs target = bodyImgs.Find(x => x.partType == targetImageType);
    
        // (안전장치) 찾는 데이터가 없으면 함수 종료
        if (target == null) 
        {
            Debug.LogError($"이미지 데이터를 찾을 수 없습니다: {targetImageType}");
            return; 
        }

        // 3. 스프라이트 적용
        switch (type)
        {
            case BodyPartType.Head:
                head.sprite = target.sprites[degradeLv];
                break;
            case BodyPartType.LeftArm:
                Larm.sprite = target.sprites[degradeLv];
                break;
            case BodyPartType.RightArm:
                Rarm.sprite = target.sprites[degradeLv];
                break;
            case BodyPartType.LeftLeg:
                Lleg.sprite = target.sprites[degradeLv]; 
                break;
            case BodyPartType.RightLeg:
                Rleg.sprite = target.sprites[degradeLv];
                break;
        }
    }

    public void SelectPN(bool isPositive)
    {
        //player -> currNPC -> NPC Perform PN
    }
}
