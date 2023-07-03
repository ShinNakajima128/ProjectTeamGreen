using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オーラコンポーネント
/// </summary>
public class Aura : MonoBehaviour
{
    #region property
    #endregion

    #region serialize

    [Tooltip("現在のボールの攻撃力")]
    [SerializeField]
    private float _currentAttackAmount = 0;
    #endregion

    #region private

    //現在のオーラのスケール
    private float _currentScale = 1.0f;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(GameTag.Enemy))
        {
            var target = collision.GetComponent<IDamagable>();

            if (target != null)
            {
                target.Damage(_currentAttackAmount);
            }
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// オーラに攻撃力を持たせる。
    /// </summary>
    /// <param name="amount">スキルデータから受け取る攻撃力</param>
    public void SetAttackAmount(float amount)
    {
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// レベルが上がるごとにスケールを1変更。
    /// </summary>
    /// <param name="scaleFactor"></param>
    public void SizeChange(float scaleFactor)
    {
        _currentScale += scaleFactor;
        transform.localScale = new Vector3(_currentScale, _currentScale, 0);
    }
    #endregion

    #region private method
    #endregion
}