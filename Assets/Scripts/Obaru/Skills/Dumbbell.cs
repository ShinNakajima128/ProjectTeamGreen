using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダンベルコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Dumbbell : MonoBehaviour
{
    #region serialize
    [Tooltip("飛ぶ速さ")]
    [SerializeField]
    private float _moveSpeed = 3.0f;
    #endregion

    #region private
    /// <summary>現在の攻撃力</summary>
    private float _currentAttackAmount = 0;
    /// <summary>スキル持続時間</summary>
    private float _lifeTime = 5.0f;
    private Rigidbody2D _rb;
    private Coroutine _currentCoroutine = default;
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _currentCoroutine = StartCoroutine(InactiveCoroutine());
    }

    private void OnDisable()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }

        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy))
        {
            var target = collision.GetComponent<IDamagable>();

            target.Damage(_currentAttackAmount);
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// 攻撃力の設定
    /// </summary>
    /// <param name="amount"></param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// velocityの設定
    /// </summary>
    /// <param name="dir"></param>
    public void SetVelocity(Vector3 dir)
    {
        _rb.velocity = dir * _moveSpeed;
    }

    /// <summary>
    /// 弾の発射位置を設定
    /// </summary>
    /// <param name="parent"></param>
    public void SetShotPos(Transform parent)
    {
        transform.position = parent.position;
    }
    #endregion

    #region Coroutine method
    /// <summary>
    /// 一定時間後に非アクティブ化
    /// </summary>
    /// <returns></returns>
    private IEnumerator InactiveCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);
        gameObject.SetActive(false);
    }
    #endregion
}