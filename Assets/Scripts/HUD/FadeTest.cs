using UnityEngine;

public class FadeTest : MonoBehaviour
{
    [SerializeField] Animator animator;


    public void Escurecer()
    {
        animator.SetBool("Escurecer", true);
    }

    public void Clarear()
    {
        animator.SetBool("Escurecer", false);
    }
}