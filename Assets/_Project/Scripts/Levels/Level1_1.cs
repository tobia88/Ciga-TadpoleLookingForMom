using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class Level1_1 : BaseScn {
    public float wallMoveSpd = -10f;
    public float targetY = -5f;
    public Transform wallTrans;
    [HideInInspector]
    public int selectedId;
    public SelectionData[] answers;
    public GameObject heartObj;

    private bool m_isRestarting;
    private bool m_isGameOver;
    private Vector3 m_wallStartPos;

    void Start() {
        m_wallStartPos = wallTrans.position;
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

    protected override void OnSelectObj(SelectableObj obj) {
        selectedId = obj.id;

        heartObj.transform.position = obj.transform.position;
    }


    private void CheckResult() {
        var answer = answers.FirstOrDefault( a => a.selectionId == selectedId );


        if( answer == null ) {
            m_isRestarting = true;
            wallTrans.DOMoveY( m_wallStartPos.y, 1f )
                     .SetEase( Ease.OutQuad )
                     .OnComplete( () => m_isRestarting = false );
        }
        else {
            m_isGameOver = true;
            AudioMng.PlayOneShot( answer.clip );
            StartCoroutine( EndGameDelay() );
        }
    }

    private IEnumerator EndGameDelay() {
        yield return new WaitForSeconds( 2 );
        PhaseState = PhaseStates.End;
    }
}
