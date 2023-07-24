using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UniRx;

/// <summary>
/// カメラを扱うマネージャー
/// </summary>
public class CameraManager : MonoBehaviour
{
    /// <summary>シネマシーンカメラ</summary>
    #region serialize
    [SerializeField]
    private ActivationCamera[] _virtualCamera;

    /// <summary>カメラの種類</summary>
    [SerializeField]
    private CameraType _type;
    #endregion

    /// <summary>シネマシーンカメラをenumで扱うためのDictionary</summary>
    #region private
    private Dictionary<CameraType, CinemachineVirtualCamera> _camerasDic = new Dictionary<CameraType, CinemachineVirtualCamera>();
    #endregion

    /// <summary>プライオリティを上げたときの値</summary>
    #region Constant
    private const int PriorityAmount = 11;
    #endregion

    #region unity methods
    private void Awake()
    {
        //カメラタイプにシネマカメラを追加
        for (int i = 0; i < _virtualCamera.Length; i++)
        {
            _camerasDic.Add((CameraType)i, _virtualCamera[i].Camera);
        }
    }

    private void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag(GameTag.Player).transform;

        _camerasDic.Select(x => x.Value)
                   .ToList()
                   .ForEach(x =>
                   {
                       x.m_Follow = player;
                       x.m_LookAt = player;
                   });

        TimeManager.Instance.BossEventObserver
                            .TakeUntilDestroy(this)
                            .Subscribe(_ => OnChangeCamera());

        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnResetCamera());
    }
    #endregion

    #region public method
    //カメラの変更処理
    public void CameraChange(CameraType cameraType)
    {
        Debug.Log("実行");

        int _initialPriority = 10;

        foreach (var camera in _camerasDic)
        {
            camera.Value.Priority = _initialPriority;
        }
        _camerasDic[cameraType].Priority = PriorityAmount;
        
    }
    #endregion

    #region private method
    /// <summary>
    /// カメラを変更する
    /// </summary>
    private void OnChangeCamera()
    {
        switch (_type)
        {
            case CameraType.Wave1Camera:
                _type = CameraType.Wave2Camera;
                break;
            case CameraType.Wave2Camera:
                _type = CameraType.Wave3Camera;
                break;
            case CameraType.Wave3Camera:
                break;
            default:
                break;
        }
        CameraChange(_type);
    }

    /// <summary>
    /// カメラを初期状態に戻す
    /// </summary>
    private void OnResetCamera()
    {
        _type = CameraType.Wave1Camera;
        CameraChange(_type);
    }
    #endregion
}

[System.Serializable]
public class ActivationCamera
{
    public string CameraName;
    public CinemachineVirtualCamera Camera;
}

public enum CameraType
{
    Wave1Camera,
    Wave2Camera,
    Wave3Camera
}