using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムを確認するコライダー
/// </summary>
public class ItemCheck : MonoBehaviour
{
    #region unity methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision .CompareTag(GameTag.Item))
        {
            ItemBase item = collision.GetComponent<ItemBase>();
            StartCoroutine(item.ItemGet(PlayerController.Instance, transform));
        }
    }
    #endregion
}