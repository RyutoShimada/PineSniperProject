using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] GameObject _explosionParticle = null;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_explosionParticle, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
