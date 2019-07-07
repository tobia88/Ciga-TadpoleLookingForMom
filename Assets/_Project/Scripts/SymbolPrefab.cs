using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolPrefab: MonoBehaviour {
    public SpriteRenderer mainSpr;

    public void Init( Sprite spr ) {
        mainSpr.sprite = spr;
    }
}
