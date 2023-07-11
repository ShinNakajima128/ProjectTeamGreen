using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private BGMType _testBgm = default;
    #endregion

    #region private
    private bool _init = false;
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
        _init = true;
    }

    //インスペクターの内容が変更された時に呼び出し(エディター上のみでアプリケーションでは実行されない)
    private void OnValidate()
    {
        if (_init)
        {
            AudioManager.PlayBGM(_testBgm);
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}