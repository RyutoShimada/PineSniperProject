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

    [SerializeField] float _pullOutDistance = 1.5f;

    Generater _generater;

    /// <summary>スコープを覗いているか</summary>
    bool _isScope;

    bool _isFirst = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _generater = FindObjectOfType<Generater>();
        _isScope = false;
        _scope.gameObject.SetActive(false);
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
    }
}
