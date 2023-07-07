using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class SkillInfo : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Tooltip("スキルレベル表示用のテキスト")]
    [SerializeField]
    private TextMeshProUGUI _currentLevelText = default;

    [Tooltip("スキルタイプ")]
    [SerializeField]
    private SkillType _skillType = default;
    #endregion

    #region private
    private int _currentLevel = 1;
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    private void Start()
    {
        
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