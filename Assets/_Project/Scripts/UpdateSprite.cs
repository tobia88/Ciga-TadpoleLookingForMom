using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    public SpriteRenderer spriteRender;
    public Sprite targetSprite;

    void OnEnable() {
        Level1_1.onLevelComplete += OnLevelComplete;
    }

    void OnDisable() {
        Level1_1.onLevelComplete -= OnLevelComplete;
    }

    private void OnLevelComplete( bool result ) {
        if( result )
            spriteRender.sprite = targetSprite;
    }
}
