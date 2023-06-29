using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステータス全般の機能を持つコンポーネント
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    #region property
    public float SkillAttackCoefficient => _skillAttackCoefficient;
    #endregion

    #region serialize
    [Tooltip("スタート時に必要な経験値")]
    [SerializeField]
    private uint _startRequireExp = 100;
    #endregion

    #region private
    /// <summary>現在のプレイヤーのレベル</summary>
    private uint _currentPlayerLevel = 1;
    /// <summary>現在の経験値</summary>
    private uint _currentExp = 0;
    /// <summary>現在のレベルアップに必要な経験値</summary>
    private uint _currentRequireExp = 0;
    /// <summary>各スキルの攻撃力に掛け合わせる倍率（係数）</summary>
    private float _skillAttackCoefficient = 1.0f;
    #endregion

    #region Constant
    /// <summary>次のレベルアップ時に必要な経験値に掛け合わせる倍率</summary>
    private const float EXP_LEVERAGE = 1.2f;
    #endregion

    #region Event
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
        _currentExp += value;

        if (_currentExp >= _currentRequireExp)
        {
            _currentPlayerLevel++;
            _currentExp = 0;
            _currentRequireExp = (uint)(_currentRequireExp * EXP_LEVERAGE);
        }
    }
    #endregion

    #region private method
    /// <summary>
    /// 初期化
    /// </summary>
    private void Setup()
    {
        _currentRequireExp = _startRequireExp;
    }
    #endregion
}
