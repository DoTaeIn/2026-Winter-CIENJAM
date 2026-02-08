using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader
{
    static ScreenFader _instance;
    static ScreenFader instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ScreenFader();
            }
            return _instance;
        }
    }

    float _fadeAmount = 1f;
    float fadeAmount
    {
        get => _fadeAmount;
        set
        {
            _fadeAmount = value;
            _panelImage.color = new Color(0f, 0f, 0f, 1f-_fadeAmount);
        }
    }
    CancellationTokenSource _fadeCts;
    Image _panelImage;
    ScreenFader()
    {
        var canvas = GameObject.FindObjectsByType<Canvas>(FindObjectsSortMode.None)
            .FirstOrDefault(x => x.renderMode == RenderMode.ScreenSpaceOverlay);
        if (canvas == null)
            Debug.LogError("No Canvas with Screen Space - Overlay found in the scene.");
        
        var panelObject = new GameObject("ScreenFaderPanel");
        panelObject.transform.SetParent(canvas.transform, false);
        
        var rectTransform = panelObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        _panelImage = panelObject.AddComponent<Image>();
    }
    public static void FadeIn(float duration)
    {
        instance._fadeCts?.Cancel();
        instance._fadeCts = new CancellationTokenSource();
        instance.FadeAsync(0f, duration, instance._fadeCts.Token);
    }

    public static void FadeOut(float duration)
    {
        instance._fadeCts?.Cancel();
        instance._fadeCts = new CancellationTokenSource();
        instance.FadeAsync(1f, duration, instance._fadeCts.Token);
    }
    async Awaitable FadeAsync(float targetAlpha, float duration, CancellationToken token = default)
    {
        float startAlpha = fadeAmount;
        float time = 0;

        while (time < duration)
        {
            if (token.IsCancellationRequested) return;

            time += Time.deltaTime;
            fadeAmount = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            await Awaitable.NextFrameAsync(token);
        }
        fadeAmount = targetAlpha;
    }
}
