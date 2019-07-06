using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent( typeof( CircleCollider2D ))]
public class ClickToChangeState: BaseSelector {
    private int m_currentState = 0;
    private Animator m_animator;

    public int endState = 4;

    public int CurrentState {
        get { return m_currentState; }
        set
        {
            value = Mathf.Clamp( value, 0, endState );

            Debug.Log( "State: " + value );

            if( value != m_currentState ) {
                m_currentState = value;
                m_animator.SetInteger( "State", m_currentState );
            }
        }
    }

    void OnEnable() {
        InputMng.onClickObj += OnClickObj;
    }

    void OnDisable() {
        InputMng.onClickObj -= OnClickObj;
    }

    private void OnClickObj( BaseSelector selector ) {
        Debug.Log( "On Click" );
        if( selector.gameObject == gameObject ) {
            CurrentState++;
        }
    }

    void Awake() {
        m_animator = GetComponent<Animator>();
    }
}
