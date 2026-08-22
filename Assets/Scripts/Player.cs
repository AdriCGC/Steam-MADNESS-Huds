using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Variáveis
    private Rigidbody rigidbody;
    [SerializeField] private float velocidade = 10.0f;
    [SerializeField] private float sensibilidadeOlhar = 20.0f;
    [SerializeField] private Transform camera;
    private Vector3 movimentacao;
    private Vector2 olhar;
    private float rotacaoHorizontal;
    private float rotacaoVertical;

    // Função para pegar os componentes 
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Função para as Físicas do Jogo
    private void FixedUpdate()
    {
        // Pegando os valores para movimentar a Câmera do Player
        rotacaoHorizontal = olhar.x * sensibilidadeOlhar * Time.deltaTime;
        rotacaoVertical -= olhar.y * sensibilidadeOlhar * Time.deltaTime;

        // Limitando o Movimento na Vertical em 90 graus para cima e para baixo.
        rotacaoVertical = Mathf.Clamp(rotacaoVertical, -90.0f, 90.0f);

        // Movimento do Player
        transform.Translate(movimentacao * velocidade * Time.deltaTime);

        // Movimento da Câmera do Player
        transform.Rotate(0f, rotacaoHorizontal, 0f);
        camera.localRotation = Quaternion.Euler(rotacaoVertical, 0f, 0f);
    }

    // Função que pega os Inputs de Movimentação

    void OnMove(InputValue value)
    {
        // Pega a Movimentacao em X e em Y e converte em um Vector3
        movimentacao = new Vector3(value.Get<Vector2>().x, 0f, value.Get<Vector2>().y);
    }

    // Função para pegar a movimentação da Câmera
    void OnLook(InputValue value)
    {
        olhar = value.Get<Vector2>();
    }
}
