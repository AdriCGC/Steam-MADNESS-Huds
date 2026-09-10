using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    //Hud
    [SerializeField] private InGameAnimations Animacao;


    // Movimento
    [SerializeField] private float velocidade = 10.0f;

    // Pulo
    [SerializeField] private float forcaPulo = 7.0f;

    // Câmera
    [SerializeField] private float sensibilidadeMouse = 20.0f;
    [SerializeField] private Transform camera;
    

    // Tiro
    [SerializeField] private float distanciaTiro = 100.0f;
    [SerializeField] private float intervaloTiro = 0.2f;

    // Munição
    [SerializeField] private int capacidadeMunicao = 5;
    [SerializeField] private float tempoRecarga = 1.5f;
    [SerializeField] TextMeshProUGUI Municao;

    // Dash
    [SerializeField] private float forcaDash = 15.0f;
    [SerializeField] private float tempoDash = 0.15f;
    [SerializeField] private float recargaDash = 1.0f;

    // Vida
    [SerializeField] private float vida = 100.0f;
    [SerializeField] private Slider BarraVida;

    // Referências
    private Rigidbody rb;

    // Inputs
    private Vector3 movimentacao;
    private Vector2 olhar;

    // Rotação
    private float rotacaoHorizontal;
    private float rotacaoVertical;

    // Estados
    private bool estaNoChao;
    private bool podeDarDash = true;
    private bool estaDandoDash;
    private bool estaRecarregando;

    // Tiro
    private float proximoTiro;

    // Munição
    private int municaoAtual;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        municaoAtual = capacidadeMunicao;
    }


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        // Câmera
        rotacaoHorizontal = olhar.x * sensibilidadeMouse * Time.deltaTime;
        rotacaoVertical -= olhar.y * sensibilidadeMouse * Time.deltaTime;
        rotacaoVertical = Mathf.Clamp(rotacaoVertical, -90.0f, 90.0f);

        transform.Rotate(0f, rotacaoHorizontal, 0f);
        camera.localRotation = Quaternion.Euler(rotacaoVertical, 0f, 0f);

        //Barra de vida 
        BarraVida.value = vida;


        // Pulo
        if (Keyboard.current.spaceKey.wasPressedThisFrame && estaNoChao && !estaDandoDash)
        {
            Pular();
        }


        // Dash
        if (Keyboard.current.leftShiftKey.wasPressedThisFrame && podeDarDash && !estaRecarregando)
        {
            StartCoroutine(Dash());
        }


        // Tiro
        if (Mouse.current.leftButton.wasPressedThisFrame && !estaRecarregando)
        {
            Atirar();
        }


        // Recarga
        if (Keyboard.current.rKey.wasPressedThisFrame && !estaRecarregando && municaoAtual < capacidadeMunicao)
        {
            StartCoroutine(Recarregar());
        }
    }


    private void FixedUpdate()
    {
        // Movimento
        if (estaDandoDash)
        {
            return;
        }

        Vector3 movimento = transform.TransformDirection(movimentacao);
        rb.linearVelocity = new Vector3(movimento.x * velocidade, rb.linearVelocity.y, movimento.z * velocidade);
    }


    private void Pular()
    {
        rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
        estaNoChao = false;
    }


    private System.Collections.IEnumerator Dash()
    {
        podeDarDash = false;
        estaDandoDash = true;

        Vector3 direcaoDash = transform.forward;
        direcaoDash.y = 0f;
        direcaoDash.Normalize();

        float velocidadeVertical = rb.linearVelocity.y;

        rb.linearVelocity = new Vector3(direcaoDash.x * forcaDash, velocidadeVertical, direcaoDash.z * forcaDash);

        yield return new WaitForSeconds(tempoDash);

        estaDandoDash = false;

        yield return new WaitForSeconds(recargaDash);

        podeDarDash = true;
    }


    private void Atirar()
    {
        if (Time.time < proximoTiro)
        {
            return;
        }

        if (municaoAtual <= 0)
        {
            Debug.Log("Munição acabou! Pressione R para recarregar.");
            return;
        }

        proximoTiro = Time.time + intervaloTiro;
        municaoAtual--;

       Municao.text = municaoAtual + "/" + capacidadeMunicao;

        Ray ray = new Ray(camera.position, camera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaTiro))
        {
            Debug.Log("Acertou: " + hit.collider.gameObject.name);

            Inimigo inimigo = hit.collider.GetComponent<Inimigo>();

            if (inimigo != null)
            {
                inimigo.ReceberDano(20.0f);
            }

            InimigoCorpoACorpo inimigoMelee = hit.collider.GetComponent<InimigoCorpoACorpo>();

            if (inimigoMelee != null)
            {
                inimigoMelee.ReceberDano(20.0f);
            }
        }

        Debug.DrawRay(camera.position, camera.forward * distanciaTiro, Color.red, 1.0f);
    }


    private System.Collections.IEnumerator Recarregar()
    {
        estaRecarregando = true;

        Debug.Log("Recarregando...");

        yield return new WaitForSeconds(tempoRecarga);

        municaoAtual = capacidadeMunicao;
        estaRecarregando = false;

        Municao.text = municaoAtual + "/" + capacidadeMunicao;

    }


    public void ReceberDano(float dano)
    {
        vida -= dano;
        Animacao.Dano();
        if(vida <= 25.0f)
        {
            Animacao.Morrendo = true;
        }
        if (vida <= 0)
        {
            Morrer();
        }
    }


    private void Morrer()
    {
        Animacao.death();
    }


    // Chão
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaNoChao = true;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Chao"))
        {
            estaNoChao = false;
        }
    }


    // Movimento
    private void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        movimentacao = new Vector3(input.x, 0f, input.y);
    }


    // Mouse
    private void OnLook(InputValue value)
    {
        olhar = value.Get<Vector2>();
    }
}