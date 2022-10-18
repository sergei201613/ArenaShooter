using UnityEngine;
using UnityEngine.Assertions;

namespace Unity.FPS.AI
{
    public class FollowPlayer : MonoBehaviour
    {
        Transform m_PlayerTransform;
        Vector3 m_OriginalOffset;

        void Start()
        {
            // TODO: Bad
            m_PlayerTransform = GameObject.FindWithTag("Player").transform;
            Assert.IsNotNull(m_PlayerTransform);
            m_OriginalOffset = transform.position - m_PlayerTransform.position;
        }

        void LateUpdate()
        {
            transform.position = m_PlayerTransform.position + m_OriginalOffset;
        }
    }
}