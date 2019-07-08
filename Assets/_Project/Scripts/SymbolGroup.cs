using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolGroup : MonoBehaviour
{

    public SymbolPrefab symbolPrefab;
    public float length;


    private SymbolPrefab[] m_prefabs;

    public void Init( Sprite[] sprites ) {
        m_prefabs = new SymbolPrefab[sprites.Length];

        for( int i = 0; i < sprites.Length; i++ ) {
            m_prefabs[i] = Instantiate( symbolPrefab, transform );
            m_prefabs[i].Init( sprites[i] );
        }
    }


    void Update() {
        var from = Vector3.up * length * 0.5f;
        var to = Vector3.up * length * 0.5f;
        var spacing = length / ( m_prefabs.Length - 1 );

        for( int i = 0; i < m_prefabs.Length; i++ ) {
            m_prefabs[i].transform.localPosition = from + Vector3.down * i * spacing;
        }
    }


    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine( transform.position - Vector3.up * length / 2, transform.position + Vector3.up * length / 2 );
    }
}
