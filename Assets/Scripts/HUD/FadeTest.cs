using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class FadeTest : MonoBehaviour
{
    [SerializeField] Animator animator;
    public bool escurecer = false;
    public bool cenaTransicao = false;
    [SerializeField] GameObject fade;
    public string cena;
    public bool debug = false;




    void Awake()
    {
        fade.SetActive(true);
    }




    public IEnumerator  Fade()
    {
        
        if(escurecer == false)
        {
            //fadeIn (clarear Tela)

            fade.SetActive(true);
            animator.SetTrigger("clarear");
            yield return new WaitForSecondsRealtime(1.7f);
            fade.SetActive(false);
           

        }


        if(escurecer == true)
        {
            //fadeOut (escurecer Tela)

        fade.SetActive(true);
        animator.SetTrigger("Escurecer");
        yield return new WaitForSecondsRealtime(1.7f);

        if(cenaTransicao == true)
            {
                print("GameScene");
                SceneManager.LoadScene(cena);
            }

        else
            {
                escurecer = false;
                yield return StartCoroutine(Fade()) ;
            }

        }

    }

    public IEnumerator FadeMorte()
    {
        fade.SetActive(true);
        animator.SetTrigger("Morte");
        yield return new WaitForSecondsRealtime(0.6f);
        SceneManager.LoadScene("GameDeath");

    }
}