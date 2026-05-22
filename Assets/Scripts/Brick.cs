using UnityEngine;

public class Brick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Collider m_MainCollider;
    [SerializeField] private Collider m_BottomDetector;
    [SerializeField] private Renderer m_MeshRenderer;
    [SerializeField] private ParticleSystem m_Particles;
    [SerializeField] private AudioClip m_BreakSound;

    private bool m_Exploded = false;


    public void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        if (m_Exploded)
        {
            return;
        }

        if (m_BreakSound != null)
        {
            Audio.Instance.Play(m_BreakSound);
        }

        if (m_Particles != null)
        {
            m_Particles.Play();
        }

        if (m_MeshRenderer != null)
        {
            m_MeshRenderer.enabled = false;
        }

        if (m_MainCollider != null)
        {
            m_MainCollider.enabled = false;
        }

        if (m_BottomDetector != null)
        {
            m_BottomDetector.enabled = false;
        }

        m_Exploded = true;
    }
}
