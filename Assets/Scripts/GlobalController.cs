using UnityEngine;
using System.Collections.Generic;

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
    public List<AnimationPlayer> animPlayers = new List<AnimationPlayer>();
    public bool debugMode = false;
	// Use this for initialization
	void Awake() {
        if (debugMode)
            return;
        startAnimPlayer.EnterAnimation();
	}

    public AnimationPlayer mainScreen;
    public AnimationPlayer navBar;

    int initialMenu = 0;

    public AnimationPlayer previousMenu;
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
        for(int i = 0; i < animPlayers.Count; i++)
        {
            if(animPlayers[i].gameObject.activeInHierarchy)
            {
                if(animPlayers[i] != startAnimPlayer)
                {
                    if (i == animPlayers.Count - 1)
                    {
                        navBar.ExitAnimation();
                    }
                    animPlayers[i].ExitAnimation();
                }
            }
        }
        mainScreen.EnterAnimation();
    }
}
