using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Credits : MonoBehaviour
{
    public List<string> names;

    private Text creditsText;
    private Animator animator;

    private List<string> currentNames;

    private void Start()
    {
        creditsText = GetComponent<Text>();
        animator = GetComponent<Animator>();
    }

    public void ShowCredits()
    {
        currentNames = new List<string>(names);
        NextName();
    }

    public void NextName()
    {
        if (currentNames.Count != 0)
        {
            int index = Random.Range(0, currentNames.Count);
            creditsText.text = currentNames[index];
            currentNames.RemoveAt(index);

            animator.SetTrigger("nextName");
        }
    }
}