using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinesweeperManager : MonoBehaviour
{
    [SerializeField] Cell m_cellPrefab = null;
    [SerializeField] GameObject m_buttonPrefab = null;
    [SerializeField] GridLayoutGroup m_gridLayoutGroup = null;
    [SerializeField] GridLayoutGroup m_gridLayoutGroupCover = null;
    [SerializeField] Text m_gameOverText = null;
    [SerializeField] int m_setBomCount = 0;
    [SerializeField] int m_width = 0;
    [SerializeField] int m_height = 0;
    Cell[,] m_cell;
    GameObject[,] m_button;
    public int m_openCells = 0;
    bool m_isGameOver = false;
    bool m_isFirstClick = false;
    float m_time = 0;
    PointerEventData m_pointer;

    // Start is called before the first frame update
    void Start()
    {
        m_gridLayoutGroup.constraintCount = m_height;
        m_gridLayoutGroupCover.constraintCount = m_height;
        CreateCell(ref m_cell, m_height, m_width, m_cellPrefab, m_gridLayoutGroup.transform);
        CreateButton(ref m_button, m_height, m_width, m_buttonPrefab, m_gridLayoutGroupCover.transform, m_cell);
        m_pointer = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        m_time += Time.deltaTime;
        OnFlag();
    }

    /// <summary>
    /// Cellを指定された場所に、指定された個数(高さ * 幅)で生成する
    /// </summary>
    /// <param name="cell">Cell型の二次元配列</param>
    /// <param name="height">高さ</param>
    /// <param name="width">幅</param>
    /// <param name="prefab">生成するCellのPregfab</param>
    /// <param name="createPos">生成する位置</param>
    void CreateCell(ref Cell[,] cell, int height, int width, Cell prefab, Transform createPos)
    {
        cell = new Cell[height, width];

        for (int h = 0; h < cell.GetLength(0); h++)
        {
            for (int w = 0; w < cell.GetLength(1); w++)
            {
                cell[h, w] = Instantiate(prefab);
                cell[h, w].transform.SetParent(createPos);
                cell[h, w].IndexHegit = h;
                cell[h, w].IndexWidth = w;
            }
        }
    }

    /// <summary>
    /// Cellを指定された場所に、指定された個数(高さ * 幅)で生成する
    /// </summary>
    /// <param name="button">Cell型の二次元配列</param>
    /// <param name="height">高さ</param>
    /// <param name="width">幅</param>
    /// <param name="prefab">生成するCellのPregfab</param>
    /// <param name="createPos">生成する位置</param>
    void CreateButton(ref GameObject[,] button, int height, int width, GameObject prefab, Transform pearent, Cell[,] cell)
    {
        button = new GameObject[height, width];

        for (int h = 0; h < button.GetLength(0); h++)
        {
            for (int w = 0; w < button.GetLength(1); w++)
            {
                button[h, w] = Instantiate(prefab);
                button[h, w].transform.SetParent(pearent);
                button[h, w].transform.position = cell[h, w].transform.position;
                button[h, w].gameObject.transform.Find("Button").GetComponent<ButtonController>().IndexHegit = h;
                button[h, w].gameObject.transform.Find("Button").GetComponent<ButtonController>().IndexWidth = w;
            }
        }
    }

    void AllButtonActiveFalse()
    {
        foreach (var item in m_button)
        {
            if (item.gameObject.activeSelf == true)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Bomを指定された個数でランダムに生成する
    /// </summary>
    /// <param name="count">個数</param>
    void CreateBom(int count)
    {
        if (count > m_height * m_width) count = m_height * m_width; //無限ループを防ぐ

        for (int i = 0; i < count;)
        {
            int randomHeight = Random.Range(0, m_height);
            int randomWidth = Random.Range(0, m_width);

            Cell cell = m_cell[randomHeight, randomWidth]; //参照渡し

            if (cell.CellState != CellState.Mine && m_button[randomHeight, randomWidth].gameObject.transform.Find("Button").GetComponent<ButtonController>().m_open == false)
            {
                cell.CellState = CellState.Mine;
                SetBomCount(ref m_cell, randomHeight, randomWidth);
                i++; //ここに書くことで、条件を満たさない限り無限にループする
            }
        }
    }

    void SetBomCount(ref Cell[,] bomCell, int h, int w)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int n = -1; n < 2; n++)
            {
                if (h == i && w == i) continue;
                if (w + n < 0) continue;
                if (w + n > bomCell.GetLength(1) - 1) continue;
                if (h + i < 0) continue;
                if (h + i > bomCell.GetLength(0) - 1) continue;

                if (bomCell[h + i, w + n].CellState != CellState.Mine)
                {
                    bomCell[h + i, w + n].m_bomCount++;
                }
            }
        }
    }

    public void Searching(int h, int w)
    {
        if (m_cell[h, w].CellState == CellState.None)
        {
            m_button[h, w].gameObject.transform.Find("Button").GetComponent<ButtonController>().ActiveFalse();

            for (int i = -1; i < 2; i++)
            {
                for (int n = -1; n < 2; n++)
                {
                    if (h == i && w == i) continue;
                    if (w + n < 0) continue;
                    if (w + n > m_cell.GetLength(1) - 1) continue;
                    if (h + i < 0) continue;
                    if (h + i > m_cell.GetLength(0) - 1) continue;

                    if (m_cell[h + i, w + n].CellState != CellState.Mine && m_button[h + i, w + n].gameObject.transform.Find("Button").GetComponent<ButtonController>().m_open == false)
                    {
                        m_button[h + i, w + n].gameObject.transform.Find("Button").GetComponent<ButtonController>().ActiveFalse();
                        m_button[h + i, w + n].gameObject.transform.Find("Button").GetComponent<ButtonController>().m_open = true;
                        m_openCells++;
                        Searching(h + i, w + n);
                    }
                }
            }
        }

        m_button[h, w].gameObject.transform.Find("Button").GetComponent<ButtonController>().ActiveFalse();

    }

    public void IsBom(int h, int w)
    {
        if (m_cell[h, w].CellState == CellState.Mine)
        {
            if (m_isGameOver) return;
            m_isGameOver = true;
            m_gameOverText.text = "GAME OVER";
            AllButtonActiveFalse();
            Debug.Log("GAME OVER");
        }
        else
        {
            if (!m_isGameOver && m_openCells == m_cell.Length - m_setBomCount)
            {
                m_isGameOver = true;
                m_gameOverText.text = "GAME CLEAR";
                Debug.Log($"Time : {m_time}");
            }
        }
    }

    public void FirstClick()
    {
        if (m_isFirstClick) return;

        CreateBom(m_setBomCount);
        foreach (var item in m_cell)
        {
            if (item.CellState != CellState.Mine)
            {
                item.CellState = (CellState)item.m_bomCount;
            }
        }
        m_isFirstClick = true;
    }

    void OnFlag()
    {
        //右クリックしたらフラグを立てる
        if (Input.GetMouseButtonDown(1))
        {
            List<RaycastResult> results = new List<RaycastResult>();
            // マウスポインタの位置にレイ飛ばし、ヒットしたものを保存
            m_pointer.position = Input.mousePosition;
            EventSystem.current.RaycastAll(m_pointer, results);
            // ヒットしたUIの名前
            foreach (RaycastResult target in results)
            {
                if (target.gameObject.name == "Button")
                {
                    ButtonController bc = target.gameObject.GetComponent<ButtonController>();
                    if (!bc.m_flag)
                    {
                        bc.OnFlag();
                    }
                    else
                    {
                        bc.UnFlag();
                        
                    }
                }
            }
        }
    }
}