using System;
using UnityEngine;
using Unity.Properties;

public class DamageableColor : MonoBehaviour
{
    [Header("Property Settings")]
    public Component colorComponent;
    public string colorPropertyName;
    [Header("Color Settings")] 
    public Color normalColor;
    public Color damagedColor;
    public AnimationCurve mixAmountByTime;
    
    PropertyPath _colorPropertyPath;
    float _damageTimer = 0f;
    IDamageable _damageable;
    
    void Awake()
    {
        _colorPropertyPath = new(colorPropertyName);
        _damageable = GetComponent<IDamageable>();
    }
    
    void Update()
    {
        if (_damageTimer <= 0f || _damageTimer > mixAmountByTime.keys[^1].time)
        {
            SetColor(normalColor);
        }
        else
        {
            SetColor(EvaluateColor(_damageTimer));
        }
        _damageTimer += Time.deltaTime;
    }

    void OnEnable()
    {
        _damageable.OnDamaged += OnDamaged;
    }

    void OnDisable()
    {
        _damageable.OnDamaged -= OnDamaged;
    }

    void OnDamaged() => _damageTimer = 0f;

    void SetColor(Color color) => PropertyContainer.SetValue(colorComponent, _colorPropertyPath, color);
    Color EvaluateColor(float time)
    {
        float mix = Mathf.Clamp01(mixAmountByTime.Evaluate(time));
        Color mixColor = Color.Lerp(normalColor, damagedColor, mix * damagedColor.a);
        return new Color(mixColor.r, mixColor.g, mixColor.b, 1f);
    }
}
