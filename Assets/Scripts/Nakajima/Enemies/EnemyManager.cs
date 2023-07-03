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
    public IObservable<uint> DefeatedEnemyAmountViewObserver => _defeatedEnemyAmountViewSubject;
    #endregion

    #region serialize
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
        _generator.OnEnemyGenerate(EnemyType.Wave1_Chase1);
        
        //討伐数が変化した時のイベント処理を登録
        _defeatAmountProperty
        .Subscribe(value =>
        {
            _defeatedEnemyAmountViewSubject.OnNext(value);
        })
        .AddTo(this);
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
