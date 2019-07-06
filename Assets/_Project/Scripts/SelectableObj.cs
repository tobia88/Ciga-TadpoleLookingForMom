﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent( typeof( CircleCollider2D ))]
[ExecuteInEditMode]
public class SelectableObj: MonoBehaviour{
    public int id = 0;

    void Update() {
        int layer = LayerMask.NameToLayer( "TouchObject" );
        if( gameObject.layer != layer )
            gameObject.layer = layer;
    }
}
