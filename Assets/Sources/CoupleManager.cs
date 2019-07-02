using SakuraClicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoupleManager : MonoBehaviour
{
    private ObjectPool m_couplePool;

    public Couple m_scriptCouple;

    public void Awake()
    {
        //GameObject[] couples = Resources.LoadAll<GameObject>("Prefabs/Couples");

    }

    public IEnumerator Load()
    {
        yield return null;
    }

    public IEnumerator MoveCoupleToReady()
    {
        yield return StartCoroutine(m_scriptCouple.MoveToReady());
    }

    public IEnumerator MoveCoupleToGame()
    {
        yield return StartCoroutine(m_scriptCouple.MoveToGame());
    }
}
