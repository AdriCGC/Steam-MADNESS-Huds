using UnityEngine;

public class ProjetilInimigo : MonoBehaviour
{
    [SerializeField] private float dano = 20.0f;
    [SerializeField] private float tempoVida = 5.0f;


    private void Start()
    {
        Destroy(gameObject, tempoVida);
    }


    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.ReceberDano(dano);
        }

        Destroy(gameObject);
    }
}