using DG.Tweening;
using UnityEngine;

public class WobbleEffect : MonoBehaviour
{
    [Range(1f, 20f)]
    public float wobbleIntensity = 10f;

    [Range(0.1f, 5f)]
    public float wobbleDuration = 0.5f;

    private Sequence _tweenSequence;

    void Start() 
    {
        _tweenSequence = DOTween.Sequence();
        float stepDuration = wobbleDuration / 2f;

        _tweenSequence.Append(transform.DORotate(new Vector3(0, 0, wobbleIntensity), stepDuration).SetEase(Ease.OutSine))
            .Append(transform.DORotate(new Vector3(0, 0, 0), stepDuration).SetEase(Ease.InSine))
            .Append(transform.DORotate(new Vector3(0, 0, -wobbleIntensity), stepDuration).SetEase(Ease.OutSine))
            .Append(transform.DORotate(new Vector3(0, 0, 0), stepDuration).SetEase(Ease.InSine))
            .SetLoops(-1);  // Loop indefinitely
    }

    public void StopTweens()
    {
        _tweenSequence.Kill();
    }
}
