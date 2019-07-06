using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene: BaseScn {
    public void FadeOut() {
        PhaseState = PhaseStates.End;
        Debug.Log( "Fade Out" );
    }
}
