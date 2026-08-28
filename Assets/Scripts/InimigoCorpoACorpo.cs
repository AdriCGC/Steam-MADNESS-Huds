using UnityEngine;
using UnityEngine.AI;

public class InimigoCorpoACorpo : MonoBehaviour
{
    // Vida
    [SerializeField] private float vida = 100.0f;
    [SerializeField] private float tempoDano = 0.1f;
    [SerializeField] private Renderer rendererInimigo;

    // Detecção
    [SerializeField] private float distanciaVisao = 20.0f;
    [SerializeField] private float distanciaAtaque = 2.0f;

    // Ataque
    [SerializeField] private float dano = 20.0f;
    [SerializeField] private float intervaloAtaque = 1.0f;

    private NavMeshAgent agente;
    private Transform jogador;
    private Material material;
    private Color corOriginal;
    private float proximoAtaque;


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
            if (distancia > distanciaAtaque)
            {
                Perseguir();
            }
            else
            {
                Atacar();
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


    private void Atacar()
    {
        agente.isStopped = true;

        Vector3 direcao = jogador.position - transform.position;
        direcao.y = 0f;

        if (direcao != Vector3.zero)
        {
            Quaternion rotacao = Quaternion.LookRotation(direcao);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacao, Time.deltaTime * 10.0f);
        }

        if (Time.time < proximoAtaque)
        {
            return;
        }

        proximoAtaque = Time.time + intervaloAtaque;

        Player player = jogador.GetComponent<Player>();

        if (player != null)
        {
            player.ReceberDano(dano);
        }
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


    public void ReceberDano(float danoRecebido)
    {
        vida -= danoRecebido;

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