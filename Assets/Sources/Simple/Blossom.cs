using SakuraClicker;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Blossom : PoolElement
{
    public BlossomManager m_scriptBlossomManager;

    public void Awake()
    {
        m_scriptBlossomManager = GameObject.Find("BlossomManager").GetComponent<BlossomManager>();
    }
    public void Pop()
    {
        m_scriptBlossomManager.PopBlossom(transform.position);
        ReturnToPool();
    }
}