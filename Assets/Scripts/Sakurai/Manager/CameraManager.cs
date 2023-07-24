using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    private CameraType type;
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