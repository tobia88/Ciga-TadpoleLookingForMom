using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChangeAnimState: MonoBehaviour {
    private Animator m_anim;
    void Awake() {
        m_anim = GetComponent<Animator>();
    }
    void OnEnable() {
        Level1_1.onLevelComplete += OnLevelComplete;
    }

    void OnDisable() {
        Level1_1.onLevelComplete -= OnLevelComplete;
    }

    private void OnLevelComplete( bool result ) {
        m_anim.SetBool( "Result", result );
    }
}
