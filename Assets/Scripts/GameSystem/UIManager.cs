using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIObj
{
    public string name;
    public GameObject Obj;
    
}

[System.Serializable]
public class BodyImgs
{
    public BodyPartType partType;
    public int degradeLv = 4;
    public Sprite head;
    public Sprite arm;
    public Sprite leg;
}

public class UIManager : MonoBehaviour
{
    
    public static UIManager instance;
    
    public List<BodyImgs> bodyImgs;

    [Header("BodyPart Imgs")] 
    private SpriteRenderer head;
    private SpriteRenderer Larm;
    private SpriteRenderer Rarm;
    private SpriteRenderer Lleg;
    private SpriteRenderer Rleg;

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
        switch (type)
        {
            case BodyPartType.LeftArm:
                Larm.sprite = bodyImgs[degradeLv].arm;
                break;
            case BodyPartType.Head:
                head.sprite = bodyImgs[degradeLv].head;
                break;
            case BodyPartType.LeftLeg:
                Lleg.sprite = bodyImgs[degradeLv].leg;
                break;
            case BodyPartType.RightArm:
                Rarm.sprite = bodyImgs[degradeLv].arm;
                break;
            case BodyPartType.RightLeg:
                Rleg.sprite = bodyImgs[degradeLv].leg;
                break;
        }
    }
}
