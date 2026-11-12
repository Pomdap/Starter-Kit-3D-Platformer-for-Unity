using UnityEngine;

public class PlatformFalling : MonoBehaviour
{
    [SerializeField] private AudioClip m_PlatformSound;

    private bool m_Falling = false;
    private float m_FallVelocity = 0f;

    private Vector3 m_StartPosition;

    private void Start()
    {
        m_StartPosition = transform.position;
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, delta * 10f);

        if (m_Falling)
        {
            m_FallVelocity += 15f * delta;
            transform.position -= new Vector3(0f, m_FallVelocity * delta, 0f);
        }
        else
        {
            m_FallVelocity = 0f;
        }

        if (transform.position.y < (m_StartPosition.y - 10f))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Fall();
    }

    private void Fall()
    {
        if (m_Falling)
        {
            return;
        }

        if (m_PlatformSound != null)
        {
            Audio.Instance.Play(m_PlatformSound);
        }

        transform.localScale = new Vector3(1.25f, 1f, 1.25f);
        m_Falling = true;
    }
}
