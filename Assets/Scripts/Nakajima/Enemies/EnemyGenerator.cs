using System;
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
        PlayerController.Instance.Status.CurrentPlayerLevel
                                        .TakeUntilDestroy(this)
                                        .Subscribe(_ => AddGenerateLimitAmount());
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
            int randomXY = UnityEngine.Random.Range(0, 2);
            float randomX, randomY;

            if (randomXY == 0)
            {
                randomX = UnityEngine.Random.Range(_playerTrans.position.x,
                                                   _playerTrans.position.x + _generatePointAbsValue.x + 2.5f);
                randomY = UnityEngine.Random.Range(_playerTrans.position.y + _generatePointAbsValue.y,
                                                   _playerTrans.position.y + _generatePointAbsValue.y + 2.5f);

                randomX = UnityEngine.Random.Range(0, 2) == 0 ? randomX : -randomX;
                randomY = UnityEngine.Random.Range(0, 2) == 0 ? randomY : randomY - (_generatePointAbsValue.y * -2);

            }
            else
            {
                randomX = UnityEngine.Random.Range(_playerTrans.position.x + _generatePointAbsValue.x,
                                                   _playerTrans.position.x + _generatePointAbsValue.x + 2.5f);
                randomY = UnityEngine.Random.Range(_playerTrans.position.y,
                                                   _playerTrans.position.y + _generatePointAbsValue.y + 2.5f);

                randomX = UnityEngine.Random.Range(0, 2) == 0 ? randomX : randomX - (_generatePointAbsValue.x * -2);
                randomY = UnityEngine.Random.Range(0, 2) == 0 ? randomY : -randomY;
            }

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
                    int randomXY = UnityEngine.Random.Range(0, 2);
                    float randomX, randomY;

                    if (randomXY == 0)
                    {
                        randomX = UnityEngine.Random.Range(_playerTrans.position.x - _generatePointAbsValue.x - 1.5f,
                                                           _playerTrans.position.x + _generatePointAbsValue.x + 1.5f);
                        randomY = UnityEngine.Random.Range(_playerTrans.position.y + _generatePointAbsValue.y,
                                                           _playerTrans.position.y + _generatePointAbsValue.y + 2.5f);

                        randomY = UnityEngine.Random.Range(0, 2) == 0 ? randomY : randomY - (Vector2.Distance(_playerTrans.position,
                                                                                                              new Vector2(_playerTrans.position.x, randomY)) * -2);

                    }
                    else
                    {
                        randomX = UnityEngine.Random.Range(_playerTrans.position.x + _generatePointAbsValue.x,
                                                           _playerTrans.position.x + _generatePointAbsValue.x + 1.5f);
                        randomY = UnityEngine.Random.Range(_playerTrans.position.y - _generatePointAbsValue.y - 2f,
                                                           _playerTrans.position.y + _generatePointAbsValue.y + 2f);

                        randomX = UnityEngine.Random.Range(0, 2) == 0 ? randomX : randomX - (Vector2.Distance(_playerTrans.position,
                                                                                                              new Vector2(randomX, _playerTrans.position.y)) * -2);
                    }
                    Vector2 generatePos = new Vector2(randomX, randomY);
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

