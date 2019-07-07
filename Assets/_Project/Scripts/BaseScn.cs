using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseScn: MonoBehaviour {
    public const float WALL_MOVE_SPD = -0.5f;
    public const float WALL_TARGET_Y = -2f;
    public const float WALL_START_Y = 5f;

    public static Action<bool> onLevelComplete;
    public static Action<PhaseStates> onPhaseStateChanged;

    public enum PhaseStates {
        Null,
        Start,
        Playing,
        End
    }


    public static BaseScn Create( int phase, int level ) {
        string format = null;

        if( GameData.LevelSkip >= 0 ) {
            // Enter flag scene only once
            format = string.Format( "{0}_{1}_{2}", phase, level, GameData.LevelSkip );
            GameData.LevelSkip = -1;
        }
        else {
            format = string.Format( "{0}_{1}", phase, level );
        }

        Debug.Log( "Load Level: " + format );
        var rsc = Resources.Load<BaseScn>( format );
        Instantiate( rsc );

        return rsc;
    }

    private PhaseStates m_phaseState;

    [HideInInspector]
    public GameMng gameMng;

    public PhaseStates PhaseState {
        get { return m_phaseState; }
        set {
            if ( m_phaseState != value ) {
                OnChangePhaseStates( value );
            }
        }
    }

    void OnEnable() {
        InputMng.onSelectObj += OnSelectObj;
    }

    void OnDisable() {
        InputMng.onSelectObj -= OnSelectObj;
    }

    protected virtual void OnSelectObj( BaseSelector obj ) { }

    protected virtual void OnChangePhaseStates( PhaseStates phaseState ) {
        m_phaseState = phaseState;

        Debug.Log( "Phase State: " + m_phaseState );

        if( onPhaseStateChanged != null )
            onPhaseStateChanged( m_phaseState );

        if( m_phaseState == PhaseStates.Start ) {
            PhaseState = PhaseStates.Playing;
        }
    }

    public virtual void StartGame() {
        PhaseState = PhaseStates.Start;
    }
}
