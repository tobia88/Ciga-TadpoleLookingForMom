using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Level1_1 : BaseScn {
    public float wallMoveSpd = -10f;
    public float targetY = -5f;
    public Transform wallTrans;
    public SelectableObj defaultPos;
    [HideInInspector]
    public int selectedId;
    public GameObject heartObj;
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
        if( PhaseState != PhaseStates.Playing )
            return;

        if( !m_isRestarting ) {
            // Wall Moving
            wallTrans.Translate( Vector2.up * wallMoveSpd * Time.deltaTime );

            if( m_isGameOver )
                return;

            // Only check result before game over
            if( wallTrans.position.y <= targetY ) {
                CheckResult();
            }
        }
    }

    protected override void OnSelectObj(BaseSelector obj) {
        if( obj is SelectableObj ) {
            var selector = obj as SelectableObj;

            selectedId = selector.id;

            heartObj.transform.position = selector.transform.position;
        }
    }


    private void CheckResult() {
        var answer = m_sObjs.FirstOrDefault( a => a.id == selectedId );

        if( answer.isNotAnswer ) {
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

    private IEnumerator EndGameDelay() {
        yield return new WaitForSeconds( 5 );
        PhaseState = PhaseStates.End;
    }
}
