using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ItemTest : MonoBehaviour
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
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log($"HP:{_health.CurrentHP}");
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    _player.Damage(10);
                }
            })
            .AddTo(this);
    }

    private void Update()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}