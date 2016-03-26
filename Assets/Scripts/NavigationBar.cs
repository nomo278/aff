using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NavigationBar : MonoBehaviour {

    public Text monthYear;

    public Image panelButton;

    public Sprite menuIcon, backIcon;

    bool isMenuIcon = true;
    public void SetIconMenuButton()
    {
        if(!isActiveAndEnabled)
            return;
        if(!isMenuIcon)
        {
            StopAllCoroutines();
            StartCoroutine(FadeToImage(menuIcon));
            isMenuIcon = true;
        }
    }

    public void SetIconBackArrow()
    {
        if(!isActiveAndEnabled)
            return;
        if(isMenuIcon)
        {
            StopAllCoroutines();
            StartCoroutine(FadeToImage(backIcon));
            isMenuIcon = false;
        }
    }

    float fadeTime = 0.5f;
    IEnumerator FadeToImage(Sprite targetSprite)
    {
        Color color = panelButton.color;
        Color endColor = color;
        endColor.a = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < fadeTime)
        {
            color = Color.Lerp(color, endColor, elapsedTime / fadeTime);
            panelButton.color = color;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        panelButton.color = endColor;
        panelButton.sprite = targetSprite;
        elapsedTime = 0f;
        color = panelButton.color;
        endColor = color;
        endColor.a = 1f;
        while (elapsedTime < fadeTime)
        {
            color = Color.Lerp(color, endColor, elapsedTime / fadeTime);
            panelButton.color = color;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
