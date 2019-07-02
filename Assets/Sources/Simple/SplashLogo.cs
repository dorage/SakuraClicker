using UnityEngine;
using System.Collections;

public class SplashLogo : MonoBehaviour
{
    // 애니메이션이 끝났는가
    private bool m_isEnded;

    public void Awake()
    {
        m_isEnded = false;
    }

    public void AnimationEnd()
    {
        m_isEnded = true;
    }

    /// <summary>
    /// 시퀀스가 끝나면 반환됩니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator SequenceCheck()
    {
        while(!m_isEnded)
        {
            yield return null;
        }
    }
}
