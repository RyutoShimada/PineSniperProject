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
    [SerializeField] Transform _generatePos = null;
    /// <summary>未熟なパインを生成する個数</summary>
    [SerializeField] int immaturePineCount = 0;
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
    public　Pine[,] _pines { get; private set; }
    public　Leaf[,] _leafs { get; private set; }

    GameObject[,] _pineObj;

    // Start is called before the first frame update
    void Start()
    {
        _pines = new Pine[_column, _line];
        _leafs = new Leaf[_column, _line];
        _pineObj = new GameObject[_column, _line];
        GeneratePine(_pines, _pineObj);
        GenerateImmaturePine(_pines, _pineObj, immaturePineCount);
    }

    /// <summary>
    /// Pineの生成
    /// </summary>
    void GeneratePine(Pine[,] pines, GameObject[,] pineObj)
    {
        // ここでPineを生成する
        for (int column = 0; column < pines.GetLength(0); column++)
        {
            for (int line = 0; line < pines.GetLength(1); line++)
            {
                Vector3 pinePos = _generatePos.position + new Vector3(line * _tweakOfLine, _pineTweakHeight, -column * _tweakOfColumn);
                Vector3 leafPos = _generatePos.position + new Vector3(line * _tweakOfLine, _leafTweakHeight, -column * _tweakOfColumn);

               // Pineの配列にインスタンス化したものを入れるために一度変換している
                pineObj[column, line] = Instantiate(_pineObject, pinePos, _generatePos.rotation, _generatePos);

                // Leafを生成
                _leafs[column, line] = Instantiate(_leafObject, leafPos, _generatePos.rotation, _generatePos).gameObject.GetComponent<Leaf>();

                // インデックス情報を更新
                _leafs[column, line].GetComponent<Leaf>().IndexColumn = column;
                _leafs[column, line].GetComponent<Leaf>().IndexLine = line;

                // ステート変更
                pines[column, line] = pineObj[column, line].GetComponent<Pine>();
                pines[column, line].PineState = PineState.None;

                // インデックス情報を変更
                pines[column, line].IndexColumn = column;
                pines[column, line].IndexLine = line;
            }
        }
    }

    void GenerateImmaturePine(Pine[,] pines, GameObject[,] pineObj, int count)
    {
        if (count > pines.GetLength(0) * pines.GetLength(1)) count = pines.GetLength(0) * pines.GetLength(1); //無限ループを防ぐ

        for (int i = 0; i < count;)
        {
            int randomHeight = Random.Range(0, pines.GetLength(0));
            int randomWidth = Random.Range(0, pines.GetLength(1));

            Pine pine = pineObj[randomHeight, randomWidth].gameObject.GetComponent<Pine>();

            if (pine.PineState != PineState.ImmaturePine)
            {
                // コードが長くなるのを防ぐために仮の変数に代入
                Vector3 v3 = new Vector3(pine.transform.position.x, _pineTweakHeight, pine.transform.position.z);

                // 未熟なパインをインスタンス化
                GameObject immaturePine = Instantiate(_immaturePineObject, v3, pine.transform.rotation, _generatePos);

                // インスタンス化した未熟なパインを配列に入れる
                pines[randomHeight, randomWidth] = immaturePine.gameObject.GetComponent<Pine>();

                //  ステートを変更
                pines[randomHeight, randomWidth].PineState = PineState.ImmaturePine;

                // 既に生成されているパインを削除
                Destroy(pine.gameObject);

                i++; //ここに書くことで、条件を満たさない限り無限にループする
            }
        }
    }

    void SetImmaturePineCount(Pine[,] pines, int column, int line)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int n = -1; n < 2; n++)
            {
                if (column == i && line == i) continue;
                if (line + n < 0) continue;
                if (line + n > pines.GetLength(1) - 1) continue;
                if (column + i < 0) continue;
                if (column + i > pines.GetLength(0) - 1) continue;

                if (pines[column + i, line + n].PineState != PineState.ImmaturePine)
                {
                    pines[column + i, line + n]._immaturePineCount++;
                }
            }
        }

        foreach (var item in _pines)
        {
            if (item.PineState != PineState.ImmaturePine)
            {
                item.PineState = (PineState)item._immaturePineCount;
            }
        }
    }
}
