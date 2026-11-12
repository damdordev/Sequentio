using System.Linq;
using Microsoft.CodeAnalysis;

namespace Damdor.Sequentio.CodeGenerator
{
    /// <summary>
    /// Generates initialization code that registers attributed Sequentio steps.
    /// </summary>
    [Generator]
    public class SequentioSettingsCodeGenerator : ClassRegisterViaAttributeCodeGenerator
    {
        /// <summary>
        /// The short attribute name used to discover Sequentio step definitions.
        /// </summary>
        public const string SequentioStepAttribute = "SequentioStep";

        /// <inheritdoc />
        public override string[] Attributes { get; } = { SequentioStepAttribute };

        /// <inheritdoc />
        public override void Execute(GeneratorExecutionContext context)
        {
            GenerateForAttribute(context, SequentioStepAttribute, (sb, type, attrData) =>
            {
                sb.AppendLine($"        RegisterStep<{type}, {(attrData.ConstructorArguments[0].Value as ITypeSymbol).ToDisplayString()}>(typeof({type}).GetCustomAttribute<Damdor.Sequentio.SequentioStepAttribute>());");
            }, sb =>
            {
                sb.AppendLine("    private static void RegisterStep<TStepDefinition, TStepPlayer>(Damdor.Sequentio.SequentioStepAttribute attribute)");
                sb.AppendLine("        where TStepDefinition : Damdor.Sequentio.StepDefinition");
                sb.AppendLine("        where TStepPlayer : Damdor.Sequentio.StepPlayer<TStepDefinition>, new()");
                sb.AppendLine("    {");
                sb.AppendLine($"        global::Damdor.Sequentio.SequentioSettings.RegisterStep<TStepDefinition, TStepPlayer>(attribute.Path, attribute.Name, attribute.Icon);");
                
                sb.AppendLine("    }");
            });
        }

        
        
    }
}