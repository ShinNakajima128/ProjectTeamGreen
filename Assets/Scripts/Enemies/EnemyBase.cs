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
    public bool IsInvincible => _isInvincible;
    #endregion

    #region serialize
    [Tooltip("敵のデータ")]
    [SerializeField]
    private EnemyData _enemyData = default;
    #endregion

    #region protected
    /// <summary>現在のHP</summary>
    protected int _currentHp;
    /// <summary>現在の攻撃力</summary>
    protected int _currentAttackAmount;
    /// <summary>行動可能かどうか</summary>
    protected bool _isActionable = false;
    /// <summary>無敵状態かどうか</summary>
    protected bool _isInvincible = false;
    /// <summary>敵の画像を描写するRenderer</summary>
    protected SpriteRenderer _enemyRenderer;
    #endregion

    #region private
    private bool _init = false;
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
        _enemyRenderer = GetComponent<SpriteRenderer>();
        _init = true;
    }

    protected virtual void Start()
    {

    }

    protected void OnEnable()
    {
        if (_init)
        {
            Setup();
        }
    }

    protected void OnDisable()
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
                //無敵状態ではない場合はダメージを与える
                if (!target.IsInvincible)
                {
                    target.Damage(_currentAttackAmount);
                }
            }
        }
    }
    #endregion

    #region public method
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    public virtual void Damage(int amount)
    {
        _currentHp -= amount;

        if (_currentHp <= 0)
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"{gameObject.name}:Damage");
    }
    #endregion

    #region protected
    /// <summary>行動する</summary>
    protected abstract void OnAction();
    #endregion

    #region private method
    private void Setup()
    {
        _currentHp = _enemyData.HP;
        _enemyRenderer.color = Color.white;
    }
    #endregion
}
