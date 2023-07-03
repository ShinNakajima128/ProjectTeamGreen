using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceBackGround : MonoBehaviour
{
    #region property
    #endregion

    #region serialize
    [Header("変数")]
    [Tooltip("プレイヤープレハブ")]
    [SerializeField]
    private GameObject _player = default;

    [Tooltip("背景プレハブ")]
    [SerializeField]
    private GameObject _tilePrefab;

    [Tooltip("背景のサイズ")]
    [SerializeField]
    private float _tileSize = 16.0f;

    [Tooltip("背景のサイズ")]
    [SerializeField]
    private int _arraySize = 3;

    #endregion

    #region private

    //プレイヤーの現在位置
    private Vector2 _playerPos;

    //背景タイルを格納する配列
    private GameObject[,] _tiles;

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
        _playerPos = _player.transform.position;
        _tiles = new GameObject[_arraySize, _arraySize];

        //タイルの初期化
        for (int i = 0; i < _arraySize; i++)
        {
            for (int j = 0; j < _arraySize; j++)
            {
                Vector2 pos = new Vector2((i - 1) * _tileSize, (j - 1) * _tileSize);
                _tiles[i, j] = Instantiate(_tilePrefab, pos, Quaternion.identity, transform);
            }
        }
    }

    private void Update()
    {
        //プレイヤーが新しいタイルに移動したかどうかをチェック
        Vector2 displacement = (Vector2)_player.transform.position - _playerPos;
        int dx = Mathf.RoundToInt(displacement.x / _tileSize);
        int dy = Mathf.RoundToInt(displacement.y / _tileSize);

        if (dx != 0 || dy != 0)
        {
            _playerPos += new Vector2(dx * _tileSize, dy * _tileSize);

            //背景タイルを再配置
            for (int i = 0; i < _arraySize; i++)
            {
                for (int j = 0; j < _arraySize; j++)
                {
                    Vector2 pos = new Vector2((i - dx)* _tileSize,(j-dy)*_tileSize);
                    _tiles[(i - dx + _arraySize) % _arraySize, (j - dy + _arraySize)].transform.position = pos;
                }
            }

            //背景タイルの配列をシフト
            if (dx != 0)
            {
                GameObject[,] newTiles = new GameObject[_arraySize, _arraySize];
                for (int i = 0; i < _arraySize; i++)
                {
                    List<GameObject> temp = new List<GameObject>();
                    for (int j = 0; j < _arraySize; j++)
                    {
                        temp.Add(_tiles[i - dx + _arraySize % _arraySize, j]);
                    }

                    for (int j = 0; j < _arraySize; j++)
                    {
                        newTiles[i, j] = temp[j];
                    }
                }
                _tiles = newTiles;
            }

            if (dy != 0)
            {
                GameObject[,] newTiles = new GameObject[_arraySize, _arraySize];
                for (int i = 0; i < _arraySize; i++)
                {
                    List<GameObject> temp = new List<GameObject>();
                    for (int j = 0; j < _arraySize; j++)
                    {
                        temp.Add(_tiles[j, (i - dy + _arraySize) % _arraySize]);
                    }

                    for (int j = 0; j < _arraySize; j++)
                    {
                        newTiles[j, i] = temp[j];
                    }
                }
                _tiles = newTiles;
            }
        }
    }
    #endregion

    #region public method
    #endregion

    #region private method
    #endregion
}