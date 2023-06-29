using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

public class Boul : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _moveSpeed = 1.0f;
    #endregion

    #region private
    private Rigidbody2D _rb;
    private float _currentAttackAmount = 0;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float angle = Random.Range(0, 360);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        _rb.velocity = direction * _moveSpeed;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy))
        {
            var target = GetComponent<IDamagable>();

            target.Damage(_currentAttackAmount);
        }
    }

    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
