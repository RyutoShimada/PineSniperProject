using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BingoMasu : MonoBehaviour
{
    [SerializeField] Text m_numberText = null;
    public bool m_isCollected { get; private set; }

    public int Number
    {
        get => m_number;

        set
        {
            if (value > 0 && value < 100)
            {
                m_number = value;
            }
            else if (value == 0)
            {
                m_number = 0;
                m_isCollected = true;
                UnActiveText();
            }
            else
            {
                Debug.LogError("’l‚ª³‚µ‚­‚ ‚è‚Ü‚¹‚ñB", this);
            }
        }
    }

    int m_number = 0;

    void Start()
    {
        m_numberText.text = m_number.ToString();
    }

    public void UpdateNumber()
    {
        m_numberText.text = m_number.ToString();
    }

    public void OnCollect()
    {
        m_isCollected = true;
        ChangeColor();
    }

    void ChangeColor()
    {
        m_numberText.color = Color.red;
    }

    void UnActiveText()
    {
        m_numberText.gameObject.SetActive(false);
    }
}
