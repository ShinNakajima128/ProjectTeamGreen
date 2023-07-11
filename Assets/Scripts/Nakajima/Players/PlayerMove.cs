using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

/// <summary>
/// プレイヤーの移動処理を行うコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    #region property
    public ReactiveProperty<bool> IsFlipedProerty => _isFlipedProperty;
    public bool DebugMode { get; set; } = false;
    #endregion

    #region serialize
    [Tooltip("移動速度")]
    [SerializeField]
    private float _moveSpeed = 5.0f;
    #endregion

    #region private
    private Rigidbody2D _rb;
    /// <summary>移動可能かどうか</summary>
    private bool _isCanMove = false;
    /// <summary>現在の移動方向</summary>
    private Vector2 _currentDir;
    private Vector2 _currentInputDir;
    private Vector2 _currentFirstTouchPoint = Vector2.zero;
    private bool _isStartTouched = false;
    #endregion

    #region Constant
    #endregion

    #region Event
    /// <summary>プレイヤーの画像が反転しているかのフラグ。切り替わり時に通知が行われる</summary>
    private ReactiveProperty<bool> _isFlipedProperty = new ReactiveProperty<bool>();
    #endregion

    #region unity methods
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _isFlipedProperty.Value = false;
    }

    private void Start()
    {
        StageManager.Instance.IsInGameObserver
                             .Subscribe(value => ChangeIsCanMove(value))
                             .AddTo(this);

        this.UpdateAsObservable()
            .Where(_ => _isCanMove) //操作可能な場合
            .Subscribe(_ =>
            {
                if (!DebugMode)
                {
                    if (Input.touchCount > 0)
                    {
                        Touch currentTouch = Input.GetTouch(0);

                        if (!_isStartTouched)
                        {
                            _currentFirstTouchPoint = currentTouch.position;
                            _isStartTouched = true;
                            Debug.Log($"{currentTouch.position}");
                        }
                        else
                        {
                            Vector2 currentPos = currentTouch.position;
                            _currentInputDir = currentPos - _currentFirstTouchPoint;

                            _currentDir = _currentInputDir.normalized * _moveSpeed;
                        }
                    }
                    else
                    {
                        _isStartTouched = false;
                        _currentDir = Vector2.zero;
                    }
                }
                //入力がない場合
                if (_currentDir == Vector2.zero)
                {
                    _rb.velocity = Vector2.zero;
                }
                else
                {
                    _rb.velocity = _currentDir.normalized * _moveSpeed;
                }

                if (!_isFlipedProperty.Value && _currentDir.x > 0)
                {
                    _isFlipedProperty.Value = true;
                    Debug.Log("右向き");
                }
                else if (_isFlipedProperty.Value && _currentDir.x < 0)
                {
                    _isFlipedProperty.Value = false;
                    Debug.Log("左向き");
                }
            })
            .AddTo(this);
    }
#endregion

#region public method
    /// <summary>
    /// 入力された移動方向をセットする
    /// </summary>
    /// <param name="dir">方向</param>
    public void SetDirection(Vector2 dir)
    {
#if UNITY_EDITOR
        _currentDir = dir;
#endif
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

   

    //private void OnFingerDown(Finger finger)
    //{
    //    _originTouchPoint = finger.screenPosition;
    //    Debug.Log(_originTouchPoint);
    //}

    //private void OnFingerMove(Finger finger)
    //{
    //    Debug.Log(finger.screenPosition);
    //}

    //private void OnFingerUp(Finger finger)
    //{
    //    _originTouchPoint = Vector2.zero;
    //}
#endregion
}
