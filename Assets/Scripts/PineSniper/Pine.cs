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

    [SerializeField] float _pullOutDistance = 0.25f;

    //public int m_bomCount = 0;

    private int m_height = 0;
    private int m_width = 0;

    public int IndexHegit
    {
        get => m_height;
        set
        {
            m_height = value;
        }
    }

    public int IndexWidth
    {
        get => m_width;
        set
        {
            m_width = value;
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
