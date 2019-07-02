using SakuraClicker;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlossomManager : MonoBehaviour {
    public CoupleManager m_scriptCoupleManager;
    public SoundManager m_scriptSoundManager;

    private const int MIN_BRANCH = 4;
    private const int MAX_BRANCH = 7;
    // 벚꽃 풀
    private ObjectPool m_blossomPool;
    private ObjectPool m_blossomleafPool;
    private ObjectPool m_branchPool;
    // 나뭇가지 리스트
    private List<Branch> m_listPreUse;
    private List<Branch> m_listNowUse;

    private int m_nPopCount;

    private void Awake()
    {
        GameObject[] blossomleafs = Resources.LoadAll<GameObject>("Prefabs/Leaf");
        m_blossomleafPool = new ObjectPool(blossomleafs, 100);
        GameObject[] blossom = Resources.LoadAll<GameObject>("Prefabs/Blossom");
        m_blossomPool = new ObjectPool(blossom, 600);
        GameObject[] branches = Resources.LoadAll<GameObject>("Prefabs/Branch");
        m_branchPool = new ObjectPool(branches, 12);

        m_listPreUse = new List<Branch>();
        m_listNowUse = new List<Branch>();
    }

    // 나뭇가지 생성 로딩
    public IEnumerator LoadBranch()
    {
        yield return StartCoroutine(m_branchPool.CreateElement());
    }
    // 벚꽃 생성 로딩
    public IEnumerator LoadBlossoms()
    {
        yield return StartCoroutine(m_blossomPool.CreateElement());
    }
    // 잎 생성 로딩
    public IEnumerator LoadLeaves()
    {
        yield return StartCoroutine(m_blossomleafPool.CreateElement());
    }
    // 나뭇가지 준비시키기
    public IEnumerator PrepareBranch()
    {
        m_listPreUse = new List<Branch>();

        var count = Random.Range(MIN_BRANCH, MAX_BRANCH);
        var elems = m_branchPool.Pops(count);

        foreach (PoolElement elem in elems)
            m_listPreUse.Add(elem.gameObject.GetComponent<Branch>());

        
        foreach (Branch br in m_listPreUse)
        {
            yield return new WaitForSeconds(0.1f);
            // 나뭇가지 방향
            if (count % 2 == 0)
                if (count == 0)
                    br.RandomBranchDirection();
                else
                    br.SetBranchDirection(BranchDir.Right);
            else
                br.SetBranchDirection(BranchDir.Left);

            // 나뭇가지 준비
            br.MoveToReady();

            // 벚꽃 준비
            br.DisposeBlossoms(m_blossomPool.Pops(br.RandomBlossomQty(), br.gameObject.transform));

            count--;
        }
    }
    // 나뭇가지 자리 옮겨주기
    public IEnumerator MoveBranchToScreen()
    {
        m_listNowUse = new List<Branch>(m_listPreUse);
        m_listPreUse.Clear();

        foreach (Branch br in m_listNowUse)
            StartCoroutine(br.MoveToScreen());

        yield return null;
    }
    // 나무가 비었는지 검사
    public IEnumerator CheckBlankBranch()
    {
        List<Branch> listDel = new List<Branch>();
        while (m_listNowUse.Count != 0)
        {
            foreach (Branch br in m_listNowUse)
            {
                if (br.CountBlossoms() == 0)
                {
                    // 안 보이는 곳으로
                    br.MoveToReady();
                    m_branchPool.Push(br);
                    listDel.Add(br);
                }
            }
            foreach (Branch br in listDel)
                m_listNowUse.Remove(br);
            listDel.Clear();

            yield return new WaitForSecondsRealtime(0.2f);
        }
    }

    public void PopBlossom(Vector2 position)
    {
        StartCoroutine(m_scriptSoundManager.Pop());
        PoolElement leaf = m_blossomleafPool.Pop();
        leaf.gameObject.transform.position = position;
        leaf.GetComponent<BlossomLeaf>().Fall();
        m_nPopCount++;
    }
    public int popcount
    {
        get
        {
            return m_nPopCount;
        }
    }
}