using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オーラオブジェクトの機能
/// </summary>
public class Aura : MonoBehaviour
{

    #region serialize
    [Header("変数")]
    [Tooltip("現在のボールの攻撃力")]
    [SerializeField]
    private float _currentAttackAmount = 0;
    #endregion

    #region private
    /// <summary>現在のオーラの大きさ</summary>
    private float _currentScale = 1.0f;
    private bool _isCanAttack = false;
    #endregion

    #region unity methods
    private void OnEnable()
    {
        StartCoroutine(DamageIntervalCoroutine());
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_isCanAttack)
        {
            return;
        }

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
    /// オーラに攻撃力を持たせる。
    /// </summary>
    /// <param name="amount">スキルデータから受け取る攻撃力</param>
    public void SetAttackAmount(float amount)
    {
        //現在の攻撃力
        _currentAttackAmount = amount;
    }

    /// <summary>
    /// レベルが上がるごとにスケールを1変更。
    /// </summary>
    /// <param name="scaleFactor">レベルが上がったときにプラスさせる値を受け取る</param>
    public void SizeChange(float scaleFactor)
    { 
        _currentScale += scaleFactor;
        transform.localScale = new Vector3(_currentScale, _currentScale, 0);
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// ダメージを与える間隔を調整するコルーチン
    /// </summary>
    private IEnumerator DamageIntervalCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.1f);

            _isCanAttack = true;

            yield return null;
            yield return null;

            _isCanAttack = false;
        }
    }
    #endregion
}