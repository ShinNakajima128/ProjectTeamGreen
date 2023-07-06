using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [SerializeField]
    private GameObject _restartButton = default;
    #endregion

    #region private
    private Button _pauseButton;
    #endregion

    #region unity methods
    private void Awake()
    {
        _pauseButton = GetComponent<Button>();
    }

    private void Start()
    {
        _pauseButton.onClick.AddListener(() => GamePause());
        _restartButton.GetComponent<Button>().onClick.AddListener(() => PauseEnd());
    }
    #endregion

    #region public method
    #endregion

    #region private method
    /// <summary>
    /// 一時停止
    /// </summary>
    private void GamePause()
    {
        Debug.Log("Pause");
        Time.timeScale = 0;
        _restartButton.GetComponent<CanvasGroup>().alpha = 1;
    }

    private void PauseEnd()
    {
        Debug.Log("Restart");
        Time.timeScale = 1;
        _restartButton.GetComponent<CanvasGroup>().alpha = 0;
    }
    #endregion
}