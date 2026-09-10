using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour
{

    [SerializeField] FadeTest Fade;
    void Start()
    {
        StartCoroutine(Fade.Fade());
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Replay()
    {
        Fade.escurecer = true;
        Fade.cenaTransicao = true;
        StartCoroutine(Fade.Fade());
        Fade.cena = "GameScene";
    }
        public void Return()
    {
        Fade.escurecer = true;
        Fade.cenaTransicao = true;
        Fade.cena = "GameMenu";
        StartCoroutine(Fade.Fade());
    }
}
