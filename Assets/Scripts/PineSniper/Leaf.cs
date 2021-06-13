using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    [SerializeField] GameObject _breakPrticle = null;
    [SerializeField] float _instancePrticleSetPosY = 0;
    private Pine _pine;

    public Pine Pine
    {
        get => _pine;
        set
        {
            _pine = value;
        }
    }

    void SetActivFalse()
    {
        this.gameObject.SetActive(false);
    }

    public void InstancePrticle()
    {
        Vector3 v3 = transform.position;
        v3.y = _instancePrticleSetPosY;
        transform.position = v3;
        Instantiate(_breakPrticle, transform.position, transform.rotation);
        //Invoke("SetActivFalse", 0.1f);
        SetActivFalse();
    }
}
