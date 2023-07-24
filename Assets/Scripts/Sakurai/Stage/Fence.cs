using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// フェンスプレハブの機能
/// </summary>
public class Fence : MonoBehaviour
{
    #region serialize
    [Header("変数")]
    [Tooltip("プレイヤーに与えるの攻撃力")]
    [SerializeField]
    private float _attackAmount = 0f;
    #endregion

    #region private
    private float _currentAttackAmount = 5.0f;
    #endregion

    #region unity methods
    private void Start()
    {
        _attackAmount = _currentAttackAmount;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GameTag.Player))
        {
            //インターフェースを取得
            var target = collision.gameObject.GetComponent<IDamagable>();

            if (target != null)
            {
                //現在の攻撃力分ダメージを与える。
                target.Damage(_currentAttackAmount);
                target.Knockback(transform);
            }
        }
    }
    #endregion
}