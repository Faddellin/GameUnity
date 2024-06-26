using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameScene;

public class Prologue : MonoBehaviour
{
    public bool IsFirstText;
    public bool IsLastText;
    public string[] Lines;
    public float TextSpeed;
    public TMP_Text PrologueText;
    public Player Player;
    private int Index;

    private void Start()
    {
        PrologueText.text = string.Empty;
        StartPrologueText();
        if (IsFirstText)
        {
            Player.gameObject.SetActive(false);
        }
    }

    private void StartPrologueText()
    {
        Index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in Lines[Index].ToCharArray())
        {
            PrologueText.text += c;
            yield return new WaitForSeconds(TextSpeed);
        }
    }

    public void SkipText()
    {
        if (PrologueText.text == Lines[Index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            PrologueText.text = Lines[Index];
        }
    }

    private void NextLine()
    {
        if (Index < Lines.Length - 1)
        {
            Index++;
            PrologueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            if (IsFirstText)
            {
                Player.gameObject.SetActive(true);
            }
            if (IsLastText)
            {
                SceneManager.LoadScene("Second mission");
            }
        }
    }
}
