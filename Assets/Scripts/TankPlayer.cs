using Photon.Pun;
using UnityEngine;

namespace Tanks
{
    public class TankPlayer : MonoBehaviourPunCallbacks
    {
        private Complete.TankMovement mMovement;

        private Complete.TankShooting mShooting;

        private void Awake()
        {
            mMovement = GetComponent<Complete.TankMovement>();
            mShooting = GetComponent<Complete.TankShooting>();

            if (!photonView.IsMine)
            {
                Debug.Log("true");
                mMovement.enabled = false;
                mShooting.enabled = false;
                enabled = false;
            }
        }
    }

}
