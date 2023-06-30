using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 敵の機能全般を管理するベースクラス。
/// このクラスは直接アタッチできないので、必ず継承して敵を作成してください。
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IDamagable
{
    #region property
    public EnemyActionType ActionType => _enemyData.ActionType;
    public bool IsInvincible => _isInvincible;
    public float ApproachDistance => _enemyData.ApproachDistance;
    #endregion

    #region serialize
    [Tooltip("敵のデータ")]
    [SerializeField]
    private EnemyData _enemyData = default;
    #endregion

    #region protected
    /// <summary>現在のHP</summary>
    protected float _currentHP;
    /// <summary>現在の攻撃力</summary>
    protected float _currentAttackAmount;
    /// <summary>行動可能かどうか</summary>
    protected bool _isActionable = false;
    /// <summary>無敵状態かどうか</summary>
    protected bool _isInvincible = false;
    /// <summary>敵の画像を描写するRenderer</summary>
    protected SpriteRenderer _enemyRenderer;
    protected Transform _playerTrans;
    #endregion

    #region private
    private bool _init = false;
    private IDamagable _target;
    private Coroutine _coroutine;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    protected virtual void Awake()
    {
        _currentHP = _enemyData.HP;
        _currentAttackAmount = _enemyData.AttackAmount;
        _enemyRenderer = GetComponent<SpriteRenderer>();
        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;
        _init = true;

        //テスト処理
        _isActionable = true;
    }

    protected virtual void Start()
    {
        _coroutine = StartCoroutine(OnActionCoroutine());

        //プレイヤーと接触した時の処理を登録する
        this.OnCollisionEnter2DAsObservable()
            .Where(x => x.gameObject.CompareTag(GameTag.Player))
            //一度接触していればGetComponentを行わないようにする
            .Select(x => _target ?? (_target = x.gameObject.GetComponent<IDamagable>()))
            .Subscribe(x =>
            {
                if (!x.IsInvincible)
                {
                    x.Damage(_currentAttackAmount);
                }
            })
            .AddTo(this);
    }

    protected virtual void OnEnable()
    {
        if (_init)
        {
            Setup();

            _coroutine = StartCoroutine(OnActionCoroutine());
        }
    }

    protected virtual void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    //protected virtual void OnTriggerEnter2D(Collider2D other)
    //{
    //    //プレイヤーにヒットした場合
    //    if (other.CompareTag(GameTag.Player))
    //    {
    //        //インターフェースを通じてダメージ処理を実行
    //        if (TryGetComponent(out IDamagable target))
    //        {
    //            //無敵状態ではない場合はダメージを与える
    //            if (!target.IsInvincible)
    //            {
    //                target.Damage(_currentAttackAmount);
    //            }
    //        }
    //    }
    //}
    #endregion

    #region public method
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    public virtual void Damage(float amount)
    {
        _currentHP -= amount;

        Debug.Log(amount);
        
        if (_currentHP <= 0)
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"{gameObject.name}:Damage、残りHP:{_currentHP}");
    }
    #endregion

    #region protected method
    #endregion

    #region private method
    private void Setup()
    {
        _currentHP = _enemyData.HP;
        _enemyRenderer.color = Color.white;
    }

    private void ChangeAttackAmount(int coefficient)
    {
        _currentAttackAmount = coefficient;
    }
    #endregion
    /// <summary>
    /// 敵毎のアクションの処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator OnActionCoroutine();
}
