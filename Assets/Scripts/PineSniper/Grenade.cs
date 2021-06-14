using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] GameObject _explosionParticle = null;
    [SerializeField] AudioClip _explosionSE = null;

    private void OnTriggerEnter(Collider other)
    {
        Instantiate(_explosionParticle, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(_explosionSE, transform.position);
        Destroy(this.gameObject);
    }
}
