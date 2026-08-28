using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    // Vida
    [SerializeField] private float vida = 100.0f;
    [SerializeField] private float tempoDano = 0.1f;
    [SerializeField] private Renderer rendererInimigo;

    // Detecção
    [SerializeField] private float distanciaVisao = 20.0f;
    [SerializeField] private float distanciaTiro = 12.0f;

    // Tiro
    [SerializeField] private GameObject projetil;
    [SerializeField] private Transform pontoTiro;
    [SerializeField] private float intervaloTiro = 1.5f;
    [SerializeField] private float forcaProjetil = 15.0f;

    private NavMeshAgent agente;
    private Transform jogador;
    private Material material;
    private Color corOriginal;
    private float proximoTiro;


    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        material = rendererInimigo.material;
        corOriginal = material.color;
    }


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            jogador = player.transform;
        }
    }


    private void Update()
    {
        if (jogador == null)
        {
            return;
        }

        float distancia = Vector3.Distance(transform.position, jogador.position);

        if (distancia <= distanciaVisao && PodeVerJogador())
        {
            Perseguir();

            if (distancia <= distanciaTiro)
            {
                Atirar();
            }
        }
        else
        {
            Parar();
        }
    }


    private void Perseguir()
    {
        agente.isStopped = false;
        agente.SetDestination(jogador.position);

        Vector3 direcao = jogador.position - transform.position;
        direcao.y = 0f;

        if (direcao != Vector3.zero)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 5.0f);
        }
    }


    private void Parar()
    {
        agente.isStopped = true;
    }


    private bool PodeVerJogador()
    {
        Vector3 origem = transform.position + Vector3.up;
        Vector3 direcao = (jogador.position + Vector3.up) - origem;

        if (Physics.Raycast(origem, direcao.normalized, out RaycastHit hit, distanciaVisao))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }


    private void Atirar()
    {
        if (Time.time < proximoTiro)
        {
            return;
        }

        proximoTiro = Time.time + intervaloTiro;

        GameObject novoProjetil = Instantiate(projetil, pontoTiro.position, pontoTiro.rotation);

        Rigidbody rbProjetil = novoProjetil.GetComponent<Rigidbody>();

        if (rbProjetil != null)
        {
            Vector3 direcao = (jogador.position - pontoTiro.position).normalized;
            rbProjetil.linearVelocity = direcao * forcaProjetil;
        }
    }


    public void ReceberDano(float dano)
    {
        vida -= dano;

        Debug.Log("Vida do inimigo: " + vida);

        if (vida <= 0)
        {
            Morrer();
            return;
        }

        StopAllCoroutines();
        StartCoroutine(EfeitoDano());
    }


    private System.Collections.IEnumerator EfeitoDano()
    {
        material.color = Color.red;

        yield return new WaitForSeconds(tempoDano);

        material.color = corOriginal;
    }


    private void Morrer()
    {
        Destroy(gameObject);
    }
}