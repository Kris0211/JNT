using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Assertions;

public class Coin : MonoBehaviour
{
    public UnityEvent<GameObject> pickedUp;

    [SerializeField]
    private CircleCollider2D _collider;

    [Header("Animation")]
    public float duration = 1f;

    private Tween _tween;

    void Awake()
    {
        if (_collider == null)
        {
            _collider = GetComponent<CircleCollider2D>();
        }
        Assert.IsNotNull(_collider);
    }

    void OnEnable()
    {
        // Restart tween to play spin animation.
        PlayAnimation();
    }

    void OnDisable()
    {
        _tween?.Kill();
    }

    private void PlayAnimation()
    {
        transform.localScale = new Vector3(1f, 1f, 1f);
        _tween = transform.DOScaleX(-1, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only detect Player.
        if (!other.CompareTag("Player")) return;

        pickedUp?.Invoke(gameObject);
    }
}
