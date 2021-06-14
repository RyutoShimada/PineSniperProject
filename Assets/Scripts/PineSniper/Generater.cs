using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 指定された数の畑の上に、指定された個数のオブジェクトを指定された距離で生成する
/// </summary>
public class Generater : MonoBehaviour
{
    /// <summary>生成するオブジェクトのPrefab</summary>
    [SerializeField] GameObject _generatePrefab = null;
    /// <summary>生成する草のPrefab</summary>
    [SerializeField] GameObject _leafPrefab = null;
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
    /// <summary>生成するLeafオブジェクトのY軸</summary>
    [SerializeField] float _leafSetPosY = -1f;
    public Pine[,] _pines { get; private set; }
    public Leaf[,] _leafs { get; private set; }

    /// <summary>生成するフィールドのPrefab</summary>
    [SerializeField] GameObject _field = null;
    /// <summary>X軸に生成するフィールドの数</summary>
    [SerializeField] int _fieldCountX = 2;
    /// <summary>Z軸に生成するフィールドの数</summary>
    [SerializeField] int _fieldCountZ = 2;
    /// <summary>生成するフィールド同士の距離</summary>
    [SerializeField] float _fieldSetDistance = 1f;
    GameObject[,] _fields;

    [SerializeField] GameObject _minePrefab = null;
    public int _mineCount = 0;

    /// <summary>実際にオブジェクトを生成する位置</summary>
    Vector3[,] _generatePositions;

    /// <summary>デバッグ用</summary>
    [SerializeField] bool _leafActiv = true;

    void Start()
    {
        GenerateField();
        SetGenerateObjectPosition();
        GenerateObject();

        if (!_leafActiv)
        {
            foreach (var item in _leafs)
            {
                item.gameObject.SetActive(false);
            }
        }
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
        GameObject[,] pineObjects = new GameObject[_generatePositions.GetLength(0), _generatePositions.GetLength(1)];
        GameObject[,] leafObjects = new GameObject[_generatePositions.GetLength(0), _generatePositions.GetLength(1)];

        _pines = new Pine[pineObjects.GetLength(0), pineObjects.GetLength(1)];
        _leafs = new Leaf[pineObjects.GetLength(0), pineObjects.GetLength(1)];

        for (int z = 0; z < pineObjects.GetLength(0); z++)
        {
            for (int x = 0; x < pineObjects.GetLength(1); x++)
            {                                                                                                    // 0～_fieldCountZ 0～_fieldCount が欲しい
                pineObjects[z, x] = Instantiate(_generatePrefab, _generatePositions[z, x], transform.rotation, _fields[z / _objCountZ, x / _objCountX].transform);
                leafObjects[z, x] = Instantiate(_leafPrefab, _generatePositions[z, x], transform.rotation, _fields[z / _objCountZ, x / _objCountX].transform);

                pineObjects[z, x].transform.LookAt(Camera.main.transform); // 向きをカメラの方に向ける

                _pines[z, x] = pineObjects[z, x].gameObject.GetComponent<Pine>();
                _pines[z, x].PineState = PineState.None; // ステータスを更新
                // インデックス情報を更新
                _pines[z, x].IndexCountX = x;
                _pines[z, x].IndexCountZ = z;

                Vector3 v3 = leafObjects[z, x].transform.position;
                v3.y = _leafSetPosY; // yをリセット
                leafObjects[z, x].transform.position = v3;
                _leafs[z, x] = leafObjects[z, x].gameObject.GetComponent<Leaf>();
                _leafs[z, x].Pine = _pines[z, x]; // Pineの情報を渡す
            }
        }
    }

    /// <summary>
    /// 指定された添え字番号の配列以外から、ランダムにMineを生成
    /// </summary>
    /// <param name="indexCountZ"></param>
    /// <param name="indexCountX"></param>
    public void GenerateMine(int indexCountZ, int indexCountX)
    {
        if (_mineCount > _pines.Length) _mineCount = _pines.Length; //無限ループを防ぐ

        for (int i = 0; i < _mineCount;)
        {
            int randomZ = Random.Range(0, _pines.GetLength(0));
            int randomX = Random.Range(0, _pines.GetLength(1));

            Pine pine = _pines[randomZ, randomX];

            if (pine.PineState != PineState.ImmaturePine && randomZ != indexCountZ && randomX != indexCountX)
            {
                Destroy(_pines[randomZ, randomX].gameObject);
                _pines[randomZ, randomX] = Instantiate(_minePrefab, _pines[randomZ, randomX].transform.position, _pines[randomZ, randomX].transform.rotation).gameObject.GetComponent<Pine>();
                _pines[randomZ, randomX].PineState = PineState.ImmaturePine;
                _leafs[randomZ, randomX].Pine = _pines[randomZ, randomX];
                SetMineCount(randomZ, randomX);
                i++; //ここに書くことで、条件を満たさない限り無限にループする
            }
        }
    }

    /// <summary>
    /// デバッグ用
    /// </summary>
    public void GenerateMine()
    {
        if (_mineCount > _pines.Length) _mineCount = _pines.Length; //無限ループを防ぐ

        for (int i = 0; i < _mineCount;)
        {
            int randomZ = Random.Range(0, _pines.GetLength(0));
            int randomX = Random.Range(0, _pines.GetLength(1));

            Pine pine = _pines[randomZ, randomX];

            if (pine.PineState != PineState.ImmaturePine && randomZ != 0 && randomX != 0)
            {
                Destroy(_pines[randomZ, randomX].gameObject);
                _pines[randomZ, randomX] = Instantiate(_minePrefab, _pines[randomZ, randomX].transform.position + new Vector3(0, -1, 0), _pines[randomZ, randomX].transform.rotation).gameObject.GetComponent<Pine>();
                _pines[randomZ, randomX].PineState = PineState.ImmaturePine;
                _leafs[randomZ, randomX].Pine = _pines[randomZ, randomX];
                SetMineCount(randomZ, randomX);
                i++; //ここに書くことで、条件を満たさない限り無限にループする
            }
        }
    }

    void SetMineCount(int mineZ, int mineX)
    {
        for (int z = -1; z < 2; z++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (mineZ == z && mineX == z) continue;
                if (mineX + x < 0) continue;
                if (mineX + x > _pines.GetLength(1) - 1) continue;
                if (mineZ + z < 0) continue;
                if (mineZ + z > _pines.GetLength(0) - 1) continue;

                if (_pines[mineZ + z, mineX + x].PineState != PineState.ImmaturePine)
                {
                    _pines[mineZ + z, mineX + x].ImmaturePineCount++;
                }
            }
        }

        foreach (var item in _pines)
        {
            if (item.PineState != PineState.ImmaturePine)
            {
                item.PineState = (PineState)item.ImmaturePineCount;
            }
        }
    }
}

