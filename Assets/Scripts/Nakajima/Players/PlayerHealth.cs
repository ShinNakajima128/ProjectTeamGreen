using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// プレイヤーのHP、ステータスを管理するコンポーネント
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    #region property
    public float CurrentMaxHP => _currentMaxHP;
    public float CurrentHP => _currentHP;
    public IObservable<float> ChangeHPObserver => _changeHPSubject;
    #endregion

    #region serialize
    [Tooltip("ゲーム開始時の最大HP")]
    [SerializeField]
    private float _startMaxHP = 50f;
    #endregion

    #region private
    /// <summary>現在の「最大HP」</summary>
    private float _currentMaxHP;
    /// <summary>現在の「HP」</summary>
    private float _currentHP;
    #endregion

    #region Constant
    #endregion

    #region Event
    private Subject<float> _changeHPSubject = new Subject<float>();
    #endregion

    #region unity methods
    private void Awake()
    {
        Setup();
    }

    private void Start()
    {

    }
    #endregion

    #region public method
    /// <summary>
    /// HPを回復する
    /// </summary>
    /// <param name="amount">回復量</param>
    public void Heal(float amount)
    {
        _currentHP += amount;
        
        //値が上限を超えた場合は補正
        if (_currentHP >= _currentMaxHP)
        {
            _currentHP = _currentMaxHP;
        }
        _changeHPSubject.OnNext(_currentHP / _currentMaxHP);
    }

    /// <summary>
    /// ダメージを受ける
    /// </summary>
    /// <param name="amount">ダメージ量</param>
    /// <returns>倒されたかどうか</returns>
    public bool Damage(float amount)
    {
        _currentHP -= amount;
        _changeHPSubject.OnNext(_currentHP / _currentMaxHP);

        //倒された場合はtrueを返す
        if (_currentHP <= 0)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        _currentMaxHP = _startMaxHP;
        _currentHP = _currentMaxHP;
    }
    #endregion
}
