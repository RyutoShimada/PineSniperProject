using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BingoManager : MonoBehaviour
{
    [SerializeField] BingoGenerator m_generator = null;
    BingoMasu[,] m_masus;
    int[] m_numbers = new int[100];
    int m_count = 0;

    void Start()
    {
        m_generator.InstancePrefab();
        m_masus = new BingoMasu[m_generator.m_generateObjects.GetLength(0), m_generator.m_generateObjects.GetLength(1)];

        for (int h = 0; h < m_masus.GetLength(0); h++)
        {
            for (int w = 0; w < m_masus.GetLength(1); w++)
            {
                m_masus[h, w] = m_generator.m_generateObjects[h, w].GetComponent<BingoMasu>();
            }
        }

        for (int i = 0; i < m_numbers.Length; i++)
        {
            m_numbers[i] = i;
        }
    }

    public void GetNumber()
    {
        int n = CallRandomNumber();
        Debug.Log(n);
        Searching(m_masus, n);
    }

    int CallRandomNumber()
    {
        if (m_count > m_numbers.Length) return 0;

        int randomNumber;

        while (true)
        {
            randomNumber = Random.Range(1, 100);

            if (m_numbers[randomNumber] != 0)
            {
                m_numbers[randomNumber] = 0;
                break;
            }
            else
            {
                for (int i = 0; i < m_numbers.Length; i++)
                {
                    if (m_numbers[i] != 0)
                    {
                        randomNumber = m_numbers[i];
                        m_numbers[i] = 0;
                        return randomNumber;
                    } 
                }
            }
        }

        return randomNumber;
    }

    void Searching(BingoMasu[,] masus, int number)
    {
        foreach (var item in masus)
        {
            if (item.Number == number)
            {
                item.OnCollect();
                break;
            }
        }
    }
}
