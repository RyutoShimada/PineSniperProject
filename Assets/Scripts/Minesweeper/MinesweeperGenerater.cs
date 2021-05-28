using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinesweeperGenerater : MonoBehaviour
{
    [SerializeField] GameObject m_tail = null;
    [SerializeField] GameObject m_cover = null;
    [SerializeField] GameObject m_bom = null;
    [SerializeField] int m_bomCount = 1;
    [SerializeField] int m_width = 3;
    [SerializeField] int m_height = 3;
    GameObject[] m_tileObj;
    GameObject[] m_coverObj;
    GameObject[] m_boms;

    void Start()
    {
        CreateTile();
        CreateBoms();
        RandomActive();
        CreateCover(-1);

        for (int i = 0; i < m_tileObj.Length; i++)
        {
            SearchBoms(i);
        }
    }

    /// <summary>
    /// タイルの生成
    /// </summary>
    void CreateTile()
    {
        m_tileObj = new GameObject[m_width * m_height];
        for (int i = 0; i < m_tileObj.Length; i++)
        {
            m_tileObj[i] = Instantiate(m_tail);
            m_tileObj[i].transform.position = new Vector3((transform.position.x + i % m_width), (transform.position.y + i / m_height), 0);
        }
    }

    /// <summary>
    /// カバーの生成
    /// </summary>
    /// <param name="z">奥行</param>
    void CreateCover(float z)
    {
        m_coverObj = new GameObject[m_width * m_height];
        for (int i = 0; i < m_coverObj.Length; i++)
        {
            m_coverObj[i] = Instantiate(m_cover);
            m_coverObj[i].transform.position = new Vector3((transform.position.x + i % m_width), (transform.position.y + i / m_height), z);
        }
    }

    /// <summary>
    /// 爆弾の生成
    /// </summary>
    void CreateBoms()
    {
        m_boms = new GameObject[m_width * m_height];

        for (int i = 0; i < m_boms.Length; i++)
        {
            Vector3 v3 = new Vector3(m_tileObj[i].transform.position.x, m_tileObj[i].transform.position.y, m_tileObj[i].transform.position.z - 1);
            m_boms[i] = Instantiate(m_bom, v3, m_tileObj[i].transform.rotation, this.transform);
            m_boms[i].SetActive(false);
        }
    }

    void SearchBoms(int index)
    {
        if (index == 0)//右、右上、上
        {
            SetNumber(index, 1, m_width, m_width + 1);
        }
        else if (index == m_width - 1)//左、左上、上
        {
            SetNumber(index, m_width - 2, (m_width * 2) - 2, (m_width * 2) - 1);
        }
        else if (index == m_tileObj.Length - m_width)//右、右下、下
        {
            //SetNumber(index, 1, m_width, m_width + 1);
        }
        else if (index == m_tileObj.Length - 1)//左、左下、下
        {
            //SetNumber(index, 1, m_width, m_width + 1);
        }
    }

    void SetNumber(int index, int bomIndexZero, int bomIndex1st, int bomIndex2nd)
    {
        int bomCount = 0;
        int[] bomArrayIndex = { bomIndexZero, bomIndex1st, bomIndex2nd };
        for (int i = 0; i < 3; i++)
        {
            if (m_boms[bomArrayIndex[i]].gameObject.activeSelf)
            {
                bomCount++;
            }
        }
        m_tileObj[index].GetComponent<PanelNumberController>().SetNumber(bomCount);
        Debug.Log(bomCount);
    }

    /// <summary>
    /// ランダムに爆弾をアクティブにする(全てアクティブの状態で実行しようとすると無限ループに入る)
    /// </summary>
    public void RandomActive()
    {
        for (int i = 0; i < m_bomCount; i++)
        {
            int random = Random.Range(0, m_boms.Length);

            if (!m_boms[random].activeSelf)
            {
                m_boms[random].SetActive(true);
            }
            else
            {
                while (true)
                {
                    random = Random.Range(0, m_boms.Length);

                    if (!m_boms[random].activeSelf)
                    {
                        m_boms[random].SetActive(true);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 爆弾をすべて非アクティブ化する
    /// </summary>
    public void BomsClear()
    {
        foreach (var item in m_boms)
        {
            item.gameObject.SetActive(false);
        }
    }
}
