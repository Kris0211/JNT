using UnityEngine;
using UnityEngine.Assertions;
using DG.Tweening;

using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float colorChangeSpeed = 0.5f;
    public float colorFlashesCount = 3;

    [Space(4)]
    public TargetIndicator targetIndicator;

    [SerializeField]
    private Rigidbody2D _rb;

    void Awake()
    {
        if (_rb == null) // Unity objects should not use coalescing assignment.
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        Assert.IsNotNull(_rb); // Make sure the component is a part of our GameObject.

        if (targetIndicator == null)
        {
            targetIndicator = GetComponentInChildren<TargetIndicator>();
        }
        Assert.IsNotNull(targetIndicator);
    }

    public void Move(Vector2 dir)
    {
        _rb.velocity = dir.normalized * moveSpeed;
    }

    public void ChangeColor()
    {
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        { 
            spriteRenderer.DOColor(GetRandomColor(), colorChangeSpeed)
                .SetEase(Ease.Flash, colorFlashesCount);
        }
    }

    private Color GetRandomColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        return new Color(r, g, b);
    }
}
