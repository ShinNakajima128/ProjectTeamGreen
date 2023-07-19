﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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

    [Tooltip("生成される座標の絶対値")]
    [SerializeField]
    private Vector2 _generatePointAbsValue = default;

    [Tooltip("一度に生成する数")]
    [SerializeField]
    private uint _onceGenerateAmount = 10;

    [SerializeField]
    private uint _startGenerateLimit = 10;

    [Header("オブジェクト")]
    [Tooltip("各敵")]
    [SerializeField]
    private Enemy[] _enemies = default;
    #endregion

    #region private
    private Transform _playerTrans;
    /// <summary>各敵のプールを管理するDictionary</summary>
    private Dictionary<EnemyType, ObjectPool<EnemyBase>> _enemyPoolDic = new Dictionary<EnemyType, ObjectPool<EnemyBase>>();
    private Dictionary<EnemyType, Coroutine> _generateCoroutineDic = new Dictionary<EnemyType, Coroutine>();
    private bool _isInGame = false;
    private uint _currentOnceGenerateAmount;
    private uint _currentGenerateLimit;
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
        //プレイヤーのレベルが上がった時の処理を登録
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .TakeUntilDestroy(this)
                                        .Subscribe(_ => AddGenerateLimitAmount());

        StageManager.Instance.IsInGameObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(value => _isInGame = value);
        
        //ゲームリセット時の処理を登録
        StageManager.Instance.GameResetObserver
                             .TakeUntilDestroy(this)
                             .Subscribe(_ => OnReset());
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

    public void BossGenerate(EnemyType bossType)
    {
        switch (bossType)
        {
            case EnemyType.Wave1_Boss:
                break;
            default:
                Debug.LogError("ボスが指定されていません");
                break;
        }
        var currentBoss = _enemyPoolDic[bossType].Rent(1);

        if (currentBoss != null)
        {
            currentBoss.gameObject.SetActive(true);
            float randomX, randomY;
            int randomRad = UnityEngine.Random.Range(0, 360);

            randomX = _generatePointAbsValue.x * Mathf.Sin(Time.time * randomRad);
            randomY = _generatePointAbsValue.y * Mathf.Cos(Time.time * randomRad);

            Vector2 generatePos = new Vector2(randomX, randomY);

            currentBoss.gameObject.transform.localPosition = generatePos;
        }
    }
    #endregion

    #region private method
    private void Setup()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemyPoolDic.Add(_enemies[i].EnemyPrefab.EnemyType,
                new ObjectPool<EnemyBase>(_enemies[i].EnemyPrefab, _enemies[i].Parent));
        }

        _playerTrans = GameObject.FindGameObjectWithTag(GameTag.Player).transform;
        _currentOnceGenerateAmount = _onceGenerateAmount;
        _currentGenerateLimit = _startGenerateLimit;
    }

    private void AddGenerateLimitAmount()
    {
        _currentGenerateLimit += 5;
        _currentOnceGenerateAmount++;
    }
    private void OnReset()
    {
        //現在ステージに存在する敵を全てプールに戻す
        _enemyPoolDic.Select(x => x.Value)
                     .ToList()
                     .ForEach(x => x.Return());

        _currentOnceGenerateAmount = _onceGenerateAmount;
        _currentGenerateLimit = _startGenerateLimit;
        _isInGame = true;
        Debug.Log("リセット完了");
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
            for (int i = 0; i < _currentOnceGenerateAmount; i++)
            {
                var enemy = _enemyPoolDic[type].Rent(_currentGenerateLimit);

                //プールに敵オブジェクトがある場合は指定範囲の中でランダムな座標に生成
                if (enemy != null)
                {
                    enemy.gameObject.SetActive(true);
                    float randomX, randomY;

                    int randomRad = UnityEngine.Random.Range(0, 360);

                    randomX = _generatePointAbsValue.x * Mathf.Sin(Time.time * randomRad);
                    randomY = _generatePointAbsValue.y * Mathf.Cos(Time.time * randomRad);

                    Vector2 generatePos = new Vector2(_playerTrans.position.x + randomX, _playerTrans.position.y + randomY);
                    enemy.transform.localPosition = generatePos;
                }
            }

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
    public string EnemyName;
    public EnemyBase EnemyPrefab;
    public uint ReserveAmount;
    public uint ActivationLimit;
    public Transform Parent;
}

