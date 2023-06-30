using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の生成機能を持つコンポーネント
/// </summary>
public class EnemyGenerator : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("生成する間隔")]
    [SerializeField]
    private float _generateInterval = 1.0f;

    //[SerializeField]
    //private Vector2 _generatePoint = 

    [Tooltip("一度に生成する数")]
    [SerializeField]
    private uint _onceGenerateAmount = 10;

    [Header("オブジェクト")]
    [Tooltip("各敵")]
    [SerializeField]
    private Enemy[] _enemies = default;

    [Tooltip("生成する場所をまとめた親オブジェクト")]
    [SerializeField]
    private Transform _generatePointsParent = default;
    #endregion

    #region private
    private Transform _playerTrans;
    /// <summary>各敵のプールを管理するDictionary</summary>
    private Dictionary<EnemyType, ObjectPool> _enemyPoolDic = new Dictionary<EnemyType, ObjectPool>();
    private Dictionary<EnemyType, Coroutine> _generateCoroutineDic = new Dictionary<EnemyType, Coroutine>();
    private bool _isInGame = false;
    private uint _currentOnceGenerateAmount;
    #endregion

    #region Constant
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {
        Setup();

        //仮の処理
        _isInGame = true;
    }

    private void Start()
    {
        
    }
    #endregion

    #region public method
    /// <summary>
    /// 敵の生成を開始する
    /// </summary>
    /// <param name="type">敵の種類</param>
    public void OnEnemyGenerate(EnemyType type)
    {
        Coroutine c = StartCoroutine(GenerateCoroutine(type));

        if (!_generateCoroutineDic.ContainsKey(type))
        {
            _generateCoroutineDic.Add(type, c);
        }
        else
        {
            _generateCoroutineDic[type] = c;
        }
    }

    /// <summary>
    /// 敵の生成を停止する
    /// </summary>
    /// <param name="type">敵の種類</param>
    public void StopEnemyGenerate(EnemyType type)
    {
        if (_generateCoroutineDic[type] != null)
        {
            StopCoroutine(_generateCoroutineDic[type]);
            _generateCoroutineDic[type] = null;
        }
    }
    #endregion

    #region private method
    private void Setup()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemyPoolDic.Add((EnemyType)i, new ObjectPool(_enemies[i].EnemyPrefab.gameObject,
                                                           _enemies[i].ReserveAmount,
                                                           _enemies[i].ActivationLimit,
                                                           _enemies[i].Parent));
        }

        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;
        _currentOnceGenerateAmount = _onceGenerateAmount;
    }
    #endregion

    #region coroutine method
    /// <summary>
    /// 敵を生成するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateCoroutine(EnemyType type)
    {
        var interval = new WaitForSeconds(_generateInterval);

        while (_isInGame)
        {
            var enemy = _enemyPoolDic[type].Rent();

            yield return interval;
        }
    }
    #endregion
}

/// <summary>
/// 敵
/// </summary>
[Serializable]
class Enemy 
{
    public EnemyBase EnemyPrefab;
    public uint ReserveAmount;
    public uint ActivationLimit;
    public Transform Parent;
}

