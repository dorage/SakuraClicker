using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SakuraClicker
{

    public enum BranchDir
    {
        Right = 0,
        Left = 1
    };

    public class PoolElement : MonoBehaviour
    {
        private int m_nId;
        private ObjectPool m_pool;

        public ObjectPool pool
        {
            set { m_pool = value; }
        }
        public int id
        {
            set { m_nId = value; }
            get { return m_nId; }
        }
        public void ReturnToPool()
        {
            m_pool.Push(this);
        }
    }

    public class ObjectPool
    {
        private List<GameObject> m_listOriginal;
        private List<PoolElement> m_listUsing;
        private List<Stack<PoolElement>> m_stackPool;

        private int m_nPoolCapacity;

        // 생성자
        public ObjectPool(GameObject poolObject, int count)
        {
            Initialize(count);
            m_listOriginal.Add(poolObject);
        }
        public ObjectPool(List<GameObject> poolObjects, int count)
        {
            Initialize(count);
            m_listOriginal.AddRange(poolObjects);
        }
        public ObjectPool(GameObject[] poolObjects, int count)
        {
            Initialize(count);
            m_listOriginal.AddRange(poolObjects);
        }

        // 풀 요소 생성
        public IEnumerator CreateElement()
        {
            for (int i = 0; i < m_listOriginal.Count; i++)
                m_stackPool.Add(new Stack<PoolElement>());

            int limit = (int)(m_nPoolCapacity / m_listOriginal.Count);
            limit = m_nPoolCapacity % m_listOriginal.Count == 0 ? limit : limit++;

            for (int i = 0; i < (int)(m_nPoolCapacity / m_listOriginal.Count); i++)
            {
                for (int j = 0; j < m_listOriginal.Count; j++)
                {
                    var go = GameObject.Instantiate(m_listOriginal[j], new Vector2(99, 0), Quaternion.identity) as GameObject;
                    go.SetActive(false);
                    PoolElement elem = go.GetComponent<PoolElement>();
                    elem.pool = this;
                    elem.id = j;

                    m_stackPool[j].Push(elem);

                    yield return null;
                }
            }
        }

        // 꺼내는 스택, 랜덤으로 가져오기
        private Stack<PoolElement> GetRandomStack()
        {
            var rand = 0;

            while(true)
            {
                rand = Random.Range(0, m_stackPool.Count);
                if (m_stackPool[rand].Count != 0)
                {
                    break;
                }
            }
            return m_stackPool[rand];
        }

        private PoolElement CommonPop(Transform parent = null)
        {
            PoolElement elem = GetRandomStack().Pop();
            elem.gameObject.SetActive(true);
            if (parent != null)
                elem.gameObject.transform.parent = parent;
            m_listUsing.Add(elem);

            return elem;
        }
        public PoolElement Pop(Transform parent = null)
        {
            if (!CheckStack())
                return null;

            return CommonPop(parent);
        }
        public List<PoolElement> Pops(int count, Transform parent = null)
        {
            if (!CheckStack(count))
                return null;

            List<PoolElement> elems = new List<PoolElement>();

            for (int i = 0; i < count; i++)
                elems.Add(CommonPop(parent));

            return elems;
        }
        
        public void Push(PoolElement poolelement)
        {
            m_stackPool[poolelement.id].Push(poolelement);
            m_listUsing.Remove(poolelement);

            poolelement.gameObject.SetActive(false);
            poolelement.gameObject.transform.parent = null;
        }
        
        private int poolcount
        {
            get
            {
                var pool_count = 0;
                var used_count = m_listUsing.Count;

                for (int i = 0; i < m_stackPool.Count; i++)
                    pool_count += m_stackPool[i].Count;

                Debug.Log("Pool : " + pool_count + " / Using : " + used_count);

                return pool_count;
            }
        }
        
        // 초기화
        private void Initialize(int count)
        {
            m_nPoolCapacity = count;
            m_listOriginal = new List<GameObject>();
            m_listUsing = new List<PoolElement>();
            m_stackPool = new List<Stack<PoolElement>>();
        }

        // 사용할 수 있는 상태인지 체크
        private bool CheckStack(int count = 0)
        {
            if (m_stackPool == null)
            {
                Debug.Log("Stack is Empty!");
                return false;
            }
            if (poolcount < count)
            {
                Debug.Log("Pool needs more objects");
                return false;
            }
            if (count < 0)
            {
                Debug.Log("You have to write positive number");
                return false;
            }
            return true;
        }
    }

    public class Polygon
    {
        private struct Vertex
        {
            float x, y;
        }

        public Polygon()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {

        }

        public void Random()
        {

        }
    }
}