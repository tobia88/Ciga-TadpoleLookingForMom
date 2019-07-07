using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputMng: MonoBehaviour {
    public static event Action<BaseSelector> onSelectObj;
    public static event Action<BaseSelector> onClickObj;
    public int frameIntervalForClick = 10;
    private bool m_mp;
    private int clickTick;
    private List<Collider2D> m_lastSelectedObjs = new List<Collider2D>();

    void Update() {
        clickTick--;

        if( Camera.main == null )
            return;

        // Mouse Movement
        if( Input.GetMouseButtonDown( 0 ) ) {
            m_mp = true;
            clickTick = frameIntervalForClick;
        }
        else if (Input.GetMouseButtonUp( 0 ) ){
            m_mp = false;
            // Check if click
            if( clickTick > 0 && m_lastSelectedObjs != null ) {
                if( onClickObj != null ) {
                    foreach( var c in m_lastSelectedObjs ) {
                        onClickObj( c.GetComponent<BaseSelector>() );
                    }
                }
            }

            m_lastSelectedObjs.Clear();
            return;
       }

        if( m_mp ) {
            var wp = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            var cols = Physics2D.OverlapPointAll( wp, 1 << LayerMask.NameToLayer( "TouchObject" ));

            if( cols != null ) {
                foreach( var col in cols ) {
                    Debug.Log( col.name );
                    if( m_lastSelectedObjs.Contains( col ) ) {
                        continue;
                    }

                    m_lastSelectedObjs.Add( col );

                    var selectableObj = col.GetComponent<BaseSelector>();

                    if( selectableObj != null ) {
                        if( onSelectObj != null )
                            onSelectObj( selectableObj );
                    }

                    Debug.Log( "Touch Something: " + col.gameObject.name );
                }
            } else {
                m_lastSelectedObjs.Clear();
            }
        }

        // Debug Hotkeys
        // Speedup
        Time.timeScale = ( Input.GetKey( KeyCode.LeftAlt ) && Input.GetKey( KeyCode.Alpha1 ) ) ? 4 : 1;

        // Skip current level
        if( Input.GetKey( KeyCode.LeftAlt ) && Input.GetKeyDown( KeyCode.Alpha2 ) ) {
            GameMng.Inst.baseScn.PhaseState = BaseScn.PhaseStates.End;
        }

        if( Input.GetKey( KeyCode.LeftAlt ) && Input.GetKeyDown( KeyCode.R ) ) {
            UnityEngine.SceneManagement.SceneManager.LoadScene( 0 );
        }
    }
}
