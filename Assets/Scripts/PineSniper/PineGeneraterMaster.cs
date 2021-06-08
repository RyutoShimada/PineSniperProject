using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineGeneraterMaster : MonoBehaviour
{
    public List<PineGenerater> _generaterList = new List<PineGenerater>();

    public int _fieldWidth { get; set; }

    Pine[,] _pines;

    public void Generate()
    {
        _pines = new Pine[_fieldWidth * _generaterList[0]._pines.GetLength(0), _fieldWidth * _generaterList[0]._pines.GetLength(1)];

        List<Pine> pList = new List<Pine>();

        int n = 0;

        // 一度PineのListに入れることで、配列に入れ替える工程を楽にしている
        for (int column = 0; column < _generaterList[0]._pines.GetLength(0); column++)
        {
            for (int line = 0; line < _generaterList[0]._pines.GetLength(1); line++)
            {
                pList.Add(_generaterList[n]._pines[column, line]);
            }
            n++;
        }

        // 配列に入れ替え
        int i = 0;
        for (int column = 0; column < _fieldWidth * _generaterList[0]._pines.GetLength(0); column++)
        {
            for (int line = 0; line < _fieldWidth * _generaterList[0]._pines.GetLength(1); line++)
            {
                _pines[column, line] = pList[i];
                Debug.Log($"{_pines[column, line].gameObject.name}, [{column}][{line}]");
            }
            i++;
        }
    }
}
