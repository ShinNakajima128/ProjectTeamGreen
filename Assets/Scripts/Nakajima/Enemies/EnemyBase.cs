using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;

/// <summary>
/// 敵の機能全般を管理するベースクラス。
/// このクラスは直接アタッチできないので、必ず継承して敵を作成してください。
/// </summary>
public abstract class EnemyBase : MonoBehaviour, IDamagable, IPoolable
{
    #region property
    public EnemyType EnemyType => _enemyData.EnemyType;
    public EnemyActionType ActionType => _enemyData.ActionType;
    public bool IsInvincible => _isInvincible;
    public float ApproachDistance => _enemyData.ApproachDistance;

    public IObservable<Unit> InactiveObserver => _inactiveSubject;
    #endregion

    #region serialize
    [Tooltip("敵のデータ")]
    [SerializeField]
    private EnemyData _enemyData = default;

    [Tooltip("非表示になるプレイヤーとの距離")]
    [SerializeField]
    protected float _hideDistance = 10f;
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
    private float _currentMaxHP;
    private IDamagable _target;
    private Coroutine _coroutine;
    private Tween _currentTween;
    private DamageTextGenerator _damageTextGenerator;
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>非アクティブとなった時に通知を発行するSubject</summary>
    private Subject<Unit> _inactiveSubject = new Subject<Unit>();
    #endregion

    #region unity methods
    protected virtual void Awake()
    {
        _currentMaxHP = _enemyData.HP;
        _currentHP = _currentMaxHP;
        _currentAttackAmount = _enemyData.AttackAmount;
        _enemyRenderer = GetComponent<SpriteRenderer>();
        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;

        //テスト処理
        _isActionable = true;
    }

    protected virtual void Start()
    {
        _init = true;
        _coroutine = StartCoroutine(OnActionCoroutine());
        _damageTextGenerator = DamageTextManager.Instance.TextGenerator;

        //プレイヤーと接触した時の処理を登録する
        this.OnTriggerStay2DAsObservable()
            .TakeUntilDestroy(this)
            .Where(x => x.CompareTag(GameTag.Player))
            //一度接触していればGetComponentを行わないようにする
            .Select(x => _target ?? (_target = x.gameObject.GetComponent<IDamagable>()))
            .Subscribe(x =>
            {
                //プレイヤーがダメージを受けない状態ではない場合
                if (!x.IsInvincible)
                {
                    x.Damage(_currentAttackAmount);
                }
            });

        //プレイヤーから一定距離離れたらプールに戻る処理を登録
        Observable.Interval(TimeSpan.FromSeconds(3f))
                  .TakeUntilDestroy(this)
                  .Where(_ => gameObject.activeSelf)
                  .Subscribe(_ =>
                  {
                      if (Vector2.Distance(_playerTrans.position, transform.position) >= _hideDistance)
                      {
                          gameObject.SetActive(false);
                      }
                  });

        //ゲーム開始時の処理を登録
        StageManager.Instance.IsInGameObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => _isInvincible = !value);

        //ゲームリセット時の処理を登録
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => ResetParameter());
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

        _currentTween = null;

        _inactiveSubject.OnNext(Unit.Default);
    }
    #endregion

    #region public method
    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    public virtual void Damage(float amount)
    {
        if (_isInvincible)
        {
            return;
        }
        _currentHP -= amount;

        Debug.Log(amount);
        DamageAnimation();
        DamageTextGenerate(amount);

        if (_currentHP <= 0)
        {
            EffectManager.PlayEffect(EffectType.EnemyDied, transform.position);
            //討伐数を加算
            EnemyManager.Instance.DefeatAmount.Value++;
            ItemManager.Instance.GenerateItem(_enemyData.DropItemType, transform.position);
            AudioManager.PlaySE(SEType.Dead_Enemy);

            switch (_enemyData.EnemyType)
            {

                case EnemyType.Wave1_Boss:
                    //EnemyManager.Instance.OnDefeatedBossEnemyEvent();
                    StageManager.Instance.OnGameEnd();
                    break;
                case EnemyType.Wave2_Boss:
                    EnemyManager.Instance.OnDefeatedBossEnemyEvent();
                    break;
                case EnemyType.Wave3_Boss:
                    //EnemyManager.Instance.OnDefeatedBossEnemyEvent();
                    //ウェーブ3のボスが倒されたらゲーム終了
                    StageManager.Instance.OnGameEnd();
                    break;
                default:
                    break;
            }
            gameObject.SetActive(false);
        }
        else
        {
            if (amount >= 15.5f)
            {
                Knockback((transform.position - _playerTrans.position).normalized);
            }
            AudioManager.PlaySE(SEType.Damage_Enemy);
        }

        Debug.Log($"{gameObject.name}:Damage、残りHP:{_currentHP}");
    }
    public void SetEnemyStatus(float coefficient)
    {
        _currentAttackAmount *= coefficient;
        _currentMaxHP *= coefficient;
    }

    /// <summary>
    /// ノックバックする
    /// </summary>
    /// <param name="dir">吹き飛ぶ方向</param>
    public void Knockback(Vector2 dir)
    {
        transform.DOLocalMove(transform.localPosition + (Vector3)(dir * _enemyData.KnockbackAmount),
                              0.15f)
                 .SetEase(Ease.InQuad)
                 .SetLink(gameObject, LinkBehaviour.CompleteAndKillOnDisable);
    }
    #endregion

    #region protected method
    #endregion

    #region private method
    private void Setup()
    {
        _currentHP = _currentMaxHP;
        _enemyRenderer.color = Color.white;
        transform.localScale = Vector3.one;
    }

    private void DamageAnimation()
    {
        if (_currentTween == null)
        {
            _currentTween = transform.DOShakeScale(1.1f, 0.15f)
                                     .SetEase(Ease.InBounce);

            _enemyRenderer.DOColor(Color.red, 0.1f)
                          .SetLoops(2, LoopType.Yoyo)
                          .OnComplete(() =>
                          {
                              _enemyRenderer.color = Color.white;
                              transform.localScale = Vector3.one;
                              _currentTween = null;
                          });
        }
    }

    private void DamageTextGenerate(float amount)
    {
        DamageText textObj = _damageTextGenerator.DamageTextPool.Rent();

        if (textObj != null)
        {
            textObj.gameObject.SetActive(true);
            textObj.SetDamageText(transform, amount);
        }
    }

    /// <summary>
    /// 各パラメーターを初期値に戻す
    /// </summary>
    private void ResetParameter()
    {
        _currentMaxHP = _enemyData.HP;
        _currentHP = _currentMaxHP;
        _currentAttackAmount = _enemyData.AttackAmount;
    }
    #endregion
    /// <summary>
    /// 敵毎のアクションの処理を行うコルーチン
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator OnActionCoroutine();

    public void ReturnPool()
    {
        gameObject.SetActive(false);
    }
}
