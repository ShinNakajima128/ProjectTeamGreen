using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵の機能全般を管理するベースクラス。
/// このクラスは直接アタッチできないので、必ず継承して敵を作成してください。
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    #region property
    public int CurrentHp => _currentHp;

    public bool IsInvincible => _isInvincible;
    #endregion

    #region serialize
    [SerializeField]
    private EnemyData _enemyData = default;
    #endregion

    #region protected
    /// <summary>
    /// 現在のHP。実行中はこの変数を扱う 
    /// </summary>
    protected int _currentHp;
    /// <summary>
    /// 現在の攻撃力。実行中はこの変数を扱う 
    /// </summary>
    protected int _currentAttackAmount;
    protected bool _isActived = false;
    protected bool _isInvincible = false;
   
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected virtual void Awake()
    {
        _currentHp = _enemyData.HP;
        _currentAttackAmount = _enemyData.AttackAmount;
    }

    protected virtual void Start()
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //プレイヤーにヒットした場合
        if (other.CompareTag(GameTag.Player))
        {
            //インターフェースを通じてダメージ処理を実行
            if (TryGetComponent(out IDamagable target))
            {
                //体力が0ではない且つ無敵状態ではない場合はダメージを与える
                if (target.CurrentHp > 0 && !target.IsInvincible)
                {
                    target.Damage(_currentAttackAmount);
                }
            }
        }
    }

    public void Damage(int amount)
    {
        throw new System.NotImplementedException();
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
