using System.Collections;
using UnityEngine;

namespace PolySpatial.Samples
{
    public class BalloonBehavior : MonoBehaviour
    {
        [SerializeField]
        Material m_WhiteMaterial;

        [SerializeField]
        GameObject m_WhiteParticlePrefab;

        [SerializeField]
        Animator m_Animator;

        MeshRenderer m_MeshRenderer;
        GameObject m_ParticlePrefab;

        public BalloonGalleryManager m_Manager;
        bool m_Popped;

        const int k_WhiteScore = 1;
        const float k_ParticleOffset = 0.05f;
        const float k_PopDelay = 0.12f;
        const string k_PopAnimTrigger = "Pop";

        void Start()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            SetBalloonValues(m_WhiteMaterial, k_WhiteScore, m_WhiteParticlePrefab);
   
        }

        void SetBalloonValues(Material mat, int score, GameObject particlePrefab)
        {
            m_MeshRenderer.material = mat;
            m_ParticlePrefab = particlePrefab;
        }

        public void Pop()
        {
            if (!m_Popped)
            {
                StartCoroutine(PopSequence());
            }
        }

        IEnumerator PopSequence()
        {
            m_Popped = true;
            if (m_Animator != null)
            {
                m_Animator.SetTrigger(k_PopAnimTrigger);
            }
            yield return new WaitForSeconds(k_PopDelay);
            Instantiate(m_ParticlePrefab, transform.position + new Vector3(0, k_ParticleOffset, 0), Quaternion.identity);
            //m_Manager.BalloonPopped();

            Destroy(this.gameObject);
        }
    }
}
