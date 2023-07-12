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
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("中ボスのバレットをプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerater _middleBossPoolGenerator;

    [Tooltip("タレットのバレットをプールするオブジェクト")]
    [SerializeField]
    private EnemyBulletGenerater _turretPoolGenerator;
    #endregion

    #region private
    /// <summary>敵の生成機能を持つコンポーネント</summary>
    private EnemyGenerator _generator;
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>討伐数</summary>
    private ReactiveProperty<uint> _defeatAmountProperty = new ReactiveProperty<uint>();
    /// <summary>討伐数を表示する処理のSubject</summary>
    private Subject<uint> _defeatedEnemyAmountViewSubject = new Subject<uint>();
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
                        _generator.OnEnemyGenerate(EnemyType.Wave1_Chase1);
                        _generator.OnEnemyGenerate(EnemyType.Wave1_Point1);
                    });
        
        
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
   
    }
    #endregion

    #region public method
    #endregion

    #region private method
    private void BossGenerate(EnemyType bossType)
    {
        _generator.BossGenerate(bossType);
    }
    #endregion
}
