using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
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

    Mine = -1,


}
public class Cell : MonoBehaviour
{
    [SerializeField] private Text _view = null;

    [SerializeField] private CellState _cellState = CellState.None;

    public int m_bomCount = 0;

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

    public CellState CellState 
    {
        get => _cellState;
        set 
        {
            _cellState = value;
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

        if (_cellState == CellState.None)
        {
            _view.text = "";
        }
        else if (_cellState == CellState.Mine)
        {
            _view.text = "X";
            _view.color = Color.red;
        }
        else
        {
            _view.text = ((int)_cellState).ToString();
            _view.color = Color.blue;
        }
    }

}
