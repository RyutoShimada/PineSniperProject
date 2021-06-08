using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{

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
}
