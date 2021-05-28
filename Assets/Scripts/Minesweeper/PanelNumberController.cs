using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelNumberController : MonoBehaviour
{
    /// <summary>ラベルとなる UI オブジェクト</summary>
    [SerializeField] UnityEngine.UI.Text m_label = null;

    /// <summary>
    /// 引数に指定された番号をラベルに表示する。
    /// </summary>
    /// <param name="number">番号</param>
    public void SetNumber(int number)
    {
        if (number != 0)
        {
            m_label.text = number.ToString();
        }
    }
}
