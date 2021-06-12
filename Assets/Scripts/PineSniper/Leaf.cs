using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public Pine _pine;
    
    public void SetActivFalse()
    {
        this.gameObject.SetActive(false);
    }
}
