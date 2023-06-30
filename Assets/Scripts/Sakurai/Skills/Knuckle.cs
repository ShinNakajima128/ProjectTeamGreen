using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用の仮の弾丸コンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Knuckle : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private float _moveSpeed = 3.0f;
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
        int dirType = Random.Range(0, 4);

        switch (dirType)
        {
            case 0:
                _rb.velocity = new Vector2(0, _moveSpeed);
                break;
            case 1:
                _rb.velocity = new Vector2(_moveSpeed, 0);
                break;
            case 2:
                _rb.velocity = new Vector2(0, -_moveSpeed);
                break;
            case 3:
                _rb.velocity = new Vector2(-_moveSpeed, 0);
                break;
            default:
                break;
        }
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
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }
    #endregion

    #region private method
    #endregion
}
