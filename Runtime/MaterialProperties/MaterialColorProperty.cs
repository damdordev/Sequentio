using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Selects a material color property, with a storage-resolved name for custom properties.
    /// </summary>
    [Serializable]
    public struct MaterialColorProperty
    {
        /// <summary>
        /// The material property to use.
        /// </summary>
        public MaterialColorPropertyType Type
        {
            get => type;
            set => type = value;
        }
        
        /// <summary>
        /// The shader property name used when Type is Custom.
        /// </summary>
        public VarioValue<string> Name
        {
            get => name;
            set => name = value;
        }

        [SerializeField] private MaterialColorPropertyType type;
        [SerializeField] private VarioValue<string> name;
    }
}
