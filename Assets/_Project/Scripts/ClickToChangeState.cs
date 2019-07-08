using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent( typeof( CircleCollider2D ))]
public class ClickToChangeState: BaseSelector {
    private int m_currentState = 0;
    private Animator m_animator;

    public int endState = 4;

    public bool IsEndState {
        get { return m_currentState == endState; }
    }

    public int CurrentState {
        get { return m_currentState; }
        set
        {
            value = Mathf.Clamp( value, 0, endState );

            Debug.Log( "State: " + value + ", Is End State: " + IsEndState );

            if( value != m_currentState ) {
                m_currentState = value;
                m_animator.SetInteger( "State", m_currentState );
            }
        }
    }

    void OnEnable() {
        // InputMng.onClickObj += OnClickObj;
    }

    void OnDisable() {
        // InputMng.onClickObj -= OnClickObj;
    }

    void Update() {
        var scn = GetComponentInParent<BaseScn>();

        if( scn == null || scn.PhaseState != BaseScn.PhaseStates.Playing )
            return;

        if( Input.GetMouseButtonDown( 0 ) ) 
            CurrentState++;
    }

    private void OnClickObj( BaseSelector selector ) {
        if( selector.gameObject == gameObject ) {
            CurrentState++;
        }
    }

    void Awake() {
        m_animator = GetComponent<Animator>();
    }
}
