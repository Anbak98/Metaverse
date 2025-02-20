using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum UIState
{
    Home,
    AvatarShop
}

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            return instance;
        }
    }

    UIState currentState = UIState.Home;

    AvatarShopUI avatarShopUI = null;

    private void Awake()
    {
        instance = this;
        avatarShopUI = GetComponentInChildren<AvatarShopUI>(true);
        avatarShopUI?.Init(this);

        ChangeState(UIState.Home);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;
        avatarShopUI?.SetActive(currentState);
    }

    public void OnClickStart()
    {
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
