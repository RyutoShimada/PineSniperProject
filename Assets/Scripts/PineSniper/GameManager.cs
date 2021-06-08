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

    PineGenerater _pine;

    bool _isScope;

    // Start is called before the first frame update
    void Start()
    {
        _pine = FindObjectOfType<PineGenerater>();
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

                    Debug.Log($"Pine : [{_pine._pines[leaf.IndexColumn, leaf.IndexLine].gameObject.name}]");

                    Pine pine = _pine._pines[leaf.IndexColumn, leaf.IndexLine].gameObject.GetComponent<Pine>();

                    pine.PullOut();

                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
}
