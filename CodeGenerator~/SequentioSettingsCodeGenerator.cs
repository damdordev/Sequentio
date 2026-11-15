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
        public const string RunSequenceInChildrenGetterAttribute = "RunSequenceInChildrenGetter";
        public const string RunSequenceInChildrenFilterAttribute = "RunSequenceInChildrenFilter";
        public const string RunSequenceInChildrenSorterAttribute = "RunSequenceInChildrenSorter";
        public const string RunSequenceInChildrenSchedulerAttribute = "RunSequenceInChildrenScheduler";

        /// <inheritdoc />
        public override string[] Attributes { get; } =
        {
            SequentioStepAttribute,
            RunSequenceInChildrenGetterAttribute,
            RunSequenceInChildrenFilterAttribute,
            RunSequenceInChildrenSorterAttribute,
            RunSequenceInChildrenSchedulerAttribute
        };

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
            
            GenerateForAttribute(context, RunSequenceInChildrenGetterAttribute, (sb, type, attrData) =>
            {
                sb.AppendLine($"        Register<{type}>(typeof({type}).GetCustomAttribute<Damdor.Sequentio.RunSequenceInChildrenGetterAttribute>());");
            }, sb =>
            {
                sb.AppendLine("    private static void Register<T>(Damdor.Sequentio.RunSequenceInChildrenGetterAttribute attribute)");
                sb.AppendLine("        where T : Damdor.Sequentio.BaseRunSequenceInChildrenGetter, new()");
                sb.AppendLine("    {");
                sb.AppendLine($"        global::Damdor.Sequentio.RunSequenceInChildrenSettings.Getters.Register<T>(attribute.Name);");
                
                sb.AppendLine("    }");
            });
            
            GenerateForAttribute(context, RunSequenceInChildrenFilterAttribute, (sb, type, attrData) =>
            {
                sb.AppendLine($"        Register<{type}>(typeof({type}).GetCustomAttribute<Damdor.Sequentio.RunSequenceInChildrenFilterAttribute>());");
            }, sb =>
            {
                sb.AppendLine("    private static void Register<T>(Damdor.Sequentio.RunSequenceInChildrenFilterAttribute attribute)");
                sb.AppendLine("        where T : Damdor.Sequentio.BaseRunSequenceInChildrenFilter, new()");
                sb.AppendLine("    {");
                sb.AppendLine($"        global::Damdor.Sequentio.RunSequenceInChildrenSettings.Filters.Register<T>(attribute.Name);");
                
                sb.AppendLine("    }");
            });
            
            GenerateForAttribute(context, RunSequenceInChildrenSorterAttribute, (sb, type, attrData) =>
            {
                sb.AppendLine($"        Register<{type}>(typeof({type}).GetCustomAttribute<Damdor.Sequentio.RunSequenceInChildrenSorterAttribute>());");
            }, sb =>
            {
                sb.AppendLine("    private static void Register<T>(Damdor.Sequentio.RunSequenceInChildrenSorterAttribute attribute)");
                sb.AppendLine("        where T : Damdor.Sequentio.BaseRunSequenceInChildrenSorter, new()");
                sb.AppendLine("    {");
                sb.AppendLine($"        global::Damdor.Sequentio.RunSequenceInChildrenSettings.Sorters.Register<T>(attribute.Name);");
                
                sb.AppendLine("    }");
            });
            
            GenerateForAttribute(context, RunSequenceInChildrenSchedulerAttribute, (sb, type, attrData) =>
            {
                sb.AppendLine($"        Register<{type}>(typeof({type}).GetCustomAttribute<Damdor.Sequentio.RunSequenceInChildrenSchedulerAttribute>());");
            }, sb =>
            {
                sb.AppendLine("    private static void Register<T>(Damdor.Sequentio.RunSequenceInChildrenSchedulerAttribute attribute)");
                sb.AppendLine("        where T : Damdor.Sequentio.BaseRunSequenceInChildrenScheduler, new()");
                sb.AppendLine("    {");
                sb.AppendLine($"        global::Damdor.Sequentio.RunSequenceInChildrenSettings.Schedulers.Register<T>(attribute.Name);");
                
                sb.AppendLine("    }");
            });
        }

        
        
    }
}