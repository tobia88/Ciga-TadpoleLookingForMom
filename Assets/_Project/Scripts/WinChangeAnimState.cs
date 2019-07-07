using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChangeAnimState: MonoBehaviour {
    private Animator m_anim;
    void Awake() {
        m_anim = GetComponent<Animator>();
    }
    void OnEnable() {
        BaseScn.onLevelComplete += OnLevelComplete;
    }

    void OnDisable() {
        BaseScn.onLevelComplete -= OnLevelComplete;
    }

    private void OnLevelComplete( bool result ) {
        m_anim.SetBool( "Result", result );
    }
}
