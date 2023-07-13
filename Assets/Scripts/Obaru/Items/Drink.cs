using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回復アイテム
/// </summary>
public class Drink : ItemBase
{
    #region serialize
    [Tooltip("回復割合")]
    [SerializeField]
    private float _healRate = 0.3f;
    #endregion

    #region public method
    /// <summary>
    /// アイテムを使用
    /// </summary>
    /// <param name="player"></param>
    public override void Use(PlayerController player)
    {
        Debug.Log($"{ItemType}を使用した");

        //回復量を計算
        float healAmount = player.CurrentMaxHP * _healRate;

        player.Heal(healAmount);
        AudioManager.PlaySE(SEType.Heal);
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