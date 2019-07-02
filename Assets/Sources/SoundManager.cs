using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip m_clipBGM;
    public AudioClip[] m_clipPop;

    private AudioSource m_audioBGM;
    private AudioSource m_audioPop;

    public bool m_isBGM = true;
    public bool m_isSFX = true;

    public void Awake()
    {
        m_audioBGM = gameObject.AddComponent<AudioSource>();
        m_audioPop = gameObject.AddComponent<AudioSource>();

        m_audioBGM.clip = m_clipBGM;
        m_audioPop.clip = m_clipPop[1];

        m_audioBGM.volume = 0.5f;
        m_audioPop.volume = 0.01f;
    }

    public IEnumerator Play_BGM()
    {
        m_audioBGM.loop = true;
        m_audioBGM.Play();
        yield return null;
    }

    public void Stop_BGM()
    {
        if (m_audioBGM.isPlaying)
            m_audioBGM.Stop();
    }

    public IEnumerator Pop()
    {
        m_audioPop.Play();
        yield return null;
    }
}
