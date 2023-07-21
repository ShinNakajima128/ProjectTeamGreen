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

    public EnemyBulletGenerater MiddleBossPoolGenerator => _middleBossPoolGenerator;

    public EnemyBulletGenerater TurretPoolGenerator => _turretPoolGenerator;

    public RockGenerator RockPoolGenerator => _rockPoolGenerator;

    public MissileGenerator MissilePoolGenerator => _missilePoolGenerator;
    public IObservable<float> ChangeEnemyStatusCoefficientObserver => _changeEnemyStatusCoefficientSubject;
    public IObservable<Unit> DefeatedBossObserver => _defeatedBossSubject;
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("中ボスのバレットをプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerater _middleBossPoolGenerator;

    [Tooltip("タレットのバレットをプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerater _turretPoolGenerator;

    [Tooltip("中ボスタレットの岩をプールするオブジェクト")]
    [SerializeField]
    private RockGenerator _rockPoolGenerator;

    [Tooltip("中ボスタレットのミサイルをプールするオブジェクト")]
    [SerializeField]
    private MissileGenerator _missilePoolGenerator;
    #endregion

    #region private
    private EnemyWaveType _currentEnemyWave = EnemyWaveType.Wave_1;
    /// <summary>敵の生成機能を持つコンポーネント</summary>
    private EnemyGenerator _generator;
    private Subject<float> _changeEnemyStatusCoefficientSubject = new Subject<float>();
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>討伐数</summary>
    private ReactiveProperty<uint> _defeatAmountProperty = new ReactiveProperty<uint>();
    /// <summary>討伐数を表示する処理のSubject</summary>
    private Subject<uint> _defeatedEnemyAmountViewSubject = new Subject<uint>();
    private Subject<Unit> _defeatedBossSubject = new Subject<Unit>();
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
                                        .Subscribe(_ => _changeEnemyStatusCoefficientSubject.OnNext(1.05f));
   
    }
    #endregion

    #region public method
    public void OnDefeatedBossEnemyEvent()
    {
        _defeatedBossSubject.OnNext(Unit.Default);

        _currentEnemyWave = (EnemyWaveType)(int)_currentEnemyWave++;
        int currentWave = (int)_currentEnemyWave + 1;
        OnGenerateEnemies((EnemyWaveType)currentWave);
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
                _generator.OnEnemyGenerate(EnemyType.Wave1_Point1);
                Debug.Log("Wave2開始");
                break;
            case EnemyWaveType.Wave_3:
                _generator.OnEnemyGenerate(EnemyType.Wave2_Chase1);
                _generator.OnEnemyGenerate(EnemyType.Wave1_Point1);
                Debug.Log("Wave3開始");
                break;
            default:
                break;
        }
    }
    private void BossGenerate(EnemyType bossType)
    {
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
