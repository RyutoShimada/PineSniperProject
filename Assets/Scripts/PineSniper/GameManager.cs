using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    /// <summary>スコープを覗いたときに表示するUIのイメージ</summary>
    [SerializeField] Image _scope = null;
    /// <summary>初期位置にあるカメラ</summary>
    [SerializeField] GameObject _originVcam = null;
    /// <summary>スコープのカメラ</summary>
    [SerializeField] CinemachineVirtualCamera _scopeVcam = null;
    /// <summary>ズームした時の距離</summary>
    [SerializeField] float _zoomDis = 20f;
    /// <summary>Pineを地表に出す距離</summary>
    [SerializeField] float _pullOutDistance = 1.5f;

    [SerializeField] SniperRifleController _sniper = null;

    Generater _generater;

    /// <summary>ゲーム中か</summary>
    bool _isGame = false;
    /// <summary>スコープを覗いているか</summary>
    bool _isScope;
    /// <summary>初めて撃ったか</summary>
    bool _isFirst = true;
    /// <summary>ズームしているか</summary>
    bool _isZoom = false;
    /// <summary>ズームする時の初期位置</summary>
    float _originView;

    // Start is called before the first frame update
    void Start()
    {
        _generater = FindObjectOfType<Generater>();
        _isScope = false;
        _scope.gameObject.SetActive(false);
        _originView = _scopeVcam.m_Lens.FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isScope) { _sniper.Idol(); }
        if (_isGame) { Ray(); }
    }

    /// <summary>
    /// スコープUIを表示する
    /// </summary>
    public void OnScope()
    {
        _isGame = true;
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
                if (hit.collider.tag == "LeafTag")
                {
                    Leaf leaf = hit.collider.gameObject.GetComponent<Leaf>();

                    if (_isFirst)
                    {
                        _generater.GenerateMine(leaf._pine.IndexCountZ, leaf._pine.IndexCountX);
                        _isFirst = false;
                    }

                    if (leaf._pine.PineState == PineState.None)
                    {
                        Searching(leaf._pine.IndexCountZ, leaf._pine.IndexCountX);
                    }
                    else
                    {
                        leaf._pine.PullOut(_pullOutDistance);
                        leaf.SetActivFalse();
                        leaf._pine.IsSearched = true;
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            float view = 0;

            if (_isZoom)
            {
                //ズームアウト
                view = _originView;
                _scopeVcam.m_Lens.FieldOfView = Mathf.Clamp(value: view, min: _zoomDis, max: _originView);
                _isZoom = false;
            }
            else
            {
                //ズームイン
                view = _scopeVcam.m_Lens.FieldOfView - _zoomDis;
                _scopeVcam.m_Lens.FieldOfView = Mathf.Clamp(value: view, min: _zoomDis, max: _originView);
                _isZoom = true;
            }
        }
    }

    void Searching(int indexZ, int indexX)
    {
        if (_generater._pines[indexZ, indexX].PineState == PineState.None)
        {
            for (int z = -1; z < 2; z++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (indexZ == z && indexX == z) continue;
                    if (indexX + x < 0) continue;
                    if (indexX + x > _generater._pines.GetLength(1) - 1) continue;
                    if (indexZ + z < 0) continue;
                    if (indexZ + z > _generater._pines.GetLength(0) - 1) continue;

                    if (_generater._pines[indexZ + z, indexX + x].PineState != PineState.ImmaturePine && !_generater._pines[indexZ + z, indexX + x].gameObject.GetComponent<Pine>().IsSearched)
                    {
                        _generater._leafs[indexZ + z, indexX + x].SetActivFalse();
                        _generater._pines[indexZ + z, indexX + x].gameObject.GetComponent<Pine>().IsSearched = true;
                        _generater._pines[indexZ + z, indexX + x].PullOut(_pullOutDistance);
                        Searching(indexZ + z, indexX + x);
                    }
                }
            }
        }
    }
}
