using UnityEditor;

namespace Damdor.Sequentio.Editor
{
    /// <summary>
    /// Draws a main or custom material color property selector.
    /// </summary>
    [CustomPropertyDrawer(typeof(MaterialColorProperty))]
    public class MaterialColorPropertyDrawer : MaterialPropertyDrawer
    {
        protected override int CustomTypeValue => (int)MaterialColorPropertyType.Custom;
    }
}
