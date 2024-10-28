using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 4.0f;

    [SerializeField]
    private Rigidbody2D _rb;

    void Awake()
    {
        if (_rb == null) // Unity objects should not use coalescing assignment.
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        Assert.IsNotNull(_rb); // Make sure the component is a part of our Player GameObject.
    }

    void Update()
    {
        float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");
        Vector2 dir = new(xDir, yDir);
        if (dir != Vector2.zero)
        {
            Move(dir);
        }
    }

    void Move(Vector2 dir)
    {
        _rb.velocity = dir * moveSpeed;
    }
}
