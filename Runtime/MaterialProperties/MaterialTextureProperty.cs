using System;
using Damdor.Vario;
using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Selects a material texture property, with a storage-resolved name for custom properties.
    /// </summary>
    [Serializable]
    public struct MaterialTextureProperty
    {
        /// <summary>
        /// The material property to use.
        /// </summary>
        public MaterialTexturePropertyType Type
        {
            get => type;
            set => type = value;
        }

        [SerializeField] private MaterialTexturePropertyType type;
        [SerializeField] private VarioValue<string> name;

        /// <summary>
        /// The shader property name used when Type is Custom.
        /// </summary>
        public VarioValue<string> Name
        {
            get => name;
            set => name = value;
        }
    }
}
