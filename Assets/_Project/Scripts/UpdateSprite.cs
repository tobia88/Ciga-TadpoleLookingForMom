using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Sprite targetSprite;

    void OnEnable() {
        BaseScn.onLevelComplete += OnLevelComplete;
    }

    void OnDisable() {
        BaseScn.onLevelComplete -= OnLevelComplete;
    }

    private void OnLevelComplete( bool result ) {
        if( result )
            spriteRender.sprite = targetSprite;
    }
}
