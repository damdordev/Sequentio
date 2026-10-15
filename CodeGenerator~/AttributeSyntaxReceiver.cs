using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Damdor.Sequentio.CodeGenerator;

public class AttributeSyntaxReceiver : ISyntaxReceiver
{
    private class AttributeData
    {
        public string Attribute;
        public string AttributeName2;
        public string AttributeEnding1;
        public string AttributeEnding2;

        public List<ClassDeclarationSyntax> Classes = new();
    }
    
    public Dictionary<string, List<ClassDeclarationSyntax>> CandidateClasses { get; }
    
    private readonly List<AttributeData> attributes = new();

    public AttributeSyntaxReceiver(params string[] attributes)
    {
        CandidateClasses = new Dictionary<string, List<ClassDeclarationSyntax>>(attributes.Length);
        foreach (var attribute in attributes)
        {
            var data = new AttributeData
            {
                Attribute = attribute,
                AttributeName2 = attribute + "Attribute",
                AttributeEnding1 = "." + attribute,
                AttributeEnding2 = "." + attribute + "Attribute"
            };
            this.attributes.Add(data);
            CandidateClasses.Add(attribute, data.Classes);
        }
    }

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if(syntaxNode is not ClassDeclarationSyntax classSyntax) return;
        if (classSyntax.AttributeLists.Count == 0) return;

        foreach (var attributeList in classSyntax.AttributeLists)
        {
            foreach (var attribute in attributeList.Attributes)
            {
                var attrName = attribute.Name switch
                {
                    SimpleNameSyntax simple => simple.Identifier.ValueText,
                    _ => attribute.Name.ToString()
                }; 
                
                foreach (var data in attributes)
                {
                    if (!Accept(attrName, data)) continue;
                    if (data.Classes.Count > 0 && data.Classes[data.Classes.Count - 1] == classSyntax) continue;
                    data.Classes.Add(classSyntax);
                }
            }
        }

    }
    
    private static bool Accept(string attrName, AttributeData data) =>
        attrName == data.Attribute ||
        attrName == data.AttributeName2 ||
        attrName.EndsWith(data.AttributeEnding1) ||
        attrName.EndsWith(data.AttributeEnding2);
}