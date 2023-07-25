using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// 敵全般を管理するManagerクラス
/// </summary>
[RequireComponent(typeof(EnemyGenerator))]
public class EnemyManager : MonoBehaviour
{
    #region property
    public static EnemyManager Instance { get; private set; }
    public ReactiveProperty<uint> DefeatAmount => _defeatAmountProperty;
    /// <summary>
    /// 討伐数を表示する処理の購読のみ可能なプロパティ
    /// </summary>
    public IObservable<uint> DefeatedEnemyAmountViewObserver => _defeatedEnemyAmountViewSubject;

    public EnemyBulletGenerator MiddleBossPoolGenerator => _middleBossPoolGenerator;

    public EnemyBulletGenerator TurretPoolGenerator => _turretPoolGenerator;

    public EnemyBulletGenerator WaterMelonBulletPoolGenerator => _waterMelonBulletPoolGenerator;

    public RockGenerator RockPoolGenerator => _rockPoolGenerator;

    public BigBossBulletGenerator PotatoPoolGenerator => _potatoPoolGenerator;

    public BigBossEffects BigBossEffects => _bigBossEffects;

    public MissileGenerator MissilePoolGenerator => _missilePoolGenerator;
    public IObservable<float> ChangeEnemyStatusCoefficientObserver => _changeEnemyStatusCoefficientSubject;
    public IObservable<Unit> DefeatedBossObserver => _defeatedBossSubject;
    public IObservable<string> UpdateBossNameObserver => _updateBossNameSubject;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("中ボスのバレットをプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerator _middleBossPoolGenerator;

    [Tooltip("タレットのバレット(レモン)をプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerator _turretPoolGenerator;

    [Tooltip("タレットのバレット(スイカ)をプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerator _waterMelonBulletPoolGenerator;

    [Tooltip("中ボスタレットの岩をプールするオブジェクト")]
    [SerializeField]
    private RockGenerator _rockPoolGenerator;

    [Tooltip("中ボスタレットのミサイルをプールするオブジェクト")]
    [SerializeField]
    private MissileGenerator _missilePoolGenerator;

    [Tooltip("大ボスエネミーのポテトをプールするオブジェクト")]
    [SerializeField]
    private BigBossBulletGenerator _potatoPoolGenerator;

    [Tooltip("大ボスに扱うエフェクト")]
    [SerializeField]
    private BigBossEffects _bigBossEffects;
    #endregion

    #region private
    private EnemyWaveType _currentEnemyWave = EnemyWaveType.Wave_1;
    /// <summary>敵の生成機能を持つコンポーネント</summary>
    private EnemyGenerator _generator;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<float> _changeEnemyStatusCoefficientSubject = new Subject<float>();
    /// <summary>討伐数</summary>
    private ReactiveProperty<uint> _defeatAmountProperty = new ReactiveProperty<uint>();
    /// <summary>討伐数を表示する処理のSubject</summary>
    private Subject<uint> _defeatedEnemyAmountViewSubject = new Subject<uint>();
    private Subject<Unit> _defeatedBossSubject = new Subject<Unit>();
    private Subject<string> _updateBossNameSubject = new Subject<string>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
        _generator = GetComponent<EnemyGenerator>();
    }

    private void Start()
    {
        //テスト生成
        StageManager.Instance.GameStartObserver
                    .TakeUntilDestroy(this)
                    .Subscribe(_ =>
                    {
                        OnGenerateEnemies(EnemyWaveType.Wave_1);
                    });

        //リセット時の処理を登録
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnReset());
        
        //討伐数が変化した時のイベント処理を登録
        _defeatAmountProperty
        .TakeUntilDestroy(this)
        .Subscribe(value =>
        {
            _defeatedEnemyAmountViewSubject.OnNext(value);
        });

        //指定された時間毎にボスを生成する処理を登録
        TimeManager.Instance.BossEventObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(value => BossGenerate(value));

        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .TakeUntilDestroy(this)
                                        .Subscribe(_ => _changeEnemyStatusCoefficientSubject.OnNext(1.01f));
   
    }
    #endregion

    #region public method
    public void OnDefeatedBossEnemyEvent()
    {
        _defeatedBossSubject.OnNext(Unit.Default);

        int currentWave = (int)_currentEnemyWave + 1;
        _currentEnemyWave = (EnemyWaveType)currentWave;
        OnGenerateEnemies(_currentEnemyWave);
    }

    /// <summary>
    /// ボスの名前を表示する
    /// </summary>
    /// <param name="name"></param>
    public void CurrentBossNameView(string name)
    {
        _updateBossNameSubject.OnNext(name);
    }
    #endregion

    #region private method
    private void OnGenerateEnemies(EnemyWaveType type)
    {
        switch (type)
        {
            case EnemyWaveType.Wave_1:
                _generator.OnEnemyGenerate(EnemyType.Wave1_Chase1);
                _generator.OnEnemyGenerate(EnemyType.Wave1_Point1);
                Debug.Log("Wave1開始");
                break;
            case EnemyWaveType.Wave_2:
                _generator.OnEnemyGenerate(EnemyType.Wave2_Chase1);
                _generator.OnEnemyGenerate(EnemyType.Wave2_Point1);
                Debug.Log("Wave2開始");
                break;
            case EnemyWaveType.Wave_3:
                _generator.OnEnemyGenerate(EnemyType.Wave3_Chase1);
                _generator.OnEnemyGenerate(EnemyType.Wave3_Chase2);
                Debug.Log("Wave3開始");
                break;
            default:
                break;
        }
    }
    private void BossGenerate(EnemyType bossType)
    {
        AudioManager.PlaySE(SEType.BossAlert);
        StartCoroutine(BossEventCoroutine(bossType));
    }

    private void OnReset()
    {
        _defeatAmountProperty.Value = 0;
        _currentEnemyWave = EnemyWaveType.Wave_1;
    }
    #endregion

    private IEnumerator BossEventCoroutine(EnemyType bossType)
    {
        
        yield return new WaitForSeconds(5.0f);
        
        _generator.StopEnemyGenerate();
        _generator.ReturnEnemies();

        _generator.BossGenerate(bossType);
    }
}

/// <summary>
/// 敵のウェーブの種類
/// </summary>
public enum EnemyWaveType
{
    Wave_1,
    Wave_2,
    Wave_3
}

[Serializable]
public class BigBossEffects
{
    public GameObject Smoke;
    public GameObject Target;
}