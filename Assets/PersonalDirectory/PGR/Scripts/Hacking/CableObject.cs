using System.Collections;
using UnityEngine;

namespace PGR
{
    public class CableObject : CustomGrabInteractable
    {
        [Header("Cable Object Parameters")]
        [SerializeField] Transform playerTransform;
        [SerializeField] Light pointLight;
        [SerializeField] IHackable hackingTarget;

        protected override void Awake()
        {
            base.Awake();
            pointLight = GetComponent<Light>();

            Priority = 10;
            pointLight.enabled = false;
        }

        public void FixExit()
        {
            pointLight.enabled = false;
        }

        public void ReadyToShot()
        {
            pointLight.enabled = true;
        }

        public void ShotCable(Vector3 shotDirection, float shotPower)
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            rb.AddForce(shotDirection * shotPower, ForceMode.Impulse);
        }

        void OnTriggerEnter(Collider other)
        {
            if (hackingTarget != null)
                return;

            hackingTarget = other.GetComponent<IHackable>();
            if (hackingTarget == null)
                return;

            hackingTarget.Hack();
            HackingPuzzle puzzle = GameManager.Resource.Instantiate<HackingPuzzle>("Hacking/HackingPuzzle", playerTransform.position, Quaternion.identity);
            puzzle.InitialPuzzle(hackingTarget, 2, 3);
        }
    }
}
