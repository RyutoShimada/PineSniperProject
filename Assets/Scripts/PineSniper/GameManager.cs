using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Image _scope = null;
    [SerializeField] GameObject _vcam = null;

    bool _isScope;

    // Start is called before the first frame update
    void Start()
    {
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
        _vcam.SetActive(false);
    }

    void Ray()
    {
        if (!_isScope) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (hit.collider.gameObject.name == "Leaf(Clone)")
                {
                    Debug.Log(hit.collider.gameObject.name);
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }
    }
}
