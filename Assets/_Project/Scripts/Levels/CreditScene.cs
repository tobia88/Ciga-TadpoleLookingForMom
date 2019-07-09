using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CreditScene: BaseScn {
    public PlayableDirector director;
    public SymbolGroup symbolGroup;

    [Header("Test")]
    public Sprite testSpr;
    public int testSprAmount;

    protected override void Start() {
        if( testSprAmount <= 0 ) {
            var sprites = GameData.SymbolSprites;
            symbolGroup.Init( sprites.ToArray() );
        }
        else {
            var testSprs = new Sprite[testSprAmount];

            for( int i = 0; i < testSprAmount; i++ ) {
                testSprs[i] = testSpr;
            }

            symbolGroup.Init( testSprs );
        }

        StartCoroutine( EndRoutine() );
    }

    IEnumerator EndRoutine() {
        director.Play();
        yield return new WaitUntil( () => ( director.time / director.duration ) >= 0.85f );
        PhaseState = PhaseStates.End;
    }
}
