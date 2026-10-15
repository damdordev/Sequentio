using Microsoft.CodeAnalysis;

namespace Damdor.Sequentio.CodeGenerator
{
    [Generator]
    public class SequentioSettingsCodeGenerator : ClassRegisterViaAttributeCodeGenerator
    {
        public const string SequentioStepAttribute = "SequentioStep";

        public override string[] Attributes { get; } = { SequentioStepAttribute };

        public override void Execute(GeneratorExecutionContext context)
        {
            GenerateForAttribute(context, SequentioStepAttribute, (sb, type) =>
            {
                sb.AppendLine($"        RegisterStep(typeof({type}), typeof({type}).GetCustomAttribute<Damdor.Sequentio.SequentioStepAttribute>());");
            }, sb =>
            {
                sb.AppendLine("    private static void RegisterStep(System.Type type, Damdor.Sequentio.SequentioStepAttribute attribute)");
                sb.AppendLine("    {");
                sb.AppendLine($"        global::Damdor.Sequentio.SequentioSettings.RegisterStepType(type, attribute.Name, attribute.Icon);");
                
                sb.AppendLine("    }");
            });
        }

        
        
    }
}