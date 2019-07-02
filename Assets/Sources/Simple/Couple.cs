using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Couple : MonoBehaviour
{
    public CoupleManager m_scriptCoupleManager;
    public int m_nHp;

    public void Awake()
    {
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector2(0, 1f);
        gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
        m_nHp = 10;
    }

    public void GetDmg(int damage)
    {
        Debug.Log("Get " + damage + " Damage!");
        m_nHp -= damage;
        if (m_nHp <= 0)
            RunAway();
    }

    public void Kiss()
    {

    }

    public void Hug()
    {

    }

    public void RunAway()
    {
        gameObject.SetActive(false);
    }
    
    public IEnumerator MoveToReady()
    {
        Initialize();

        yield return null;
    }
    public IEnumerator MoveToGame()
    {
        var lerpTime = 0.0f;
        var startPos = gameObject.transform.position;
        var startScale = gameObject.transform.localScale;
        var endPos = new Vector2(0, -2.5f);
        var endScale = new Vector2(1, 1);

        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            lerpTime += 0.005f;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, lerpTime);
            gameObject.transform.localScale = Vector2.Lerp(startScale, endScale, lerpTime);

            if (lerpTime >= 1f)
                break;
        }
    }
}