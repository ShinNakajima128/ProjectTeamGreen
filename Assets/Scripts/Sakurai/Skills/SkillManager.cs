using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillManager : MonoBehaviour
{
    #region property
    public static SkillManager Instance { get; private set; }
    #endregion

    #region serialize
    [Tooltip("プレイヤーの各スキル")]
    [SerializeField]
    private SkillBase[] skills = default;
    #endregion

    #region private
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}
