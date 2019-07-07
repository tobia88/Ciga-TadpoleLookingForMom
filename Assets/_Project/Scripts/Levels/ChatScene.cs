
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

[Serializable]
public struct ChatDialogData {
    public int wallStateEffect;
    public Sprite[] sprites;

    public Sprite GetRandomSpr() {
        return sprites[Random.Range( 0, sprites.Length )];
    }
}

public class ChatScene : BaseScn {
    public SelectableObj defaultPos;
    [HideInInspector]
    public int selectedId;
    public Transform selectionGroup;
    public int tryAmount = 10;

    [Header( "Chat Dialog" )]
    public int chatStartState = 1;
    public SpriteRenderer chatDialogSpr;
    public ChatDialogData[] dialogDatas;

    [Header( "Wall" )]
    public int wallStartState = 2;
    public SpriteRenderer wallSpr;
    public Transform wallTrans;
    public Sprite[] wallStateSprites;

    public int WallState {
        get { return m_wallState; }
        set {
            value = Mathf.Clamp( value, 0, wallStateSprites.Length - 1 );
            if( m_wallState != value ) {
                m_wallState = value;
                wallSpr.sprite = wallStateSprites[m_wallState];
            }
        }
    }

    public int ChatDialogState {
        get { return m_chatDialogState; }
        set {
            value %= dialogDatas.Length;

            if( m_chatDialogState != value ) {
                m_chatDialogState = value;

                chatDialogSpr.sprite = dialogDatas[m_chatDialogState].GetRandomSpr();
            }
        }
    }
    
    private int m_chatDialogState = -1;
    private int m_tryLeft;
    private int m_wallState = -1;

    private bool m_isRestarting;
    private bool m_isGameOver;
    private Vector3 m_wallStartPos;
    private SelectableObj[] m_sObjs;

    void Start() {
        m_wallStartPos = wallTrans.position = Vector3.up * WALL_START_Y;

        // Initialize selection object id
        m_sObjs = selectionGroup.GetComponentsInChildren<SelectableObj>();
        for( int i = 0; i < m_sObjs.Length; i++ )
            m_sObjs[i].id = i;

        selectedId = defaultPos.id;

        chatDialogSpr.transform.position = defaultPos.transform.position;

        m_tryLeft = tryAmount;

        ChatDialogState = chatStartState;
        WallState = wallStartState;
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

    protected override void OnSelectObj(BaseSelector obj) {
        if( PhaseState != PhaseStates.Playing )
            return;

        if( obj is SelectableObj ) {
            var selector = obj as SelectableObj;

            selectedId = selector.id;

            chatDialogSpr.transform.position = selector.transform.position;
        }
    }


    protected virtual void CheckResult() {
        var answer = m_sObjs.FirstOrDefault( a => a.id == selectedId );

        bool result = CheckMatch( answer );

        WallState += dialogDatas[ChatDialogState].wallStateEffect;

        if( onLevelComplete != null )
            onLevelComplete( result );

        // End game if no more try
        m_tryLeft--;

        if( m_tryLeft <= 0 ) {
            GameOver( answer );
            return;
        }

        if( !result ) {
            m_isRestarting = true;
            wallTrans.DOMoveY( m_wallStartPos.y, 1f )
                     .SetEase( Ease.OutQuad )
                     .OnComplete( () => m_isRestarting = false );
        } else {
            GameOver( answer );
        }
    }

    private void GameOver( SelectableObj answer ) {
        m_isGameOver = true;

        AudioMng.PlayOneShot( answer.selectionData.clip );

        GameData.LevelSkip = answer.levelSkip;

        StartCoroutine( EndGameDelay() );
    }

    private bool CheckMatch( SelectableObj answer ) {
        return WallState == wallStateSprites.Length - 1;
    }

    private IEnumerator EndGameDelay() {
        yield return new WaitForSeconds( 2 );
        PhaseState = PhaseStates.End;
    }
}