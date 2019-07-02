using UnityEngine;
using System.Collections;

public class GameSequenceManager : MonoBehaviour
{
    private enum Sequence
    {
        Splash,
        Title,
        Game,
        Setting
    }

    public BlossomManager m_scriptBlossomManager;
    public CoupleManager m_scriptCoupleManager;
    public SoundManager m_scriptSoundManager;
    public UIManager m_scriptUIManager;

    public GameObject Logo_Splash_Dorage;
    public GameObject Logo_Game_Title;

    private SplashLogo m_scriptSplashLogo;
    private GameTitle m_scriptGameTitle;

    private Sequence m_enumGameSequence;

	private void Awake()
    {
        m_scriptSplashLogo = Logo_Splash_Dorage.GetComponent<SplashLogo>();
        m_scriptGameTitle = Logo_Game_Title.GetComponent<GameTitle>();
	}

    private void Start()
    {
        StartCoroutine(GameSequence());
    }

    private IEnumerator GameSequence()
    {
        // 스플래쉬
        m_enumGameSequence = Sequence.Splash;
        Log_GameSequence();
        yield return WaitForRealSecond(0.5f);
        SetObjectActive(Logo_Splash_Dorage, true);
        // 스플래쉬 로고 애니메이션 끝 기다림 (애니메이션 트리거)
        yield return StartCoroutine(m_scriptSplashLogo.SequenceCheck());
        SetObjectActive(Logo_Splash_Dorage, false);
        yield return WaitForRealSecond(0.5f);

        // 타이틀
        m_enumGameSequence = Sequence.Title;
        Log_GameSequence();
        SetObjectActive(Logo_Game_Title, true);
        // 음악재생
        yield return StartCoroutine(m_scriptSoundManager.Play_BGM());
        // 로딩시작
        yield return StartCoroutine(m_scriptBlossomManager.LoadBranch());
        // 벚꽃 생성
        yield return StartCoroutine(m_scriptBlossomManager.LoadBlossoms());
        // 꽃잎 생성
        yield return StartCoroutine(m_scriptBlossomManager.LoadLeaves());
        // 나뭇가지 준비
        yield return StartCoroutine(m_scriptBlossomManager.PrepareBranch());
        // 로딩완료
        m_scriptGameTitle.GameLoaded();
        // Fade Out Animation
        yield return StartCoroutine(m_scriptGameTitle.SequenceCheck());
        SetObjectActive(Logo_Game_Title, false);
        yield return WaitForRealSecond(0.5f);
        
        m_scriptUIManager.ShowGUI();
         
        m_enumGameSequence = Sequence.Game;
        Log_GameSequence();
        while (m_enumGameSequence == Sequence.Game)
        {
            // 게임화면에 등장
            yield return StartCoroutine(m_scriptBlossomManager.MoveBranchToScreen());
            yield return StartCoroutine(m_scriptCoupleManager.MoveCoupleToReady());
            StartCoroutine(m_scriptCoupleManager.MoveCoupleToGame());
            // 나뭇가지 준비
            StartCoroutine(m_scriptBlossomManager.PrepareBranch());
            // 나뭇가지 관리
            yield return StartCoroutine(m_scriptBlossomManager.CheckBlankBranch());
        }
    }

    private void Log_GameSequence()
    {
        Debug.Log("Start" + m_enumGameSequence);
    }

    private void SetObjectActive(GameObject go, bool value)
    {
        go.SetActive(value);
    }

    public Coroutine WaitForRealSecond(float time)
    {
        return StartCoroutine(Wait(time));
    }

    private IEnumerator Wait(float time)
    {
        var current = Time.realtimeSinceStartup;

        while(Time.realtimeSinceStartup - current < time)
        {
            yield return null;
        }
    }

    /*
      내가 그의 이름을 불러주기 전에는 
           그는 다만 
           하나의 몸짓에 지나지 않았다. 

           내가 그의 이름을 불러주었을 때 
           그는 나에게로 와서 
           꽃이 되었다. 

           내가 그의 이름을 불러준 것처럼 
           나의 이 빛깔과 향기에 알맞는 0
           누가 나의 이름을 불러 다오. 
           그에게로 가서 나도 
           그의 꽃이 되고 싶다. 

           우리들은 모두 
           무엇이 되고 싶다. 
           나는 너에게 너는 나에게 
           잊혀지지 않는 하나의 의미가 되고 싶다. 
     */ 
}
