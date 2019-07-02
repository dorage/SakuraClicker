using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public decimal m_mBlossom;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(pos, pos, 1f);

            if (hit.collider != null)
            {
                hit.collider.GetComponent<Blossom>().Pop();
            }
        }
    }
}
