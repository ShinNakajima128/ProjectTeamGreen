using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// プレイヤーの移動処理を行うコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    #region property
    public ReactiveProperty<bool> IsFlipedProerty => _isFlipedProperty;
    #endregion

    #region serialize
    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 5.0f;
    #endregion

    #region private
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _isCanMove = false;
    private Vector2 _currentDir;
    private Vector2 _inputMove;
    #endregion

    #region Constant
    #endregion

    #region Event
    private ReactiveProperty<bool> _isFlipedProperty = new ReactiveProperty<bool>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

        _isFlipedProperty.Value = false;
    }

    private void Start()
    {
        StageManager.Instance.IsInGameSubject
                             .Subscribe(value => ChangeIsCanMove(value))
                             .AddTo(this);

        this.FixedUpdateAsObservable()
            .Where(_ => _isCanMove)
            .Subscribe(_ =>
            {
                if (_currentDir == Vector2.zero)
                {
                    _rb.velocity = Vector2.zero;
                }
                else
                {
                    _inputMove = Vector2.up * _currentDir.y + Vector2.right * _currentDir.x;

                    _rb.velocity = _inputMove.normalized * _moveSpeed;
                }

                if (!_isFlipedProperty.Value && _inputMove.x > 0)
                {
                    _isFlipedProperty.Value = true;
                    Debug.Log("右向き");
                }
                else if (_isFlipedProperty.Value && _inputMove.x < 0)
                {
                    _isFlipedProperty.Value = false;
                    Debug.Log("左向き");
                }
            })
            .AddTo(this);
    }

    #endregion

    #region public method
    public void SetDirection(Vector2 dir)
    {
        _currentDir = dir;
    }
    #endregion

    #region private method
    /// <summary>
    /// 操作可/不可を切り替える
    /// </summary>
    /// <param name="value">ONかOFFの状態の値</param>
    private void ChangeIsCanMove(bool value)
    {
        _isCanMove = value;
    }
    #endregion
}
