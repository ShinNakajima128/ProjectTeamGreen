using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCheck : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
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

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision .CompareTag(GameTag.Item))
        {
            ItemBase item = collision.GetComponent<ItemBase>();
            StartCoroutine(item.ItemGet(PlayerController.Instance, transform));
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}