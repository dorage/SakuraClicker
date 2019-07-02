using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    public GameObject leaf;
    void Start()
    {
        StartCoroutine(Gen());
    }

    public IEnumerator Gen()
    {
        while(true)
        {
            GameObject.Instantiate(leaf, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
