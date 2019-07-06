using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng: MonoBehaviour {
    private static GameMng m_inst;

    // public static GameMng Inst {
    //     get {
    //         if( m_inst == null ) {
    //             var gameMng = FindObjectOfType<GameMng>();

    //             if( gameMng != null )
    //                 Destroy( gameMng.gameObject );

    //             var rsc = Resources.Load<GameMng>( "GameMng" );
    //             m_inst = Instantiate( rsc );
    //             DontDestroyOnLoad( m_inst.gameObject );
    //         }

    //         return m_inst;
    //     }
    // }

    void OnEnable() {
        BaseScn.onPhaseStateChanged += OnBaseScnPhaseStateChanged;
        ThemeTransition.onFadeOutFinish += OnTransitFadeOutFinish;
    }

    void OnDisable() {
        BaseScn.onPhaseStateChanged -= OnBaseScnPhaseStateChanged;
        ThemeTransition.onFadeInFinish -= OnTransitFadeInFinish;
    }

    void Awake() {
        m_inst = this;
    }

    public PhaseData currentPhase;
    [HideInInspector]
    public BaseScn baseScn; 

    void Start() {
        currentPhase = PhaseData.All[0];
        baseScn = Instantiate( currentPhase.scenes[0] );
        ThemeTransition.FadeOut( currentPhase.themeSpr );
    }

    private void OnTransitFadeOutFinish() {
        baseScn.StartGame();
    }

    private void OnTransitFadeInFinish() {
        Destroy( baseScn.gameObject );
        Debug.Log( "Next Level" );
    }

    private void OnBaseScnPhaseStateChanged( BaseScn.PhaseStates phase ) {
        if( phase == BaseScn.PhaseStates.End ) {
            ThemeTransition.FadeIn();
        }
    }
}
