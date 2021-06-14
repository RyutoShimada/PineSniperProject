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
    /// <summary>制限時間</summary>
    [SerializeField] float _setTimeSecond = 0;
    [SerializeField] SniperRifleController _sniper = null;
    [SerializeField] Text _uriageText = null;
    [SerializeField] Text _pineCountText = null;
    [SerializeField] Text _mineCountText = null;
    [SerializeField] Text _timerText = null;
    [SerializeField] GameObject _gaugeObj = null;
    [SerializeField] GameObject _resultPanel = null;
    [SerializeField] GameObject _startText = null;
    GaugeController _gaugeController;
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
    float _originView = 0;
    /// <summary>売上金</summary>
    int _uriage = 0;
    /// <summary>現在のPineの数</summary>
    int _currentPineCount = 0;
    /// <summary>全てのPineの数</summary>
    int _totalPineCount = 0;
    /// <summary>現在のMineの数</summary>
    int _currentMineCount = 0;
    /// <summary>全てのMineの数</summary>
    int _totalMineCount = 0;

    bool _doneSearching = false;

    float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _generater = FindObjectOfType<Generater>();
        _isScope = false;
        _scope.gameObject.SetActive(false);
        _originView = _scopeVcam.m_Lens.FieldOfView;
        _gaugeController = _gaugeObj.GetComponent<GaugeController>();
        _timer = _setTimeSecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isScope && !_resultPanel.activeSelf)
        {
            _sniper.Idol();
        }

        if (_isGame)
        {
            _timer -= Time.deltaTime;

            _timerText.text = $"タイム  {_timer.ToString("00")}ビョウ";

            if (_gaugeController.IsGauge) return;
            Ray();
            MonitoringGame();
        }
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
        _startText.SetActive(false);
        Cursor.visible = false;
    }

    void OffScope()
    {
        _isGame = false;
        _isScope = false;
        _scope.gameObject.SetActive(false);
        _originVcam.SetActive(true);
        Cursor.visible = true;
    }

    void Ray()
    {
        if (!_isScope) return;

        RaycastHit hit;

        if (Input.GetButtonDown("Fire1"))
        {
            _sniper.ShotSE();

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                if (hit.collider.tag == "LeafTag")
                {
                    Leaf leaf = hit.collider.gameObject.GetComponent<Leaf>();

                    _doneSearching = false; // ここで呼ぶことで、再起呼び出しした時に引っかからなくなる

                    if (_isFirst)
                    {
                        _generater.GenerateMine(leaf.Pine.IndexCountZ, leaf.Pine.IndexCountX);
                        _totalMineCount = _generater._mineCount;
                        _totalPineCount = _generater._pines.Length - _totalMineCount;
                        _isFirst = false;
                    }

                    if (leaf.Pine.PineState == PineState.None)
                    {
                        _gaugeObj.SetActive(true);
                        _gaugeController.IsGauge = true;
                        _gaugeController.Leaf = leaf;
                        StartCoroutine(Searching(leaf.Pine.IndexCountZ, leaf.Pine.IndexCountX));

                    }
                    else
                    {
                        leaf.Pine.PullOut(_pullOutDistance);
                        leaf.InstancePrticle();
                        leaf.Pine.IsSearched = true;
                        GamingScore(leaf.Pine);
                    }
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            float view;

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

    IEnumerator Searching(int indexZ, int indexX)
    {
        if (!_doneSearching)
        {
            _doneSearching = true;
            yield return new WaitForSeconds(_gaugeController.GaugeTime);
        }

        if (!_gaugeController._isSuccessGauge)
        {
            GamingScore(_generater._pines[indexZ, indexX].gameObject.GetComponent<Pine>());
            _generater._leafs[indexZ, indexX].InstancePrticle();
            _generater._pines[indexZ, indexX].gameObject.GetComponent<Pine>().IsSearched = true;
            _generater._pines[indexZ, indexX].PullOut(_pullOutDistance);
            yield return null;
        }
        else
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
                            GamingScore(_generater._pines[indexZ + z, indexX + x].gameObject.GetComponent<Pine>());
                            _generater._leafs[indexZ + z, indexX + x].InstancePrticle();
                            _generater._pines[indexZ + z, indexX + x].gameObject.GetComponent<Pine>().IsSearched = true;
                            _generater._pines[indexZ + z, indexX + x].PullOut(_pullOutDistance);
                            StartCoroutine(Searching(indexZ + z, indexX + x));
                        }
                    }
                }
            }
        }
    }

    void GamingScore(Pine pine)
    {
        if (pine.PineState != PineState.ImmaturePine)
        {
            _currentPineCount++;
            _uriage += pine._pineScore;
        }
        else
        {
            _currentMineCount++;
            _uriage -= pine._mineScore;
        }

        _pineCountText.text = $"パイン  {_currentPineCount.ToString("00")} / {_totalPineCount.ToString("00")}";
        _mineCountText.text = $"ダメパイン  {_currentMineCount.ToString("00")} / {_totalMineCount.ToString("00")}";
        _uriageText.text = $"ウリアゲ   ￥{_uriage}";
    }

    void MonitoringGame()
    {
        if (!_isGame || !_isScope || _isFirst || _gaugeController.IsGauge) return;

        if (_currentPineCount == _totalPineCount || _currentMineCount == _totalMineCount || _timer <= 0)
        {
            // GameOver
            OffScope();
            // リザルト画面Panelを表示
            _resultPanel.SetActive(true);
            int pineScore = _generater._pines[0, 0]._pineScore;
            int mineScore = _generater._pines[0, 0]._mineScore;
            _resultPanel.GetComponent<ResultScoreManager>().ResultScore(_currentPineCount, pineScore, _currentMineCount, mineScore, _totalMineCount, _timer, _setTimeSecond);
        }
    }
}
