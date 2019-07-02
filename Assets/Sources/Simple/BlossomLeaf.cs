using SakuraClicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlossomLeaf : PoolElement
{
    private Animator m_animator;

    public float m_fSlope = 1;
    public float m_fConst = 1;

    public float m_fGround = -4;

    public const float SPEED = 5;
    public const float TERM = 0.01f;

    public void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
    }

    public void Fall()
    {
        Initialize();
        StartCoroutine("LerpLeaf");
    }

    private void Initialize()
    {
        m_animator.SetBool("isFallen", false);
        m_fGround = Random.Range(35, 40) / -10.0f;
        m_fSlope = Random.Range(1, 10) / 10.0f;
        m_fConst = CalcConst();
    }
    
    private float CalcConst()
    {
        var pos = transform.position;
        return pos.x / m_fSlope * (float)System.Math.Sin((double)pos.y);
    }

    private IEnumerator LerpLeaf()
    {
        Vector2 pos = transform.position;

        while(pos.y > m_fGround)
        {
            pos.y -= SPEED * TERM;
            pos.x = m_fSlope * pos.y + m_fConst;

            gameObject.transform.position = pos;

            yield return new WaitForSecondsRealtime(TERM);
        }

        m_animator.SetBool("isFallen", true);

        yield return new WaitForSecondsRealtime(5);

        ReturnToPool();
    }
}