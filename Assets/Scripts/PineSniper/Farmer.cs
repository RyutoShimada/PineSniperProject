using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farmer : MonoBehaviour
{
    public List<PineGenerater> _generaterList = new List<PineGenerater>();

    public int _fieldWidth { get; set; }

    public Pine[,] _pines { get; private set; }

    public Leaf[,] _leafs { get; private set; }

    public void Generate()
    {
        _pines = new Pine[_fieldWidth * _generaterList[0]._pines.GetLength(0), _fieldWidth * _generaterList[0]._pines.GetLength(1)];
        _leafs = new Leaf[_fieldWidth * _generaterList[0]._pines.GetLength(0), _fieldWidth * _generaterList[0]._pines.GetLength(1)];

        List<Pine> pList = new List<Pine>();
        List<Leaf> lList = new List<Leaf>();

        // 一度PineのListに入れることで、配列に入れ替える作業を楽にしている
        for (int n = 0; n < _generaterList.Count; n++)
        {
            for (int column = 0; column < _generaterList[0]._pines.GetLength(0); column++)
            {
                for (int line = 0; line < _generaterList[0]._pines.GetLength(1); line++)
                {
                    pList.Add(_generaterList[n]._pines[column, line]);
                    lList.Add(_generaterList[n]._leafs[column, line]);
                }
            }
        }

        // 配列に入れ替え
        int i = 0;

        for (int column = 0; column < _fieldWidth * _generaterList[0]._pines.GetLength(0); column++)
        {
            for (int line = 0; line < _fieldWidth * _generaterList[0]._pines.GetLength(1); line++)
            {
                _pines[column, line] = pList[i];
                _leafs[column, line] = lList[i];

                _leafs[column, line].MasterIndexColumn = column;
                _leafs[column, line].MasterIndexLine = line;

                Debug.Log($"{_pines[column, line].gameObject.name} : [{column}][{line}]");
                i++;
            }
        }
    }   
}

