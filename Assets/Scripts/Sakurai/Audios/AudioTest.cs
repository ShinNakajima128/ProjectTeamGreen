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

    [SerializeField]
    private SEType _testSe = default;
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.PlaySE(_testSe);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.PlayBGM(_testBgm);
        }
    }
    /*
    //インスペクターの内容が変更された時に呼び出し(エディター上のみでアプリケーションでは実行されない)
    private void OnValidate()
    {
        if (_init)
        {
            AudioManager.PlayBGM(_testBgm);
        }
    }
    */
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}