using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>スコープを覗いたときに表示するUIのイメージ</summary>
    [SerializeField] Image _scope = null;
    /// <summary>初期位置にあるカメラ</summary>
    [SerializeField] GameObject _originVcam = null;

    [SerializeField] GameObject _field = null;

    [SerializeField] int _fieldsWidth = 1;

    [SerializeField] float _fieldSpace = 0.25f;

    [SerializeField] PineGeneraterMaster _generaterMaster;

    /// <summary>スコープを覗いているか</summary>
    bool _isScope;

    PineGenerater _pine;

    PineGeneraterMaster _master;
    
    // Start is called before the first frame update
    void Start()
    {
        _master = FindObjectOfType<PineGeneraterMaster>();
        _isScope = false;
        _scope.gameObject.SetActive(false);
        CreateFields();
    }

    // Update is called once per frame
    void Update()
    {
        Ray();
    }

    /// <summary>
    /// スコープUIを表示する
    /// </summary>
    public void OnScope()
    {
        _pine = FindObjectOfType<PineGenerater>();
        _master.Generate();
        _isScope = true;
        _scope.gameObject.SetActive(true);
        _originVcam.SetActive(false);
        Cursor.visible = false;
    }

    void Ray()
    {
        if (!_isScope) return;

        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                if (hit.collider.gameObject.name == "Leaf(Clone)")
                {
                    Leaf leaf = hit.collider.gameObject.GetComponent<Leaf>();

                    //Debug.Log($"Pine : [{_pine._pines[leaf.IndexColumn, leaf.IndexLine].gameObject.name}]");

                    Pine pine = _pine._pines[leaf.IndexColumn, leaf.IndexLine].gameObject.GetComponent<Pine>();

                    pine.PullOut();

                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }

    void CreateFields()
    {
        float fieldWidthX = _field.transform.Find("Soil").GetComponent<Renderer>().bounds.size.x;
        float fieldWidthZ = _field.transform.Find("Soil").GetComponent<Renderer>().bounds.size.z;

        GameObject[,] fields = new GameObject[_fieldsWidth, _fieldsWidth];
        for (int column = 0; column < _fieldsWidth; column++)
        {
            for (int line = 0; line < _fieldsWidth; line++)
            {
                fields[column, line] = Instantiate(_field);
                Vector3 v3 = new Vector3((line * fieldWidthX) * _fieldSpace, 0, (column * fieldWidthZ) * _fieldSpace);
                fields[column, line].gameObject.transform.position = v3;
                _generaterMaster._generaterList.Add(fields[column, line].gameObject.transform.Find("PineGenerater").GetComponent<PineGenerater>());
                _generaterMaster._fieldWidth = _fieldsWidth;
            }
        }
    }
}
