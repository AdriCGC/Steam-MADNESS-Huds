using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class MenuAnimations : MonoBehaviour
{

    [SerializeField] Animator animator;
    [SerializeField] GameObject MenuPrincipal;
    [SerializeField] GameObject MenuOpcoes;
    [SerializeField] FadeTest fade;
    int back = 1;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Start()
    {
        StartCoroutine(fade.Fade());
    }


    public void StartGame()
    {
        fade.cenaTransicao = true;
        fade.escurecer = true;
        fade.cena = "GameScene";
        StartCoroutine(fade.Fade());
    }

    
    public void MainOptions()
    {
        fade.escurecer = true;
        StartCoroutine(fade.Fade());
        StartCoroutine(InternalChange());     
    }

    public void MainCredits()
    {
        fade.cenaTransicao = true;
        fade.escurecer = true;
        fade.cena = "GameCredits";
        StartCoroutine(fade.Fade());     
    }

    public void ExitGame()
    {
        fade.escurecer = true;
        StartCoroutine(fade.Fade());
        back = 2;
        StartCoroutine(InternalChange());       
    }
      public void OptionsExit()
    {
        fade.escurecer = true;
        StartCoroutine(fade.Fade());
        back = 0;
        StartCoroutine(InternalChange());          
    }

    IEnumerator InternalChange()
    {
        yield return new WaitForSeconds(1.7f);
        
        if (back == 1)
        {
            MenuPrincipal.SetActive(false);
            MenuOpcoes.SetActive(true);
        }
        else if(back == 0)
        {
            back = 1;
            MenuPrincipal.SetActive(true);
            MenuOpcoes.SetActive(false); 
        }
        else
        {
            back = 1;
            Application.Quit();
            print("Jogo encerrado");
        }


    }

   
}
