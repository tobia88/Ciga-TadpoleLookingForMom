﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

[Serializable]
public class ChatDialogData {
    public int wallStateEffect;
    public Sprite[] sprites;
    public Sound feedbackClip;

    private int m_last;

    public Sprite GetRandomSpr() {
        var randIndex = Random.Range( 0, sprites.Length );

        if( randIndex == m_last ) {
            randIndex++;
            randIndex %= sprites.Length;
        }
        
        m_last = randIndex;

        return sprites[m_last];
    }
}

public class ChatScene : BaseScn {
    private SelectableObj m_answer;

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
    public Sprite successSymbol;
    public Sprite breakupSymbol;

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
    private bool m_isWin;
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
        m_answer = m_sObjs.FirstOrDefault( a => a.id == selectedId );

        m_isWin = CheckMatch( m_answer );

        WallState += dialogDatas[ChatDialogState].wallStateEffect;

        chatDialogSpr.sprite = dialogDatas[m_chatDialogState].GetRandomSpr();

        AudioMng.PlayOneShot( dialogDatas[ChatDialogState].feedbackClip );

        if( onLevelComplete != null )
            onLevelComplete( m_isWin );

        // End game if no more try
        m_tryLeft--;

        if( m_tryLeft <= 0 ) {
            GameOver();
            return;
        }

        if( !m_isWin ) {
            m_isRestarting = true;
            wallTrans.DOMoveY( m_wallStartPos.y, 1f )
                     .SetEase( Ease.OutQuad )
                     .OnComplete( () => m_isRestarting = false );
        } else {
            GameOver();
        }
    }

    protected override void GameOver() {
        m_isGameOver = true;

        AudioMng.PlayOneShot( m_answer.selectionData.clip );

        GameData.LevelSkip = m_answer.levelSkip;

        //FIXME: I need the true symbol!
        GameData.SymbolSprites.Add( (m_isWin) ? successSymbol : breakupSymbol );
        GameData.IsBreakup = !m_isWin;

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