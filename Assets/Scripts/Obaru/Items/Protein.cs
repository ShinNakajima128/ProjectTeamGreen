using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃力アップアイテム
/// </summary>
public class Protein : ItemBase
{
    #region serialize
    [Tooltip("倍率")]
    [SerializeField]
    private float _coefficient = 1.1f;
    #endregion

    #region public method
    /// <summary>
    /// アイテムの使用
    /// </summary>
    /// <param name="player"></param>
    public override void Use(PlayerController player)
    {
        Debug.Log($"{ItemType}を使用した");
        player.PowerUp(_coefficient);
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