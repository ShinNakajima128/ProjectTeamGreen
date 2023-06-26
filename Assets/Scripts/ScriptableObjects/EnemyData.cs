using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵のデータをまとめたScriptableObjectクラス
/// </summary>
[CreateAssetMenu(menuName = "MyScriptable/EnemyData")]
public class EnemyData : ScriptableObject
{
    #region property
    public int HP { get { return _hp; } }
    public int AttackAmount { get { return _attackAmount; } }
    #endregion

    #region serialize

    [Tooltip("敵のHP")]
    [SerializeField]
    private int _hp = 10;

    [Tooltip("攻撃力")]
    [SerializeField]
    private int _attackAmount = 1;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
