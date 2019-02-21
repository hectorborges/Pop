﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class Health : MonoBehaviour
{
    public static bool isDead;
    public static bool cantDied;
    public Text reviveTime;
    Animator anim;
    AudioSource source;

    void Start()
    {
        isDead = false;
        cantDied = false;
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    public IEnumerator Died()
    {
        if (cantDied) yield break;
        isDead = true;
        CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f,new Vector3(1,1,0), new Vector3(0, 0, 1));
        if (Application.isMobilePlatform)
            Handheld.Vibrate();

        Movement.canMove = false;
        anim.SetBool("Died", true);

        yield return new WaitForSeconds(1.5f);
        transform.position = Vector3.zero;
        reviveTime.gameObject.SetActive(true);

        for(int i = 3; i > 0; i--)
        {
            reviveTime.text = "Respawning in " + i + "...";
            yield return new WaitForSeconds(1);

            if(i == 2)
                anim.SetBool("Died", false);
        }

        reviveTime.gameObject.SetActive(false);
        Movement.canMove = true;
        isDead = false;
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}