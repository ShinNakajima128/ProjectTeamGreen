using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MyScriptable/SkillData")]

public class SkillData : ScriptableObject
{
    #region property
    public SkillType SkillType => _skillType;

    public float AttackAmount => _attackAmount;

    public float Correctionvalue  => _correctionvalue;

    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("スキルの種類")]
    [SerializeField]
    private SkillType _skillType = default;

    [Header("変数")]
    [Tooltip("各スキルの初期の攻撃力")]
    [SerializeField]
    private float _attackAmount = 1.0f;

    [Header("変数")]
    [Tooltip("パワーアップ時の補正値")]
    [SerializeField]
    private float _correctionvalue = 1.0f;

    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region public method

    #endregion

    #region protected method


    #endregion

    #region private method
    #endregion
}

/// <summary>
/// スキルの種類
/// </summary>
public enum SkillType
{
   Aura,
   Fairy,
   knuckle,
   Boul,
   Throw
}