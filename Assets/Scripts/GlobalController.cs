using UnityEngine;
using System.Collections;

public class GlobalController : MonoBehaviour {

    private static GlobalController _instance;

    public static GlobalController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GlobalController>();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

    public AnimationPlayer startAnimPlayer;
    AnimationPlayer[] animPlayers;
    public bool debugMode = false;
	// Use this for initialization
	void Awake() {
        animPlayers = FindObjectsOfType<AnimationPlayer>();
        if (debugMode)
            return;
        foreach(AnimationPlayer ap in animPlayers)
        {
            ap.gameObject.SetActive(false);
        }
        startAnimPlayer.EnterAnimation();
	}

    public AnimationPlayer mainScreen;
    public AnimationPlayer navBar;

    int initialMenu = 0;

    public AnimationPlayer currentMenu;
    public void SetCurrentMenu(AnimationPlayer animPlayer)
    {
        if(initialMenu < 2)
        {
            if (animPlayer.gameObject.name == "MainScreen")
            {
                initialMenu++;
                if (initialMenu > 1)
                {
                    if (animPlayer.enterAnimation == "MainScreenIntro")
                    {
                        animPlayer.enterAnimation = "MainScreenEnter";
                    }
                }
            }
        }

        currentMenu = animPlayer;
    }

    public void GoToHomeScreen()
    {
        currentMenu.ExitAnimation(mainScreen);
        navBar.ExitAnimation();
    }
}
