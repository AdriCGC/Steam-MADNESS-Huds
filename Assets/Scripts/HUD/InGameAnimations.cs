using UnityEngine;
using System.Collections;


public class InGameAnimations : MonoBehaviour
{

    [SerializeField] private FadeTest Fade;
    [SerializeField] Animator animator;
    [SerializeField] GameObject Poucavida;
    public bool Morrendo = false;


    
    void Start()
    {
        StartCoroutine(Fade.Fade());
    }

    void Update()
    {
        if (Morrendo == true)
        {
        Poucavida.SetActive(true);
        }
        else
        {
        Poucavida.SetActive(false);   
        }
    }


    public void Dano()
    {
        animator.SetTrigger("Dano");
    }

    public void death()
    {
        StartCoroutine(Fade.FadeMorte());
    }
}
