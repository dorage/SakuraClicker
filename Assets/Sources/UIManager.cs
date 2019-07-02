using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public BlossomManager m_scriptBlossomManager;

    public Text m_txtScore;

    public void Start()
    {
        m_txtScore.gameObject.SetActive(false);
    }

    public void Update()
    {
        m_txtScore.text = m_scriptBlossomManager.popcount.ToString() + " 송이";
    }
    
    public void ShowGUI()
    {
        m_txtScore.gameObject.SetActive(true);
    }
}
