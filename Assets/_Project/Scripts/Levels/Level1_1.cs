using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using Thinksquirrel.CShake;
using System;

public class Level1_1 : BaseScn {
    public Transform wallTrans;
    public SelectableObj defaultPos;
    [HideInInspector]
    public int selectedId;
    public GameObject heartObj;
    public bool matchAnimState;
    public Transform selectionGroup;

    private bool m_isRestarting;
    private bool m_isGameOver;
    private SelectableObj m_answer;
    private Vector3 m_wallStartPos;
    private SelectableObj[] m_sObjs;

    protected override void Start() {
        base.Start();

        m_wallStartPos = wallTrans.position = Vector3.up * WALL_START_Y;

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
            wallTrans.Translate( Vector2.up * WALL_MOVE_SPD * Time.deltaTime );

            // Only check result before game over
            if( wallTrans.position.y <= WALL_TARGET_Y ) {
                CheckResult();
            }
        }
    }

    protected override void OnSelectObj( BaseSelector obj ) {
        if( PhaseState != PhaseStates.Playing )
            return;

        if( obj is SelectableObj ) {
            var selector = obj as SelectableObj;

            selectedId = selector.id;

            heartObj.transform.position = selector.transform.position;
        }
    }


    protected virtual void CheckResult() {
        m_answer = m_sObjs.FirstOrDefault( a => a.id == selectedId );

        bool result = CheckMatch( m_answer );

        if( onLevelComplete != null )
            onLevelComplete( result );

        AudioMng.PlayOneShot( m_answer.selectionData.clip );

        if( !result ) {
            m_isRestarting = true;
            wallTrans.DOMoveY( m_wallStartPos.y, 1f )
                     .SetEase( Ease.OutQuad )
                     .OnComplete( () => m_isRestarting = false );

            CameraShake.ShakeAll();
        } else {
            GameOver();
        }
    }

    protected override void GameOver() {
        base.GameOver();

        m_isGameOver = true;

        GameData.LevelSkip = m_answer.levelSkip;

        GameData.SymbolSprites.Add( m_answer.selectionData.symbolSpr );

        var anim = heartObj.GetComponentInChildren<Animator>();

        if( anim != null )
            anim.enabled = true;

        StartCoroutine( EndGameDelay() );
    }

    private bool CheckMatch( SelectableObj answer ) {
        if( answer.isNotAnswer )
            return false;

        if( matchAnimState ) {
            var clickToChangeState = heartObj.GetComponentInChildren<ClickToChangeState>();

            if( clickToChangeState == null || !clickToChangeState.IsEndState )
                return false;
        }

        return true;
    }

    private IEnumerator EndGameDelay() {
        yield return new WaitForSeconds( 2 );
        PhaseState = PhaseStates.End;
    }
}