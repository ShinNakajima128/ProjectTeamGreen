using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private ActivationCamera[] _virtualCamera;

    [SerializeField]
    private CameraType type;
    #endregion

    #region private
    private Dictionary<CameraType, CinemachineVirtualCamera> _camerasDic = new Dictionary<CameraType, CinemachineVirtualCamera>();
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        for (int i = 0; i < _virtualCamera.Length; i++)
        {
            _camerasDic.Add((CameraType)i, _virtualCamera[i].Camera);
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
    #endregion

    #region public method
    public void CameraChange(CameraType cameraType,int priorityAmount)
    {
        Debug.Log("実行");

        int _initialPriority = 10;

        foreach (var camera in _camerasDic)
        {
            camera.Value.Priority = _initialPriority;
        }

        _camerasDic[cameraType].Priority = priorityAmount;
        
    }
    #endregion

    #region private method
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