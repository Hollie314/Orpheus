using System;
using DG.Tweening;
using Orpheus.Core.Orbital.Player;
using UnityEngine;

namespace Orpheus.Core
{
    public class PlayerYPositionTracker : MonoBehaviour
    {
        [field: SerializeField] private GameObject player;

        private void Update()
        {
            Vector3 postion = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            this.transform.DOMove(postion,0.5f);
        }
    }
}