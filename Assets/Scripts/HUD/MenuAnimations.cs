using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MenuAnimations : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] GameObject fade;
    [SerializeField] GameObject MenuPrincipal;
    [SerializeField] GameObject MenuOpcoes;

    [SerializeField] bool escurecer = false;
    [SerializeField] int Phases = 0;
    void Start()
    {
        StartCoroutine(Fade());
    }


    public void StartGame()
    {
        Phases = 1;
        escurecer = true;
        StartCoroutine(Fade());        
    }

    
    public void MainOptions()
    {
        Phases = 2;
        escurecer = true;
        StartCoroutine(Fade());     
    }

    public void MainCredits()
    {
        Phases = 3;
        escurecer = true;
        StartCoroutine(Fade());     
    }

    public void ExitGame()
    {
        Phases = 4;
        escurecer = true;
        StartCoroutine(Fade());     
    }
      public void OptionsExit()
    {
        Phases = 5;
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

        }

        if(escurecer == true)
        {
        fade.SetActive(true);
        animator.SetTrigger("Escurecer");
        yield return new WaitForSeconds(1.7f);
        if(Phases == 1)
            {
            SceneManager.LoadScene("GameScene");
            }
        if(Phases == 2)
            {
            MenuPrincipal.SetActive(false);
            MenuOpcoes.SetActive(true);
            animator.SetTrigger("clarear");
            yield return new WaitForSeconds(1.7f);
            fade.SetActive(false);
            }
        if(Phases == 3)
            {
                SceneManager.LoadScene("GameCredits");
            }
            if(Phases == 4)
            {
                Application.Quit();
            }
            if(Phases == 5)
            {
            MenuPrincipal.SetActive(true);
            MenuOpcoes.SetActive(false);
            animator.SetTrigger("clarear");
            yield return new WaitForSeconds(1.7f);
            fade.SetActive(false);
            }
        }
    }
}
