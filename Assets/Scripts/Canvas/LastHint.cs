using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LastHint : MonoBehaviour
{
    public GameObject UIhint;
    private CanvasGroup canvasGroup;
    public EnemyHealth Enemy;
    public float fadeDuration = 0.5f;

    private void Start()
    {
        canvasGroup = UIhint.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = UIhint.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
    }

    public void Update()
    {
        if (Enemy.IsDied)
        {
            StartCoroutine(FadeIn());
        }
    }


    IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }
}