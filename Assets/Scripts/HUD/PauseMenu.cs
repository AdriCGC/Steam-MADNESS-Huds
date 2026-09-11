using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject PauseObject;
    [SerializeField] GameObject InGameHud;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] Volume Blur;
    [SerializeField] FadeTest fade;
    [SerializeField] Player player;
    [SerializeField] Slider SliderSense;
    private DepthOfField depthOfField;
    
    
    

    void Awake()
    {
            Blur.profile.TryGet(out depthOfField);

    }

    // Update is called once per frame
    void Update()
    {

        player.sensibilidadeMouse = SliderSense.value;


        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            StartCoroutine(PauseSystem());
        }

    }
    IEnumerator PauseSystem()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PauseObject.SetActive(true);
        InGameHud.SetActive(false);
        Time.timeScale = 0f;
        depthOfField.focusDistance.value = 0.1f;
        yield break;
    }
    
    public void Retomar()
    {
         Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PauseObject.SetActive(false);
        InGameHud.SetActive(true);
        Time.timeScale = 1f;
        depthOfField.focusDistance.value = 10f;
    }
    public void Opcoes()
    {
        OptionsMenu.SetActive(true);
    }
    
    public void Sair()
    {
        fade.escurecer = true;
        fade.cenaTransicao = true;
        fade.cena = "GameMenu";
        StartCoroutine(fade.Fade());
    }
    public void Voltar()
    {
        OptionsMenu.SetActive(false);
        PauseObject.SetActive(true);
    }



}
