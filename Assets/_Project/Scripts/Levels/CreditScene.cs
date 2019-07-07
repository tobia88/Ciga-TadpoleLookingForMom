using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CreditScene: BaseScn {
    public PlayableDirector director;
    public SymbolGroup symbolGroup;

    void Start() {
        var sprites =  GameData.SymbolSprites;
        symbolGroup.Init( sprites.ToArray() );

        StartCoroutine( EndRoutine() );
    }

    IEnumerator EndRoutine() {
        director.Play();
        yield return new WaitUntil( () => ( director.time / director.duration) >= 0.85f );
        PhaseState = PhaseStates.End;
    }
}
