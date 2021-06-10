using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPineGenerater : MonoBehaviour
{
    [SerializeField] int _columnCount = 3;
    [SerializeField] int _lineCount = 5;
    [SerializeField] float _columnSetDistance = 1f;
    [SerializeField] float _lineSetDistance = 1f;
    [SerializeField] Transform _generatePos = null;

    private void Start()
    {
        foreach (var item in GeneratePos(2, 2, 2f))
        {
            Debug.Log(item);
        }
    }

    public Vector3[,] GeneratePos(int masterColumn, int masterLine, float fieldDistance)
    {
        Vector3[,] generatePositions = new Vector3[_columnCount * masterColumn, _lineCount * masterLine];

        //float fieldX = this.transform.Find("Soil").GetComponent<Renderer>().bounds.size.x; // FieldオブジェクトのX軸の幅
        //float fieldZ = this.transform.Find("Soil").GetComponent<Renderer>().bounds.size.z; // FieldオブジェクトのZ軸の幅

        float x = _generatePos.position.x; // 生成する原点X
        float z = _generatePos.position.z; // 生成する原点Z

        //int masterCountC = 0; // 現在の畑のColumn位置
        //int masterCountL = 0; // 現在の畑のLine位置

        //for (int c = 0; c < generatePositions.GetLength(0); c++)
        //{
        //    for (int l = 0; l < generatePositions.GetLength(1); l++)
        //    {
        //        // 生成する原点X + (l * 行の調整) + ((FieldオブジェクトのX軸の幅 + Fieldオブジェクト同士の間隔) * 現在の畑のColumn位置)
        //        //float posX = x + (l * _lineSetDistance) + ((fieldX + fieldDistance) * masterCountL);
        //        float posX = x + (l * _lineSetDistance) + ((4 + fieldDistance) * masterCountL);
        //        //float posZ = z + (c * _columnSetDistance) + ((fieldZ + fieldDistance) * masterCountC);
        //        float posZ = z + (c * _columnSetDistance) + ((4 + fieldDistance) * masterCountC);

        //        if (l >= _lineCount)
        //        {
        //            masterCountL++;
        //        }

        //        generatePositions[c, l] = new Vector3(posX, 0, posZ);
        //    }

        //    if (c >= _columnCount)
        //    {
        //        masterCountC++;
        //    }
        //}


        return generatePositions;
    }
}

