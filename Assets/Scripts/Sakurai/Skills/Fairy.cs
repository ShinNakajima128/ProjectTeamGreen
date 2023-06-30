using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フェアリーコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Fairy : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("現在の妖精の攻撃力")]
    [SerializeField]
    private float _currentAttackAmount = 0;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //タグがEnemyならダメージを与える
        if (collision.CompareTag(GameTag.Enemy))
        {
           var target = GetComponent<IDamagable>();

            target.Damage(_currentAttackAmount);
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// 妖精に攻撃力をもたせる。
    /// </summary>
    /// <param name="amount">スキルデータから受け取る攻撃力</param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// スキルレベル3になったらサイズ変更
    /// </summary>
    /// <param name="scaleFactor">変更サイズ</param>
    public void SizaChange(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
    #endregion

    #region private method
    #endregion
}