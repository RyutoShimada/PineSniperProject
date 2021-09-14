using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoGenerator : MonoBehaviour
{
    [Tooltip("生成するプレファブ")]
    [SerializeField] GameObject m_prefab = null;
    [Tooltip("親オブジェクト")]
    [SerializeField] RectTransform m_pearent = null;
    [Tooltip("生成する数(縦)")]
    [SerializeField] int m_height = 0;
    [Tooltip("生成する数(縦)")]
    [SerializeField] int m_width = 0;

    public GameObject[,] m_generateObjects { get; private set; }

    public int[] m_numbers = new int[100];

    public void InstancePrefab()
    {
        m_generateObjects = new GameObject[m_height, m_width];
        Vector3 pos;
        RectTransform dis = m_prefab.GetComponent<RectTransform>();

        for (int h = 0; h < m_height; h++)
        {
            for (int w = 0; w < m_width; w++)
            {
                pos = new Vector3(w * dis.sizeDelta.x, h * dis.sizeDelta.y, 0);
                m_generateObjects[h, w] = Instantiate(m_prefab, pos, Quaternion.identity, m_pearent);
                m_generateObjects[h, w].GetComponent<BingoMasu>().Number = GetRandomNumber();
            }
        }

        m_generateObjects[m_height / 2, m_width / 2].GetComponent<BingoMasu>().Number = 0;

        m_pearent.anchoredPosition = new Vector3(440, 160, 0);
    }

    /// <summary>
    /// 0～99のランダムな整数を返す
    /// </summary>
    /// <returns>ランダムな整数</returns>
    int GetRandomNumber()
    {
        int randomNumber;

        while (true)
        {
            randomNumber = Random.Range(1, 100);

            if (m_numbers[randomNumber] != 0)
            {
                break;
            }
            else
            {
                m_numbers[randomNumber] = randomNumber;
            }
        }

        return randomNumber;
    }

    public void ResetAndGenerate()
    {
        m_numbers = new int[100];

        for (int h = 0; h < m_height; h++)
        {
            for (int w = 0; w < m_width; w++)
            {
                ResetNumber(m_generateObjects[h, w].GetComponent<BingoMasu>(), GetRandomNumber());
            }
        }

        ResetNumber(m_generateObjects[m_height / 2, m_width / 2].GetComponent<BingoMasu>(), 0);
    }

    void ResetNumber(BingoMasu bingo, int number)
    {
        bingo.Number = number;
        bingo.UpdateNumber();
    }
}
