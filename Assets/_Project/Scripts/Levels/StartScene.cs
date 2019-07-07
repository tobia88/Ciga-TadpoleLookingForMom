using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartScene: BaseScn {
    public PlayableDirector startDirector;
    public PlayableDirector waitInputDirector;
    public PlayableDirector endInputDirector;

    public BaseSelector startObj;

    private bool m_input;

    void Start() {
        GameData.LevelSkip = 0;
        GameData.SymbolSprites.Clear();

        StartCoroutine( TestRoutine() );
    }

    protected override void OnSelectObj(BaseSelector obj) {
        m_input = obj == startObj;
    }

    IEnumerator TestRoutine() {
        startDirector.Play();
        yield return new WaitUntil( () => startDirector.time >= startDirector.duration );

        waitInputDirector.Play();
        yield return new WaitUntil( () => m_input );

        endInputDirector.Play();
        yield return new WaitUntil( () => endInputDirector.time >= endInputDirector.duration );

        PhaseState = PhaseStates.End;
    }
}
