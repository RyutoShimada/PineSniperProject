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

    [SerializeField] float _scopeDis = 5f;

    [SerializeField] float _pullOutDistance = 1.5f;

    Vector3 _originScopePos;

    Generater _generater;

    /// <summary>スコープを覗いているか</summary>
    bool _isScope;

    bool _isFirst = true;

    bool _isZoom = false;

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
        Ray();
    }

    /// <summary>
    /// スコープUIを表示する
    /// </summary>
    public void OnScope()
    {
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

                    if (_isFirst)
                    {
                        _generater.GenerateMine(leaf._pine.IndexCountZ, leaf._pine.IndexCountX);
                        _isFirst = false;
                    }

                    leaf._pine.PullOut(_pullOutDistance);
                    leaf.SetActivFalse();
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
                _scopeVcam.m_Lens.FieldOfView = Mathf.Clamp(value: view, min: _scopeDis, max: _originView);
                _isZoom = false;
            }
            else
            {
                //ズームイン
                view = _scopeVcam.m_Lens.FieldOfView - _scopeDis;
                _scopeVcam.m_Lens.FieldOfView = Mathf.Clamp(value: view, min: _scopeDis, max: _originView);
                _isZoom = true;
            }
        }
    }
}
