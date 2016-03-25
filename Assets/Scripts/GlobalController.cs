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

    public delegate void BackMenuClick();
    public static event BackMenuClick OnBackMenuClick;


    public AnimationPlayer startAnimPlayer;
    public List<AnimationPlayer> animPlayers = new List<AnimationPlayer>();
    public bool debugMode = false;
	// Use this for initialization
	void Awake() {
        if (debugMode)
            return;
	}

    void Start()
    {
        mainScreen.EnterAnimation();
    }

    public AnimationPlayer mainScreen;
    public AnimationPlayer navBar;

    int initialMenu = 0;

    public AnimationPlayer previousMenu;
    public AnimationPlayer currentMenu;

    Stack<AnimationPlayer> menuStack = new Stack<AnimationPlayer>();
    public void SetCurrentMenu(AnimationPlayer animPlayer)
    {
        if(animPlayer.gameObject.name != "NavBar")
        {
            Debug.Log("pushing: " + animPlayer.name);
            menuStack.Push(animPlayer);
        }

        if (initialMenu < 2)
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
    }


    //public AnimationPlayer backMenu;

    public void BackMenu()
    {
        /*
        AnimationPlayer exitMenu = menuStack.Pop();
        Debug.Log("popping: " + exitMenu.name);
        // Debug.Log("exit menu: " + exitMenu);
        AnimationPlayer nextMenu = menuStack.Peek();
        Debug.Log("pushing: " + nextMenu.name);
        // Debug.Log("next menu: " + nextMenu);
        exitMenu.ExitAnimation(nextMenu); 
         
        */
        if (OnBackMenuClick != null)
            OnBackMenuClick();

        if (menuStack.Count < 1) { 

        }


            if (menuStack.Count > 1)
            if (menuStack.Peek().gameObject.name != "VenueList")
            {
                if(menuStack.Peek().gameObject.name == "EventInformation")
                {
                    menuStack.Pop().ExitAnimation();
                }
                else
                {
                    menuStack.Pop().ExitAnimation(menuStack.Peek());
                    if (menuStack.Peek().gameObject.name == "FilmCategories")
                        menuStack.Pop();
                }


            }
            else
            {
                menuStack.Pop().ExitAnimationNoDisable();
            }
    }

    public void BackMenuNoEnter()
    {
        Debug.Log("popping: " + menuStack.Peek().name);
        if(menuStack.Count > 0)
        {
            if(menuStack.Peek().gameObject.name != "VenueList")
                menuStack.Pop().ExitAnimation();
        }
    }

    public void GoToMenu(AnimationPlayer animationPlayer)
    {

        if(menuStack.Count > 0)
            menuStack.Peek().ExitAnimation(animationPlayer);
    }

    public void GoToMenuNoExit(AnimationPlayer animationPlayer)
    {
        animationPlayer.EnterAnimation();
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

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }
}
