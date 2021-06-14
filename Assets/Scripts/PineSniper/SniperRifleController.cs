using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleController : MonoBehaviour
{
    [SerializeField] AudioClip _aimSE = null;
    [SerializeField] AudioClip _shotSE = null;
    AudioSource _audio = null;
    Animator _anim = null;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// スコープを覗く
    /// アニメーションイベントから呼ばれる
    /// </summary>
    public void DoAim()
    {
        FindObjectOfType<GameManager>().OnScope();
    }

    public void Idol()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            _anim.SetTrigger("AimTrigger");
            _audio.PlayOneShot(_aimSE);
        }
    }

    public void ShotSE()
    {
        _audio.PlayOneShot(_shotSE);
    }
}
