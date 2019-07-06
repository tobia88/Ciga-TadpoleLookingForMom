using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputMng: MonoBehaviour {
    public static event Action<SelectableObj> onSelectObj;
    private bool m_mp;
    private Vector3 m_startPos, m_lastPos, m_pos, m_delta;
    private GameObject m_lastSelectedObj;

    void Update() {
        // Mouse Movement
        if( Input.GetMouseButtonDown( 0 ) ) {
            m_mp = true;
            m_startPos = m_pos = m_lastPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp( 0 ) ){
            m_mp = false;
            m_lastSelectedObj = null;
        }

        if( m_mp ) {
            m_lastPos = m_pos;
            m_pos = Input.mousePosition;
            m_delta = m_pos - m_lastPos;

            var wp = Camera.main.ScreenToWorldPoint( m_pos );
            var col = Physics2D.OverlapPoint( wp, 1 << LayerMask.NameToLayer( "TouchObject" ));

            if( col != null ) {

                if( m_lastSelectedObj == col.gameObject )
                    return;

                m_lastSelectedObj = col.gameObject;

                if( onSelectObj != null )
                    onSelectObj( m_lastSelectedObj.GetComponent<SelectableObj>() );

                Debug.Log( "Touch Something: " + col.gameObject.name );
            }
            else {
                m_lastSelectedObj = null;
            }
        }
        
        // Debug Hotkeys
        Time.timeScale = ( Input.GetKey( KeyCode.LeftAlt ) && Input.GetKey( KeyCode.Alpha1 ) ) ? 2 : 1;
    }
}
