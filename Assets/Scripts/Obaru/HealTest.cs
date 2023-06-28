using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTest : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    PlayerController _player;

    [SerializeField]
    PlayerHealth _health;
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
        Debug.Log($"現在のHP{_health.CurrentHP}");
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _player.Damage(10);
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}