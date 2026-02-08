using System;
using System.Collections.Generic;
using TMPro;
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
    
    [Header("Body parts UI")]
    [SerializeField] private TMP_Text headTxt;
    [SerializeField] private TMP_Text lArmTxt;
    [SerializeField] private TMP_Text rArmTxt;
    [SerializeField] private TMP_Text lLegTxt;
    [SerializeField] private TMP_Text rLegTxt;

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

    public void DegradePart(BodyPartType type, float degradeLv)
    {
        int lv = Math.Clamp((int)(degradeLv / 10), 0, 4);

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
        
        string[] hexColors = new string[] { "#FFFFFF", "#FFD3D3", "#FF9494", "#FF4F4F", "#E50000" };
        Color color = new  Color32(255, 255, 255, 255);
        if (ColorUtility.TryParseHtmlString(hexColors[4-lv], out Color newColor))
        {
            color = newColor;
        }
        switch (type)
        {
            case BodyPartType.Head:
                head.sprite = target.sprites[lv];
                headTxt.text = $"{new[] { "파괴됨", "골절", "출혈 중", "가벼운 뇌진탕", "건강함" }[lv]} ({(degradeLv / 40.0f * 100):F0}%)";
                headTxt.color = color;
                break;
            case BodyPartType.LeftArm:
                Larm.sprite = target.sprites[lv];
                lArmTxt.text = $"{new[] { "파괴됨", "골절", "찢어짐", "찰과상", "튼튼함" }[lv]} ({(degradeLv / 40.0f * 100):F0}%)";
                lArmTxt.color = color;
                break;
            case BodyPartType.RightArm:
                Rarm.sprite = target.sprites[lv];
                rArmTxt.text = $"{new[] { "파괴됨", "골절", "찢어짐", "찰과상", "튼튼함" }[lv]} ({(degradeLv / 40.0f * 100):F0}%)";
                rArmTxt.color = color;
                break;
            case BodyPartType.LeftLeg:
                Lleg.sprite = target.sprites[lv];
                lLegTxt.text = $"{new[] { "파괴됨", "골절", "깊은 상처", "염좌", "튼튼함" }[lv]} ({(degradeLv / 40.0f * 100):F0}%)";
                lLegTxt.color = color;
                break;
            case BodyPartType.RightLeg:
                Rleg.sprite = target.sprites[lv];
                rLegTxt.text = $"{new[] { "파괴됨", "골절", "깊은 상처", "염좌", "튼튼함" }[lv]} ({(degradeLv / 40.0f * 100):F0}%)";
                rLegTxt.color = color;
                break;
        }
    }

    public void SelectPN(bool isPositive)
    {
        //player -> currNPC -> NPC Perform PN
    }
}
