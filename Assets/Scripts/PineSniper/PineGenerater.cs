using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineGenerater : MonoBehaviour
{
    /// <summary>草のオブジェクト。Playerがこれを撃つことでPineが出てくる。</summary>
    [SerializeField] GameObject _leafObject = null;
    /// <summary>パインのオブジェクト。周辺にあるImmaturePineを表示してくれる。</summary>
    [SerializeField] GameObject _pineObject = null;
    /// <summary>未熟のパインのオブジェクト。Playerがこれを撃つとGameOver</summary>
    [SerializeField] GameObject _immaturePineObject = null;
    /// <summary>PineObjectとLeafObjectを生成する際に基準となる位置</summary>
    [SerializeField] Transform _generatePos;
    /// <summary>横に生成する個数</summary>
    [SerializeField] int _width = 0;
    /// <summary>横に生成する幅の微調整</summary>
    [SerializeField] float _tweakOfWidth = 0;
    /// <summary>縦に生成する個数</summary>
    [SerializeField] int _height = 0;
    /// <summary>縦に生成する幅の微調整</summary>
    [SerializeField] float _tweakOfHeight = 0;
    /// <summary>Pineの2次元配列</summary>
    Pine[,] _pines;

    // Start is called before the first frame update
    void Start()
    {
        PineGenerate();
    }

    /// <summary>
    /// Pineの生成
    /// </summary>
    void PineGenerate()
    {
        _pines = new Pine[_height, _width];

        GameObject[,] go = new GameObject[_height, _width];

        for (int h = 0; h < _height; h++)
        {
            for (int w = 0; w < _width; w++)
            {
                Vector3 pos = _generatePos.position + new Vector3(w * _tweakOfWidth, 0, -h * _tweakOfHeight);

               // Pineの配列にインスタンス化したものを入れるために一度変換している。
                go[h, w] = Instantiate(_pineObject, pos, _generatePos.rotation, _generatePos);

                _pines[h, w] = go[h, w].GetComponent<Pine>();
                _pines[h, w].PineState = PineState.None;
            }
        }
    }
}
