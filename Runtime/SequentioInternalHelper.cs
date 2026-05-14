using System;
using Damdor.Foundation;
using Damdor.Vario;
using UnityEngine.UIElements;

namespace Damdor.Sequentio
{
    internal static class SequentioInternalHelper
    {
        public const string UiDocumentVariableName = "ui-document";
        public const string UiRootVariableName = "ui-root";
        
        public static VisualElement GetRoot(VarioStorage storage)
        {
            if (storage == null) throw new ArgumentException("Storage cannot be null");
            var document = storage.Get<UIDocument>(UiDocumentVariableName);
            if (document == null) throw new ArgumentException($"{UiDocumentVariableName} not found in storage");
            var root = document.rootVisualElement;

            if (storage.Contains<UiToolkitQuery>(UiRootVariableName))
            {
                var query = storage.Get<UiToolkitQuery>(UiRootVariableName);
                root = root.Q<VisualElement>(query);
            }

            return root;
        }

        public static void PutRoot(VarioStorage storage, UIDocument document)
        {
            storage.Update(UiDocumentVariableName, document);
        }
        
        public static void PutRoot(VarioStorage storage, UIDocument document, UiToolkitQuery root)
        {
            PutRoot(storage, document);
            storage.Update(UiRootVariableName, root);
        }
        
    }
}