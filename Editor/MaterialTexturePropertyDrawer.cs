using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws a main or custom material texture property selector.
    /// </summary>
    [CustomPropertyDrawer(typeof(MaterialTextureProperty))]
    public class MaterialTexturePropertyDrawer : MaterialPropertyDrawer
    {
        protected override int CustomTypeValue => (int)MaterialTexturePropertyType.Custom;
    }
}
