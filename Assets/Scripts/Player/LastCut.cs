using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LastCut : MonoBehaviour
{
    public BossHealth health;
    public GameObject Panel;
    public GameObject Image;
    public GameObject Audio;

    public void StartEnding()
    {
        StartCoroutine(Ending());
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(2f);
        Panel.SetActive(true);
        yield return new WaitForSeconds(2f);
        Image.SetActive(true);
        yield return new WaitForSeconds(4f);
        Image.SetActive(false);
        Audio.SetActive(true);
        yield return new WaitForSeconds(1.8f);
        SceneManager.LoadScene("Menu");
    }
}
