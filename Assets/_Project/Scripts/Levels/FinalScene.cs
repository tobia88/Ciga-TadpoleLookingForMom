using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinalScene: BaseScn {
    public Sprite[] cubeSprites;
    public SpriteRenderer cubeRenderer;
    public Sound successSnd;
    public PlayableDirector director;
    public GameObject cubeFront;

    private int m_index = -1;
    private bool m_isFinished;

    public int Index {
        get { return m_index; }
        set {
            value = Mathf.Clamp( value, 0, cubeSprites.Length - 1);

            if( m_index != value ) {
                m_index = value;
                cubeRenderer.sprite = cubeSprites[m_index];

                if( m_index == cubeSprites.Length - 1) {
                    Finish();
                }
            }
        }
    }


    protected override void Start() {
        base.Start();
        cubeFront.SetActive( false );

        if( GameData.IsBreakup )
        {
            PhaseState = PhaseStates.End;
        }
        else {
            PhaseState = PhaseStates.Start;
            Index = 0;
            Debug.Log( "Final Stage Start`" );
        }
    }

    public void Update() {
        if( PhaseState != PhaseStates.Playing )
            return;

        if( !m_isFinished ) {
            if( Input.GetMouseButtonDown( 0 ) ) {
                Index++;
                AudioMng.PlayOneShot( successSnd );
            }
        }

    }

    public void Finish() {
        m_isFinished = true;
        cubeFront.SetActive( true );
        director.Play();
    }
}
