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
    /// <summary>行に生成する個数</summary>
    [SerializeField] int _line = 0;
    /// <summary>行に生成する幅の微調整</summary>
    [SerializeField] float _tweakOfLine = 0;
    /// <summary>列に生成する個数</summary>
    [SerializeField] int _column = 0;
    /// <summary>列に生成する幅の微調整</summary>
    [SerializeField] float _tweakOfColumn = 0;
    /// <summary>Pineの高さ微調整</summary>
    [SerializeField] float _pineTweakHeight = 0;
    /// <summary>Pineの高さ微調整</summary>
    [SerializeField] float _leafTweakHeight = 0;
    /// <summary>Pineの2次元配列</summary>
    Pine[,] _pines;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    /// <summary>
    /// Pineの生成
    /// </summary>
    void Generate()
    {
        _pines = new Pine[_column, _line];

        GameObject[,] go = new GameObject[_column, _line];

        for (int h = 0; h < _column; h++)
        {
            for (int w = 0; w < _line; w++)
            {
                Vector3 pinePos = _generatePos.position + new Vector3(w * _tweakOfLine, _pineTweakHeight, -h * _tweakOfColumn);
                Vector3 leafPos = _generatePos.position + new Vector3(w * _tweakOfLine, _leafTweakHeight, -h * _tweakOfColumn);

               // Pineの配列にインスタンス化したものを入れるために一度変換している
                go[h, w] = Instantiate(_pineObject, pinePos, _generatePos.rotation, _generatePos);
                // Leafを生成
                Instantiate(_leafObject, leafPos, _generatePos.rotation, _generatePos);

                _pines[h, w] = go[h, w].GetComponent<Pine>();
                _pines[h, w].PineState = PineState.None;
                //ここで未熟なパインを決めるメソッドを呼ぶ
            }
        }
    }
}
