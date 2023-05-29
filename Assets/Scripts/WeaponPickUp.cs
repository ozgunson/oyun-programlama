using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
        }
    }
}