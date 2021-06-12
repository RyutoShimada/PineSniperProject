using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifleController : MonoBehaviour
{
    Animator _anim = null;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
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
        }
    }
}
