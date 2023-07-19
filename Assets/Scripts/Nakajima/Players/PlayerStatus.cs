using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーのステータス全般の機能を持つコンポーネント
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    #region property
    public float SkillAttackCoefficient => _skillAttackCoefficient;
    public uint CurrentPlayerLevelAmount => _currentPlayerLevel.Value;
    public IObservable<uint> CurrentPlayerLevel => _currentPlayerLevel;
    public IObservable<uint> CurrentExp => _currentExp;
    public IObservable<uint> CurrentRequireExp => _currentRequireExp;
    public IObservable<float> GetEXPObserver => _getEXPSubject;
    #endregion

    #region serialize
    [Tooltip("スタート時に必要な経験値")]
    [SerializeField]
    private uint _startRequireExp = 100;

    [Tooltip("各スキルの攻撃力に掛け合わせる倍率（係数）")]
    [SerializeField]
    private float _skillAttackCoefficient = 1.0f;
    #endregion

    #region private
    /// <summary>現在のプレイヤーのレベル。初期値は「1」</summary>
    private readonly ReactiveProperty<uint> _currentPlayerLevel = new ReactiveProperty<uint>(1);
    /// <summary>現在の経験値</summary>
    private readonly ReactiveProperty<uint> _currentExp = new ReactiveProperty<uint>();
    /// <summary>現在のレベルアップに必要な経験値</summary>
    private readonly ReactiveProperty<uint> _currentRequireExp = new ReactiveProperty<uint>();
    #endregion

    #region Constant
    /// <summary>次のレベルアップ時に必要な経験値に掛け合わせる倍率</summary>
    private const float EXP_LEVERAGE = 1.5f;
    #endregion

    #region Event
    /// <summary>経験値を取得した時のSubject</summary>
    private Subject<float> _getEXPSubject = new Subject<float>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Setup();
    }
    #endregion

    #region public method
    /// <summary>
    /// 係数を変更する
    /// </summary>
    /// <param name="value">新しい係数の値</param>
    public void ChangeCoefficient(float newValue)
    {
        _skillAttackCoefficient = newValue;
    }

    /// <summary>
    /// 経験値を足す
    /// </summary>
    /// <param name="exp">獲得した経験値</param>
    public void AddExp(uint value)
    {
        _currentExp.Value += value;
        
        //経験値が現在の必要値を超えた場合
        if (_currentExp.Value >= _currentRequireExp.Value)
        {
            //経験値が超過した分を保持
            uint overFlowExp = _currentExp.Value - _currentRequireExp.Value;

            _currentPlayerLevel.Value++;
            _currentExp.Value = overFlowExp;
            _currentRequireExp.Value = (uint)((_currentRequireExp.Value + (_currentRequireExp.Value / 2)) * EXP_LEVERAGE);
        }
        _getEXPSubject.OnNext((float)_currentExp.Value / _currentRequireExp.Value);
    }
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        _currentRequireExp.Value = _startRequireExp;
    }
    #endregion
}
