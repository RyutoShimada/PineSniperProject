using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private int m_height = 0;
    private int m_width = 0;

    public bool m_open = false;
    public bool m_flag = false;

    MinesweeperManager m_MM = null;

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

    private void Start()
    {
        m_MM = FindObjectOfType<MinesweeperManager>();
        gameObject.transform.Find("Flag").gameObject.SetActive(false);
    }

    void Search()
    {
        m_MM.Searching(m_height, m_width);
        m_MM.IsBom(m_height, m_width);
    }

    public void ActiveFalse()
    {
        this.gameObject.SetActive(false);
    }

    public void Click()
    {
        m_open = true;
        m_MM.m_openCells++;
        m_MM.FirstClick();
        Search();
        ActiveFalse();
    }

    public void OnFlag()
    {
        gameObject.transform.Find("Flag").gameObject.SetActive(true);
        m_flag = true;
    }

    public void UnFlag()
    {
        gameObject.transform.Find("Flag").gameObject.SetActive(false);
        m_flag = false;
    }
}
