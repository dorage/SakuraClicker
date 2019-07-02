using SakuraClicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : PoolElement
{
    public enum BranchType
    {
        TYPE_A = 0,
        TYPE_B = 1,
        TYPE_C = 2,
        TYPE_D = 3
    };
    public BranchType m_enBranchType = BranchType.TYPE_A;

    // 나뭇가지 방향 / 0 = 오른쪽 / 1 = 왼쪽
    public int m_nDirection = 0;
    // 벚꽃 최소/최대 수
    public int m_nMinBlossom = 0;
    public int m_nMaxBlossom = 0;
    // 벚꽃 생성 최소/최대 x값
    public float m_fMinLocalX;
    public float m_fMaxLocalX;
    // 나뭇 가지 최소/최대 y값
    public float m_fMinY = 0;
    public float m_fMaxY = 0;
    // 인게임에서 X값
    public float Game_X = 3f;
    public float Game_X_480 = 3f;
    public float Game_X_1080 = 3f;
    public float Game_X_1440 = 3f;

    private const float READY_X = 9f;
    private const string PATH = "Prefabs/Branches/Branches_";

    private BlossomManager m_scriptBlossomManager;

    public void Awake()
    {   
        m_scriptBlossomManager = GameObject.Find("BlossomManager").GetComponent<BlossomManager>();
        SetX();
    }

    public void SetX()
    {
        switch(Screen.width)
        {
            case 480:
                Game_X = Game_X_480;
                break;

            case 1080:
                Game_X = Game_X_1080;
                break;

            case 1440:
                Game_X = Game_X_1440;
                break;

            default:
                Game_X = Game_X_1080;
                break;
        }
    }

    // 나뭇가지 방향조절 (랜덤)
    public void RandomBranchDirection()
    {
        var rand = Random.Range(0, 2);

        SetBranchDirection((BranchDir)rand);
    }
    // 나뭇가지 방향조절
    public void SetBranchDirection(BranchDir brDir)
    {
        var nDir = (int)brDir;

        if (nDir == m_nDirection)
            return;

        var rV3 = new Vector3(0, 180 * nDir, 0);
        gameObject.transform.rotation = Quaternion.Euler(rV3);
        m_nDirection = nDir;
    }

    // 준비위치에 위치시키기
    public void MoveToReady()
    {
        var y = RandomHeight();

        if (m_nDirection == 0)
        {
            gameObject.transform.position = new Vector2(READY_X, y);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            gameObject.transform.position = new Vector2(-READY_X, y);
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

    }
    // 화면으로 위치시키기
    public IEnumerator MoveToScreen()
    {
        var lerpTime = 0.0f;
        var startPos = gameObject.transform.position;
        var endPos = Vector2.zero;

        if (m_nDirection == 0)
           endPos = new Vector2(Game_X, RandomHeight());
        else
           endPos = new Vector2(-Game_X, RandomHeight());

        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            lerpTime += 0.05f;
            gameObject.transform.position = Vector2.Lerp(startPos, endPos, lerpTime);

            if (lerpTime >= 1f)
                break;
        }
    }

    // 랜덤한 벚꽃 개수
    public int RandomBlossomQty()
    {
        var rand = Random.Range(m_nMinBlossom, m_nMinBlossom);
        return rand;
    }
    // 벚꽃 배치
    public void DisposeBlossoms(List<PoolElement> blossoms)
    {
        foreach (PoolElement blossom in blossoms)
        {
            blossom.transform.localPosition = RandomBlossomPos();
        }
    }
    // 빈 나뭇가지 체크
    public int CountBlossoms()
    {
        return transform.childCount;
    }

    // 랜덤 벛꽃 위치 테스트용
    [ContextMenu("AddFlower")]
    public void AddFlower()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        go.transform.parent = transform;
        go.transform.localPosition = RandomBlossomPos();
    }
    public void AddFlower(Vector2 position)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        go.transform.parent = transform;
        go.transform.localPosition = position;
    }

    // 랜덤한 높이 위치 리턴
    private float RandomHeight()
    {
        int min = (int)(m_fMinY * 10);
        int max = (int)(m_fMaxY * 10);
        float randY = (Random.Range(min, max)) / 10;

        return randY;
    }

    // 랜덤한 벚꽃 위치
    private Vector2 RandomBlossomPos()
    {
        var x = RandomBlossomX();
        var y = RandomBlossomY(x);

        return new Vector2(x, y);
    }
    private float RandomBlossomX()
    {
        Vector2[] spaces = BlossomSpace.GetSpaces(m_enBranchType);
        var min_x = spaces[0].x;
        var max_x = spaces[0].x;
        var x = spaces[0].x;

        foreach (Vector2 v in spaces)
        {
            x = v.x;
            if (x > max_x)
                max_x = x;
            else if (x < min_x)
                min_x = x;
        }
        
        float rand = Random.Range((int)(min_x*10), (int)(max_x*10));
        return rand / 10.0f;
    }

    private float RandomBlossomY(float rand_x)
    {
        Vector2[] spaces = BlossomSpace.GetSpaces(m_enBranchType);
        float y1 = -99;
        float y2 = -99;
        float rand_y = 0;

        for(int i = 0; i < spaces.Length; i++)
        {
            var next_i = i + 1;
            if (next_i == spaces.Length)
                next_i = 0;
            if(spaces[i].x >= rand_x && spaces[next_i].x < rand_x)
            {
                var slope = (spaces[i].y - spaces[next_i].y) / (spaces[i].x - spaces[next_i].x);
                var constant = spaces[i].y - (spaces[i].x * slope);
                y1 = slope * rand_x + constant;
            }
            if(spaces[i].x <= rand_x && spaces[next_i].x > rand_x)
            {
                var slope = (spaces[i].y - spaces[next_i].y) / (spaces[i].x - spaces[next_i].x);
                var constant = spaces[i].y - (spaces[i].x * slope);
                y2 = slope * rand_x + constant;
            }

        }

        if (y1 == -99)
            y1 = y2;
        else if (y2 == -99)
            y2 = y1;

        if (y1 > y2)
            rand_y = Random.Range(y1, y2);
        else if (y1 < y2)
            rand_y = Random.Range(y2, y1);
        else if (y1 == y2)
            rand_y = y1;
      
        return rand_y;
    }
}

