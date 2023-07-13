using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経験値アイテム
/// </summary>
public class EXP : ItemBase
{
    #region serialize
    [Tooltip("経験値")]
    [SerializeField]
    private uint _expValue = 1;
    #endregion

    #region public method
    /// <summary>
    /// アイテムの使用
    /// </summary>
    /// <param name="player"></param>
    public override void Use(PlayerController player)
    {
        Debug.Log($"{ItemType}を使用した");
        player.GetExp(_expValue);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 非アクティブ化
    /// </summary>
    public override void Return()
    {
        Debug.Log($"{ItemType}を非アクティブにした");

        //座標をリセットして非表示にする
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.SetActive(false);
    }
    #endregion
}