using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定された数の畑の上に、指定された個数のオブジェクトを指定された距離で生成する
/// </summary>
public class GenerateMaster : MonoBehaviour
{
    /// <summary>生成するオブジェクトのPrefab</summary>
    [SerializeField] GameObject _generatePrefab = null;
    /// <summary>Z軸に生成するオブジェクトの数</summary>
    [SerializeField] int _objCountZ = 3;
    /// <summary>X軸に生成するオブジェクトの数</summary>
    [SerializeField] int _objCountX = 5;
    /// <summary>Z軸に生成するオブジェクト同士の距離</summary>
    [SerializeField] float _objSetDistanceZ = 1f;
    /// <summary>X軸に生成するオブジェクト同士の距離</summary>
    [SerializeField] float _objSetDistanceX = 1f;
    /// <summary>生成するオブジェクトのY軸</summary>
    [SerializeField] float _objSetPosY = -1f;
    GameObject[,] _objects;
    Pine[,] _pines;

    /// <summary>生成するフィールドのPrefab</summary>
    [SerializeField] GameObject _field = null;
    /// <summary>X軸に生成するフィールドの数</summary>
    [SerializeField] int _fieldCountX = 2;
    /// <summary>Z軸に生成するフィールドの数</summary>
    [SerializeField] int _fieldCountZ = 2;
    /// <summary>生成するフィールド同士の距離</summary>
    [SerializeField] float _fieldSetDistance = 1f;
    GameObject[,] _fields;

    /// <summary>実際にオブジェクトを生成する位置</summary>
    Vector3[,] _generatePositions;

    void Start()
    {
        GenerateField();
        SetGenerateObjectPosition();
        GenerateObject();
    }

    private void OnValidate()
    {
        //GenerateField();
    }

    void GenerateField()
    {
        _fields = new GameObject[_fieldCountZ, _fieldCountX];

        float fieldWidthX = _field.transform.Find("Soil").GetComponent<Renderer>().bounds.size.x;
        float fieldWidthZ = _field.transform.Find("Soil").GetComponent<Renderer>().bounds.size.z;

        // generatePosと生成したfieldの位置のずれを修正し角に合わせる
        float fixedPosX = fieldWidthX / 2;
        float fixedPosZ = fieldWidthX / 2;

        for (int z = 0; z < _fieldCountZ; z++)
        {
            for (int x = 0; x < _fieldCountX; x++)
            {
                _fields[z, x] = Instantiate(_field, this.transform);

                float setX = (fieldWidthX + _fieldSetDistance) * x;
                float setZ = (fieldWidthZ + _fieldSetDistance) * z;

                _fields[z, x].transform.position = new Vector3(setX + fixedPosX, 0, -setZ - fixedPosZ);
            }
        }

        // ずれたフィールドたちを生成したい位置にもどす処理
        float returnOriginX = (((fieldWidthX + _fieldSetDistance) * _fieldCountX) - _fieldSetDistance) / 2;
        float returnOriginZ = (((fieldWidthZ + _fieldSetDistance) * _fieldCountZ) - _fieldSetDistance) / 2;
        transform.position = new Vector3(transform.position.x - returnOriginX, 0, transform.position.z + returnOriginZ);
    }

    void SetGenerateObjectPosition()
    {
        _generatePositions = new Vector3[_objCountZ * _fieldCountZ, _objCountX * _fieldCountX];

        for (int z = 0; z < _generatePositions.GetLength(0); z++)
        {
            for (int x = 0; x < _generatePositions.GetLength(1); x++)
            {
                // 基準となるオブジェクトの生成位置　_field[z, x]にあるオブジェクトを生成し始める位置 + (0～4 * _objSetDistanceX & _objSetDistanceX)
                Vector3 criteriaPos = _fields[z / _objCountZ, x / _objCountX].transform.Find("ObjectGeneratePosition").gameObject.transform.position;
                float genarateObjX = (x - (_objCountX * (x / _objCountX))) * _objSetDistanceX; // オブジェクトを生成する位置X (0～objCountXだけで計算したい)
                float genarateObjZ = (z - (_objCountZ * (z / _objCountZ))) * _objSetDistanceZ; // オブジェクトを生成する位置Z (0～objCountZだけで計算したい)
                _generatePositions[z, x] = new Vector3(criteriaPos.x + genarateObjX, _objSetPosY, criteriaPos.z - genarateObjZ);
            }
        }
    }

    void GenerateObject()
    {
        // ここで実際にオブジェクトを生成

        _objects = new GameObject[_generatePositions.GetLength(0), _generatePositions.GetLength(1)];
        _pines = new Pine[_objects.GetLength(0), _objects.GetLength(1)];

        for (int z = 0; z < _objects.GetLength(0); z++)
        {
            for (int x = 0; x < _objects.GetLength(1); x++)
            {                                                                                                    // 0～_fieldCountZ 0～_fieldCount が欲しい
                _objects[z, x] = Instantiate(_generatePrefab, _generatePositions[z, x], transform.rotation, _fields[z / _objCountZ, x / _objCountX].transform);
                _pines[z, x] = _objects[z, x].gameObject.GetComponent<Pine>();
                _pines[z, x].PineState = PineState.None;
            }
        }
    }
}

