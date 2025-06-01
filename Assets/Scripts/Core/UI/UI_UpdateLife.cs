using Orpheus.Core.FightSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace Orpheus.Core.UI
{
    public class UI_UpdateLife : MonoBehaviour
    {
        [field:SerializeField]
        private IAbilityTarget entity;
        [field:SerializeField]
        private Image UI_Hp;
    }
}