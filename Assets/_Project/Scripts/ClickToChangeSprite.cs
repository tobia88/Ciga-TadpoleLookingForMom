using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent( typeof( CircleCollider2D ))]
public class ClickToChangeSprite: BaseSelector {
    void OnEnable() {
        // InputMng.onClickObj += OnClickObj;
    }

    void OnDisable() {
        // InputMng.onClickObj -= OnClickObj;
    }

    void Update() {
        var scn = GetComponentInParent<BaseScn>();

        if( scn == null || scn.PhaseState != BaseScn.PhaseStates.Playing )
            return;

        if( Input.GetMouseButtonDown( 0 ) ) 
            GetComponentInParent<ChatScene>().ChatDialogState ++;
    }

    private void OnClickObj( BaseSelector selector ) {
        if( selector.gameObject == gameObject ) {
            GetComponentInParent<ChatScene>().ChatDialogState ++;
        }
    }
}
