using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSelector: MonoBehaviour {

    void Update() {
        int layer = LayerMask.NameToLayer( "TouchObject" );
        if( gameObject.layer != layer )
            gameObject.layer = layer;
    }
}
