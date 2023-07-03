using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBackGround : MonoBehaviour
{ 
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("タイルのサイズ")]
    [SerializeField]
    private float _tileSize = 16f;

    [Tooltip("プレイヤーの位置取得")]
    [SerializeField]
    private GameObject _player;

    [Tooltip("背景用のプレハブ")]
    [SerializeField]
    private GameObject _tilePrefab;
    #endregion

    #region private

    //タイルの配列
    private GameObject[,] _tiles;

    //プレイヤーの前回の座標を保存する変数
    private Vector3 _lastPlayerPos;
    #endregion

    #region Constant
    private const int GRID_SIZE = 3; 
    #endregion

    #region Event
    #endregion

    #region unity methods
    private void Awake()
    {

    }

    void Start()
    {
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

        // プレイヤーが一定の距離だけ移動したかを確認
        if (Mathf.Abs(playerPos.x - _lastPlayerPos.x) > _tileSize)
        {
            if (playerPos.x > _lastPlayerPos.x)
            {
                ShiftTilesRight();
            }
            else
            {
                ShiftTilesLeft();
            }
            _lastPlayerPos.x = playerPos.x;
        }

        if (Mathf.Abs(playerPos.y - _lastPlayerPos.y) > _tileSize)
        {
            if (playerPos.y > _lastPlayerPos.y)
            {
                ShiftTilesUp();
            }
            else
            {
                ShiftTilesDown();
            }
            _lastPlayerPos.y = playerPos.y;
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method

    void ShiftTilesLeft()
    {
        GameObject temp;

        for (int i = 0; i < 3; i++)
        {
            _tiles[i, 2].transform.position += new Vector3(-3f * _tileSize, 0, 0);

            // タイルの位置を配列内で更新
            temp = _tiles[i, 2];
            _tiles[i, 2] = _tiles[i, 1];
            _tiles[i, 1] = _tiles[i, 0];
            _tiles[i, 0] = temp;
        }
    }

    // タイルを右にシフト
    void ShiftTilesRight()
    {
        GameObject temp;

        for (int i = 0; i < 3; i++)
        {
            _tiles[i, 0].transform.position += new Vector3(3.0f * _tileSize, 0, 0);

            //タイルの位置を配列内で更新
            temp = _tiles[i, 0];
            _tiles[i, 0] = _tiles[i, 1];
            _tiles[i, 1] = _tiles[i, 2];
            _tiles[i, 2] = temp;
        }
    }

    // タイルを下にシフト
    private void ShiftTilesDown()
    {
        GameObject temp;

        for (int i = 0; i < 3; i++)
        {
            _tiles[0, i].transform.position += new Vector3(0, -3f * _tileSize, 0);

            //タイルの位置を配列内で更新
            temp = _tiles[0, i];
            _tiles[0, i] = _tiles[1, i];
            _tiles[1, i] = _tiles[2, i];
            _tiles[2, i] = temp;
        }
    }
  
    // タイルを上にシフト
    private void ShiftTilesUp()
    {
        GameObject temp;

        for (int i = 0; i < 3; i++)
        {
            _tiles[2, i].transform.position += new Vector3(0, 3f * _tileSize, 0);

            //タイルの位置を配列内で更新
            temp = _tiles[2, i];
            _tiles[2, i] = _tiles[1, i];
            _tiles[1, i] = _tiles[0, i];
            _tiles[0, i] = temp; 
        }
    }
    #endregion
}