public static class BlossomSpace
{
    private static Vector2[] space_1
        = { new Vector2(-0.7f, -0.1f) , new Vector2(-1.0f, 1.0f) , new Vector2(-1.9f, 1.7f) , new Vector2(-2.9f, 0.7f) , new Vector2(-3.3f, 0.6f) , new Vector2(-3.8f, 1.1f) , new Vector2(-3.9f, 0.1f) , new Vector2(-5.0f, 0.1f) , new Vector2(-3.8f, -0.4f) };
    private static Vector2[] space_2 
        = { new Vector2(-0.8f, -0.2f), new Vector2(-1.6f, 0.8f), new Vector2(-2.5f, 1.3f), new Vector2(-3.4f, 0.1f), new Vector2(-4f, 0.5f), new Vector2(-4.2f, -0.4f), new Vector2(-5f, -0.5f), new Vector2(-3f, -0.8f) };
    private static Vector2[] space_3 
        = { new Vector2(-0.7f, 0f), new Vector2(-1.4f, 0.5f), new Vector2(-3.7f, 0.9f), new Vector2(-2.9f, 0.2f) };
    private static Vector2[] space_4 
        = { new Vector2(-0.8f, -0.1f), new Vector2(-2.2f, 0.7f), new Vector2(-2.0f, 0.3f) };

    private static Vector2[][] spaces
        = { space_1, space_2, space_3, space_4 };

    public static Vector2[] GetSpaces(Branch.BranchType branchtype)
    {
        return spaces[(int)branchtype];
    }
}