using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ThemeTransition: MonoBehaviour {

    private static ThemeTransition m_inst;

    public static event Action onFadeInFinish;
    public static event Action onFadeOutFinish;

    public Image uiImg;

    public Animator animator { get; private set; }

    void Awake() {
        m_inst = this;
        animator = GetComponent<Animator>();
    }

    public static void FadeOut( Sprite spr ) {
        m_inst.uiImg.sprite = spr;
        m_inst.animator.SetBool( "OnShow", true );
    }

    public static void FadeIn() {
        m_inst.animator.SetBool( "OnShow", false );
    }

    public void OnFadeInFinish() {
        if( onFadeInFinish != null )
            onFadeInFinish();
    }

    public void OnFadeOutFinish() {
        if( onFadeOutFinish != null )
            onFadeOutFinish();
    }
}
