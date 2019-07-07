using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent( typeof( CircleCollider2D ))]
public class ClickToChangeSprite: BaseSelector {
    void OnEnable() {
        InputMng.onClickObj += OnClickObj;
    }

    void OnDisable() {
        InputMng.onClickObj -= OnClickObj;
    }

    private void OnClickObj( BaseSelector selector ) {
        if( selector.gameObject == gameObject ) {
            GetComponentInParent<ChatScene>().ChatDialogState ++;
        }
    }
}
