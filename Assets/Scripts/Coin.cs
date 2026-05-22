using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip m_CoinSound;

    private float m_Time = 0f;
    private bool m_Grabbed = false;
    private Vector3 m_StartPosition;

    private void Start()
    {
        m_StartPosition = transform.position;
    }

    private void Update()
    {
        if (m_Grabbed)
        {
            return;
        }

        transform.Rotate(Vector3.up, 180 * Time.deltaTime);

        float yOffset = Mathf.Sin(m_Time * 270 * Mathf.Deg2Rad) * 0.2f;
        transform.position = m_StartPosition + new Vector3(0f, yOffset, 0f);

        m_Time += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Grab(other);
    }

    private void Grab(Collider other)
    {
        if (m_Grabbed)
        {
            return;
        }

        Player player = other.GetComponent<Player>();
        if (player == null)
        {
            return;
        }

        player.CollectCoin();

        if (m_CoinSound != null)
        {
            Audio.Instance.Play(m_CoinSound);
        }

        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Stop();

        m_Grabbed = true;
    }
}
