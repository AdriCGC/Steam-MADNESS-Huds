using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class CreditAnimation : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject Luzes;
    [SerializeField] GameObject credits;


    [SerializeField] bool escurecer = false;
    void Start()
    {
        StartCoroutine(Fade());
    }


    public void BackToMenu()
    {
        escurecer = true;
        StartCoroutine(Fade());        
    }

    

    IEnumerator Fade()
    {
        
        if(escurecer == false)
        {
            fade.SetActive(true);
            animator.SetTrigger("clarear");
            yield return new WaitForSeconds(1.7f);
            fade.SetActive(false);
            yield return StartCoroutine(Creditos());


        }

        if(escurecer == true)
        {
        fade.SetActive(true);
        animator.SetTrigger("Escurecer");
        yield return new WaitForSeconds(1.7f);
        SceneManager.LoadScene("GameMenu");
        }
    }
    IEnumerator Creditos()
    {
        yield return new WaitForSeconds(2f);
        Luzes.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        credits.SetActive(true);

    }
}
