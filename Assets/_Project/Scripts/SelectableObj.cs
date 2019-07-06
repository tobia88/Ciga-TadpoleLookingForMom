using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent( typeof( CircleCollider2D ))]
[ExecuteInEditMode]
public class SelectableObj: BaseSelector {
    [HideInInspector]
    public int id = 0;
    public SelectionData selectionData;
    public bool isNotAnswer;
}
