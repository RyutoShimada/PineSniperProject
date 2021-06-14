using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeController : MonoBehaviour
{
    [SerializeField] float _gaugeTime = 1.0f;
    [SerializeField] GameObject _grenadePrefab = null;
    [SerializeField] float _generateHeight = 0;
    Image _gauge = null;
    float _time;
    public bool _isSuccessGauge { get; private set; }
    private bool _isGauge = false;

    Leaf _leaf = null;

    bool _onButton = false;

    public Leaf Leaf
    {
        get => _leaf;
        set
        {
            _leaf = value;
        }
    }

    public float GaugeTime
    {
        get => _gaugeTime;
        //set
        //{
        //    _gaugeTime = value;
        //}
    }

    public bool IsGauge
    {
        get => _isGauge;
        set
        {
            _isGauge = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _gauge = GetComponent<Image>();
        _time = _gaugeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGauge)
        {
            Cursor.visible = true;
            _time -= Time.deltaTime;
            _gauge.fillAmount -= 1.0f / _gaugeTime * Time.deltaTime;

            if (_onButton && _time > 0)
            {
                ResetGauge();
                _isSuccessGauge = true;
                Vector3 v3 = _leaf.transform.position;
                v3.y = _generateHeight;
                Instantiate(_grenadePrefab, v3, _leaf.transform.rotation);
            }
            else if (_time <= 0)
            {
                ResetGauge();
                _isSuccessGauge = false;
            }

        }
    }
    void ResetGauge()
    {
        Cursor.visible = false;
        gameObject.SetActive(false);
        _gauge.fillAmount = 1.0f;
        _time = _gaugeTime;
        _isGauge = false;
        _onButton = false;
    }

    public void OnButtton()
    {
        _onButton = true;
    }
}
