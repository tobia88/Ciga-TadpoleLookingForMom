using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Datas/Phase Data" )]
public class PhaseData: ScriptableObject {
    public int phaseId;
    public Sprite themeSpr;
    public BaseScn[] scenes {
        get { return Resources.LoadAll<BaseScn>( "Levels/" + phaseId ); }
    }

    private static PhaseData[] m_all;
    public static PhaseData[] All {
        get { return Resources.LoadAll<PhaseData>( "Datas" ); }
    }
}