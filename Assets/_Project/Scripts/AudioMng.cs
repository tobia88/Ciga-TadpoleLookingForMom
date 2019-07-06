using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[System.Serializable]
public class Sound {
    public AudioClip clip;
    [Range( 0, 1 )]
    public float volume = 1f;
    public float pitch = 1f;
    public bool randomPitch = false;
    public bool loop = false;
    public float minPitch = 0.995f;
    public float maxPitch = 1.05f;

    public float GetPitch() {
        return ( randomPitch ) ? Random.Range( minPitch, maxPitch ) : pitch;
    }

    public bool IsValid {
        get { return clip != null; }
    }
}

public class AudioMng: MonoBehaviour {
    private static AudioMng m_inst;

    public static Dictionary<AudioClip, AudioSource> sources = new Dictionary<AudioClip, AudioSource>();

    void Awake() {
        m_inst = this;
    }

    public static void Play( Sound _snd, bool _forceReplay = false ) {
        AudioSource src = GetSource( _snd );

        if( src.isPlaying && !_forceReplay )
            return;

        src.volume = _snd.volume;
        src.pitch = _snd.GetPitch();
        src.loop = _snd.loop;
        src.Play();
    }

    public static void PlayOneShot( Sound _snd ) {
        AudioSource src = GetSource( _snd );

        src.pitch = _snd.GetPitch();
        src.volume = _snd.volume;
        src.PlayOneShot( _snd.clip, src.volume );
    }

    public static void Fade( Sound _snd, float _volume, float _duration, TweenCallback _callback = null ) {
        AudioSource src = GetSource( _snd );
        src.DOFade( _volume, _duration ).OnComplete( _callback );
    }

    public static void Stop( Sound _snd ) {
        if( sources.ContainsKey( _snd.clip ) )
            sources[_snd.clip].Stop();
    }

    public static AudioSource Add( Sound _snd ) {
        GameObject go = new GameObject( _snd.clip.name );
        go.transform.SetParent( m_inst.transform );

        AudioSource retval = go.AddComponent<AudioSource>();
        sources.Add( _snd.clip, retval );
        retval.clip = _snd.clip;
        return retval;
    }

    public static AudioSource GetSource( Sound _snd ) {
        AudioSource src = null;
        if( !sources.ContainsKey( _snd.clip ) )
            src = Add( _snd );
        else
            src = sources[_snd.clip];

        return src;
    }

    public static void StopAll() {
        foreach( var snd in sources.Values ) {
            snd.Stop();
        }
    }
}