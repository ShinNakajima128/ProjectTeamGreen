using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]

/// <summary>
/// ボールオブジェクトの機能
/// </summary>
public class Boul : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("ボールのスピードの初期値")]
    [SerializeField]
    private float _moveSpeed = 10.0f;

    [Tooltip("現在のボールの攻撃力")]
    [SerializeField]
    private float _currentAttackAmount = 0;
    #endregion

    #region private
    private Rigidbody2D _rb;
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        //ボールが早くなってもコライダーの衝突検出可能にする。
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        float angle = Random.Range(0, 360);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        //ボール生成時は360度ランダムに発射。
        _rb.velocity = direction * _moveSpeed;
    }

    private void Update()
    {
        //弾の速度を一定に保つ。
        _rb.velocity = _rb.velocity.normalized * _moveSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たったのがエネミーの場合
        if (collision.CompareTag(GameTag.Enemy))
        {
            //インターフェースを取得
            var target = collision.GetComponent<IDamagable>();

            if (target != null)
            {
                //現在の攻撃力分ダメージを与える。
                target.Damage(_currentAttackAmount);
            }
        }
    }

    #endregion

    #region public method

    /// <summary>
    /// ボールに攻撃力を持たせる。
    /// </summary>
    /// <param name="amount">スキルデータから受け取る攻撃力</param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// ボールの速さを変更する。
    /// </summary>
    /// <param name="coefficient">速さに対する係数</param>
    public void MoveSpeedChange(float coefficient)
    {
        _moveSpeed *= coefficient;
    }
    #endregion

    #region private method
    #endregion
}
