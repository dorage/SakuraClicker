using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class PolygonManager : MonoBehaviour
{
    public bool DrawLine;
    public GameObject m_handle;

    private List<GameObject> m_gos;

    public void Start()
    {
        m_gos = new List<GameObject>();
    }

    private void Update()
    {
        if (!DrawLine)
            return;

        var cnt = m_gos.Count;

        if (cnt > 1)
        {
            for (int i = 0; i < cnt; i++)
            {
                var v1 = m_gos[i].transform.position;
                var v2 = Vector2.zero;

                if (i == 0)
                {
                    v2 = m_gos[cnt - 1].transform.position;
                }
                else
                {
                    v2 = m_gos[i - 1].transform.position;
                }

                Debug.DrawLine(v1, v2);
            }
        }
    }

    [ContextMenu("AddButton")]
    public void AddSquare()
    {
        var go = Instantiate(m_handle, transform.position, Quaternion.identity) as GameObject;
        go.transform.parent = gameObject.transform;

        var num = m_gos.Count;
        go.name = "Handle_" + num;

        m_gos.Add(go);
    }

    [ContextMenu("PrintLog")]
    public void PrintLog()
    {
        string path = Application.dataPath + "/Resources/Log/" + gameObject.name + ".txt";
        StreamWriter sw = new StreamWriter(path);
        print(path);

        GetChilds();

        string s = "";
        for (int i = 0; i < m_gos.Count; i++)
        {
            Vector2 v2 = m_gos[i].transform.localPosition;
            s += "new Vector2(" + v2.x + "f, " + v2.y + "f), ";
        }
        sw.WriteLine(s);
        sw.Flush();
        sw.Close();
    }

    private void GetChilds()
    {
        m_gos = new List<GameObject>();
        foreach (Transform child in transform)
        {
            m_gos.Add(child.gameObject);
            print("Helo");
        }
    }
}