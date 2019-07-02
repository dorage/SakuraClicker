using UnityEngine;
using System.Collections;

public class GameTitle : MonoBehaviour
{
    // 애니메이션이 끝났는가
    private bool m_isEnded;
    // 게임 데이타와 오브젝트들이 로드되었는가
    private bool m_isLoaded;

    private Animator m_Animator;

    public void Awake()
    {
        m_isLoaded = false;
        m_isEnded = false;

        m_Animator = GetComponent<Animator>();
    }

    public void AnimationEnd()
    {
        m_isEnded = true;
    }
    public void GameLoaded()
    {
        m_isLoaded = true;
    }

    /// <summary>
    /// 시퀀스가 끝나면 반환됩니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SequenceCheck()
    {
        while(!m_isLoaded)
        {
            yield return null;
        }

        m_Animator.SetBool("out",true);

        while(!m_isEnded)
        {
            yield return null;
        }
    }
}
