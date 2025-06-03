using System;
using System.Collections.Generic;
using Orpheus.Core.Orbital.Player;
using UnityEngine;

namespace Orpheus.Core.Rings
{
    public class Floor : MonoBehaviour
    {
        public int floorIndex;
        public float minY;
        public float maxY;
        [SerializeField]public List<Ring> rings;
        [field:SerializeField]
        public GameObject[] EnemiesToSpawn { get; private set; }
        [field:SerializeField]
        public GameObject[] TrapToSpawn { get; private set; }

        public void Awake()
        {
            foreach (var ring in rings)
            { 
                ring.Initialize(this);
            }
        }

        public Ring GetSmallerRing(Ring currentRing)
        {
            if (rings.IndexOf(currentRing) > 0)
            {
                return rings[rings.IndexOf(currentRing) - 1];
            }
            return default;
        }

        public Ring GetBiggerRing(Ring currentRing)
        {
            if (rings.IndexOf(currentRing) < rings.Count - 1)
            {
                return rings[rings.IndexOf(currentRing) + 1];
            }
            return default;
        }

        public void OnTriggerEnter(Collider other)
        {
            PlayerOrbitalController target = other.GetComponent<PlayerOrbitalController>();
            if (target != null)
            {
                GameManager.Instance.SwapFloor(this);
            }
        }
    }
}
