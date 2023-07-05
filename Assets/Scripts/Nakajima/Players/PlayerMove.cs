using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem.EnhancedTouch;
//using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
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
    /// <summary>移動可能かどうか</summary>
    private bool _isCanMove = false;
    /// <summary>現在の移動方向</summary>
    private Vector2 _currentDir;
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
        StageManager.Instance.IsInGameSubject
                             .Subscribe(value => ChangeIsCanMove(value))
                             .AddTo(this);

        this.FixedUpdateAsObservable()
            .Where(_ => _isCanMove) //操作可能な場合
            .Subscribe(_ =>
            {

                if (Input.touchCount > 0)
                {
                    Debug.Log("タッチ操作中");

                    Touch touch = Input.GetTouch(0);
                    float x = touch.deltaPosition.x;
                    float y = touch.deltaPosition.y;

                    var inputDir = new Vector2(x, y).normalized;

                    _currentDir = inputDir * _moveSpeed;
                }
                else
                {
                    _currentDir = Vector2.zero;
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
    #endregion
}
