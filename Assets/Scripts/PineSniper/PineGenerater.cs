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
    Pine[,] _pines = null;

    GameObject[,] _go;

    // Start is called before the first frame update
    void Start()
    {
        _pines = new Pine[_column, _line];
        _go = new GameObject[_column, _line];
        GeneratePine(_pines, _go);
        GenerateImmaturePine(_pines, _go, immaturePineCount);
    }

    /// <summary>
    /// Pineの生成
    /// </summary>
    void GeneratePine(Pine[,] pines, GameObject[,] go)
    {
        // ここでPineを生成する
        for (int h = 0; h < pines.GetLength(0); h++)
        {
            for (int w = 0; w < pines.GetLength(1); w++)
            {
                Vector3 pinePos = _generatePos.position + new Vector3(w * _tweakOfLine, _pineTweakHeight, -h * _tweakOfColumn);
                Vector3 leafPos = _generatePos.position + new Vector3(w * _tweakOfLine, _leafTweakHeight, -h * _tweakOfColumn);

               // Pineの配列にインスタンス化したものを入れるために一度変換している
                go[h, w] = Instantiate(_pineObject, pinePos, _generatePos.rotation, _generatePos);
                // Leafを生成
                Instantiate(_leafObject, leafPos, _generatePos.rotation, _generatePos);

                pines[h, w] = go[h, w].GetComponent<Pine>();
                pines[h, w].PineState = PineState.None;
            }
        }
    }

    void GenerateImmaturePine(Pine[,] pines, GameObject[,] go, int count)
    {
        int[] immaturePineH = new int[count];
        int[] immaturePineW = new int[count];

        for (int i = 0; i < count; i++)
        {
            RandomInt(count, i, immaturePineH);
            RandomInt(count, i, immaturePineW);
        }

        for (int p = 0; p < count; p++)
        {
            GameObject immaturePine = go[immaturePineH[p], immaturePineW[p]].gameObject;
            Debug.Log($"[{immaturePineH[p]}][{immaturePineW[p]}]");
            Instantiate(_immaturePineObject, immaturePine.transform.position + new Vector3(0, _pineTweakHeight, 0), immaturePine.transform.rotation);
            Destroy(immaturePine.gameObject);
        }
    }

    /// <summary>
    /// ランダムな整数を生成する
    /// </summary>
    /// <param name="count">生成する個数</param>
    /// <param name="index">添え字番号</param>
    /// <param name="array">指定された個数分生成されたランダムな整数を入れる配列</param>
    void RandomInt(int count, int index, int[] array)
    {
        while (true)
        {
            int random = Random.Range(0, count);

            if (random != array[index - index])
            {
                array[index] = random;
            }
            else
            {
                break;
            }
        }
    }
}
