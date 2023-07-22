using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景スクロールを管理するマネージャー
/// </summary>
public class BackGroundPlacer : MonoBehaviour
{ 
    #region serialize
    [Header("変数")]
    [Tooltip("タイルのサイズ")]
    [SerializeField]
    private float _tileSize = 16f;

    [Tooltip("背景用のプレハブ")]
    [SerializeField]
    private GameObject _tilePrefab;
    #endregion

    #region private
    //プレイヤーの位置取得用
    private GameObject _player;

    //タイルの配列
    private GameObject[,] _tiles;

    //プレイヤーの前回の座標を保存する変数
    private Vector3 _lastPlayerPos;
    #endregion

    #region Constant
    /// <summary>縦3枚,横3枚のサイズ</summary>
    private const int GRID_SIZE = 3;
    #endregion

    #region unity methods
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(GameTag.Player); 
    }
    void Start()
    {
        //配置する背景の個数を設定。
        _tiles = new GameObject[GRID_SIZE, GRID_SIZE];
        _lastPlayerPos = _player.transform.position;

        // タイルの初期配置
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                // タイルを生成
                GameObject tile = Instantiate(_tilePrefab);

                // タイルを配列に保存
                _tiles[i, j] = tile;

                // タイルの位置を設定
                tile.transform.position = new Vector3((j - 1) * _tileSize, (1 - i) * _tileSize, 0);
            }
        }
    }

    void Update()
    {
        // プレイヤーの座標を取得
        Vector3 playerPos = _player.transform.position;

        // プレイヤーが一定の距離だけ移動したかを確認。x方向に指定の距離動いた場合。
        if (Mathf.Abs(playerPos.x - _lastPlayerPos.x) > _tileSize)
        {
            //動いたポジションがx方向にプラスだった場合
            if (playerPos.x > _lastPlayerPos.x)
            {
                ShiftTilesRight();
            }
            //動いたポジションがx方向にマイナスだった場合
            else
            {
                ShiftTilesLeft();
            }
            _lastPlayerPos.x = playerPos.x;
        }
        // プレイヤーが一定の距離だけ移動したかを確認。y方向に指定の距離動いた場合。
        if (Mathf.Abs(playerPos.y - _lastPlayerPos.y) > _tileSize)
        {
            //動いたポジションがy方向にプラスだった場合
            if (playerPos.y > _lastPlayerPos.y)
            {
                ShiftTilesUp();
            }
            //動いたポジションがy方向にマイナスだった場合
            else
            {
                ShiftTilesDown();
            }
            _lastPlayerPos.y = playerPos.y;
        }
    }
    #endregion

    #region private method
    //タイルを左にシフト
    private void ShiftTilesLeft()
    {
        for (int i = 0; i < 3; i++)
        {
            //一番右のタイルを一番左側に移動。
            _tiles[i, 2].transform.position += new Vector3(-3f * _tileSize, 0, 0);

            (_tiles[i, 2], _tiles[i, 1]) = (_tiles[i, 1], _tiles[i, 2]);
            (_tiles[i, 1], _tiles[i, 0]) = (_tiles[i, 0], _tiles[i, 1]);
        }
    }

    // タイルを右にシフト
    private void ShiftTilesRight()
    {
        for (int i = 0; i < 3; i++)
        {
            _tiles[i, 0].transform.position += new Vector3(3.0f * _tileSize, 0, 0);

            //配列の中身を入れ替え
            (_tiles[i, 0], _tiles[i, 1]) = (_tiles[i, 1], _tiles[i, 0]);
            (_tiles[i, 1], _tiles[i, 2]) = (_tiles[i, 2], _tiles[i, 1]);
        }
    }

    // タイルを下にシフト
    private void ShiftTilesDown()
    {
        for (int i = 0; i < 3; i++)
        {
            _tiles[0, i].transform.position += new Vector3(0, -3f * _tileSize, 0);

            //配列の中身を入れ替え
            (_tiles[0, i], _tiles[1, i]) = (_tiles[1, i], _tiles[0, i]);
            (_tiles[1, i], _tiles[2, i]) = (_tiles[2, i], _tiles[1, i]);
        }
    }
  
    // タイルを上にシフト
    private void ShiftTilesUp()
    {
        for (int i = 0; i < 3; i++)
        {
            _tiles[2, i].transform.position += new Vector3(0, 3f * _tileSize, 0);

            //配列の中身を入れ替え
            (_tiles[2, i], _tiles[1, i]) = (_tiles[1, i], _tiles[2, i]);
            (_tiles[1, i], _tiles[0, i]) = (_tiles[0, i], _tiles[1, i]);
        }
    }
    #endregion
}