using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng: MonoBehaviour {
    public Sound bgm;
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

    public static GameMng Inst {
        get { return m_inst; }
    }

    void OnEnable() {
        BaseScn.onPhaseStateChanged += OnBaseScnPhaseStateChanged;

        ThemeTransition.onFadeInFinish += OnTransitFadeInFinish;
        ThemeTransition.onFadeOutFinish += OnTransitFadeOutFinish;
    }

    void OnDisable() {
        BaseScn.onPhaseStateChanged -= OnBaseScnPhaseStateChanged;

        ThemeTransition.onFadeInFinish -= OnTransitFadeInFinish;
        ThemeTransition.onFadeOutFinish -= OnTransitFadeOutFinish;
    }

    void Awake() {
        m_inst = this;
    }

    [HideInInspector]
    public PhaseData currentPhase;
    [HideInInspector]
    public BaseScn baseScn; 
    [HideInInspector]
    public int phase, level;

    void Start() {
        Screen.SetResolution( 1920, 1080, FullScreenMode.FullScreenWindow );

        phase = 0;
        level = 0;

        AudioMng.Play( bgm );
        InitLevel( true );
    }

    private void OnTransitFadeOutFinish() {
        baseScn.StartGame();
    }

    private void OnTransitFadeInFinish() {
        Destroy( baseScn.gameObject );

        Debug.Log( "Next Level" );
        level += GameData.LevelSkip + 1;

        // Always reset level skip
        GameData.LevelSkip = 0;

        var phaseChanged = false;

        if( level >= currentPhase.scenes.Length ) {
            phaseChanged = true;

            Debug.Log( "Enter next phase" );
            phase++;

            // Loop Phase
            phase %= PhaseData.All.Length;
            level = 0;
        }

        InitLevel( phaseChanged );
    }

    private void InitLevel( bool phaseChanged ) {
        InputMng.Reset();

        currentPhase = PhaseData.All[phase];
        baseScn = Instantiate( currentPhase.scenes[level] );

        // Show Theme Sprite if phase changed
        if( phaseChanged && currentPhase.themeSpr != null )
            ThemeTransition.FadeOutWithNewPhase( currentPhase.themeSpr );
        else
            ThemeTransition.FadeOut();
    }

    private void OnBaseScnPhaseStateChanged( BaseScn.PhaseStates phase ) {
        if( phase == BaseScn.PhaseStates.End ) {
            ThemeTransition.FadeIn();
        }
    }
}
