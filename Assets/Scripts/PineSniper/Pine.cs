using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PineState
{
    None = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,

    ImmaturePine = -1,
}

public class Pine : MonoBehaviour
{
    [SerializeField] private Text _view = null;

    [SerializeField] private PineState _pineState = PineState.None;

    /// <summary>地表に出るための高さ</summary>
    [SerializeField] float _pullOutDistance = 1.5f;

    public int _immaturePineCount = 0;

    /// <summary>探索済みかどうか</summary>
    public bool _isSearched = false;

    private int _column = 0;
    private int _line = 0;

    public int IndexColumn
    {
        get => _column;
        set
        {
            _column = value;
        }
    }

    public int IndexLine
    {
        get => _line;
        set
        {
            _line = value;
        }
    }

    public PineState PineState
    {
        get => _pineState;
        set
        {
            _pineState = value;
            OnCellStateChanged();
        }
    }

    private void OnValidate()
    {
        OnCellStateChanged();
    }

    private void OnCellStateChanged()
    {
        if (_view == null) { return; }

        if (_pineState == PineState.None)
        {
            _view.text = "";
        }
        else if (_pineState == PineState.ImmaturePine)
        {
            _view.text = "X";
            _view.color = Color.red;
        }
        else
        {
            _view.text = ((int)_pineState).ToString();
            _view.color = Color.blue;
        }
    }

    /// <summary>
    /// 土に埋もれている状態から地表へ移動する
    /// </summary>
    public void PullOut()
    {
        Vector3 v3 = this.gameObject.transform.position;
        v3.y += _pullOutDistance;
        this.gameObject.transform.position = v3;
    }
}
