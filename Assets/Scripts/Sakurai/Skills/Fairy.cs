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
    #region serialize
    [Header("変数")]
    [Tooltip("現在の妖精の攻撃力")]
    [SerializeField]
    private float _currentAttackAmount = 0;
    #endregion

    #region unity methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たったのがエネミーだった場合
        if (collision.CompareTag(GameTag.Enemy))
        {
            //インターフェースを取得
            var target = collision.GetComponent<IDamagable>();

            if (target != null)
            {
                //現在の攻撃力分ダメージを与える。
                target.Damage(_currentAttackAmount);
                target.Knockback(transform);
            }
        }
        else if (collision.CompareTag(GameTag.VanishBullet))
        {
            collision.gameObject.SetActive(false);
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
    /// 現在のスキルレベルが3になったらサイズ変更
    /// </summary>
    /// <param name="scaleFactor">変更サイズ</param>
    public void SizeChange(float scaleFactor)
    {
        transform.localScale = new Vector3(scaleFactor, scaleFactor,0);
    }
    #endregion
}