using System;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Associates a step definition with its player type and editor menu metadata.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SequentioStepAttribute : Attribute
    {
        /// <summary>
        /// The hierarchical editor menu path for the step.
        /// </summary>
        public string Path { get; }
        /// <summary>
        /// The display name of the step in the editor.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// The editor asset path of the step icon, which may be null.
        /// </summary>
        public string Icon { get; }
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// The step player type associated with the attributed definition.
        /// </summary>
        public Type PlayerType { get; }

        /// <summary>
        /// Creates step registration metadata for a player type and editor entry.
        /// </summary>
        /// <param name="playerType">The step player type associated with this definition.</param>
        /// <param name="path">The hierarchical menu path used in the step picker.</param>
        /// <param name="name">The display name shown for the step.</param>
        /// <param name="icon">The optional editor asset path of the step icon.</param>
        public SequentioStepAttribute(Type playerType, string path, string name, string icon = null)
        {
            PlayerType = playerType;
            Path = path;
            Name = name;
            Icon = icon;
        }
    }
}