using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

public class Level1_1 : BaseScn {
    public static event Action<bool> onLevelComplete;
    public float wallMoveSpd = -10f;
    public float targetY = -5f;
    public Transform wallTrans;
    public SelectableObj defaultPos;
    [HideInInspector]
    public int selectedId;
    public GameObject heartObj;
    public bool matchAnimState;
    public Transform selectionGroup;

    private bool m_isRestarting;
    private bool m_isGameOver;
    private Vector3 m_wallStartPos;
    private SelectableObj[] m_sObjs;

    void Start() {
        m_wallStartPos = wallTrans.position;

        // Initialize selection object id
        m_sObjs = selectionGroup.GetComponentsInChildren<SelectableObj>();
        for( int i = 0; i < m_sObjs.Length; i++ )
            m_sObjs[i].id = i;

        selectedId = defaultPos.id;

        heartObj.transform.position = defaultPos.transform.position;
    }

    void Update() {
        if( PhaseState != PhaseStates.Playing || m_isGameOver )
            return;

        if( !m_isRestarting ) {
            // Wall Moving
            wallTrans.Translate( Vector2.up * wallMoveSpd * Time.deltaTime );

            // Only check result before game over
            if( wallTrans.position.y <= targetY ) {
                CheckResult();
            }
        }
    }

    protected override void OnSelectObj(BaseSelector obj) {
        if( PhaseState != PhaseStates.Playing )
            return;

        if( obj is SelectableObj ) {
            var selector = obj as SelectableObj;

            selectedId = selector.id;

            heartObj.transform.position = selector.transform.position;
        }
    }


    protected virtual void CheckResult() {
        var answer = m_sObjs.FirstOrDefault( a => a.id == selectedId );

        bool result = CheckMatch( answer );

        if( onLevelComplete != null )
            onLevelComplete( result );

        if( !result ) {
            m_isRestarting = true;
            wallTrans.DOMoveY( m_wallStartPos.y, 1f )
                     .SetEase( Ease.OutQuad )
                     .OnComplete( () => m_isRestarting = false );
        } else {
            m_isGameOver = true;

            AudioMng.PlayOneShot( answer.selectionData.clip );

            StartCoroutine( EndGameDelay() );
        }
    }

    private bool CheckMatch( SelectableObj answer ) {
        if( answer.isNotAnswer )
            return false;

        if( matchAnimState ) {
            var clickToChangeState = heartObj.GetComponent<ClickToChangeState>();
            return clickToChangeState != null && clickToChangeState.IsEndState;
        }

        return true;
    }

    private IEnumerator EndGameDelay() {
        yield return new WaitForSeconds( 2 );
        PhaseState = PhaseStates.End;
    }
}