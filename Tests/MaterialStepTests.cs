using System.Collections.Generic;
using Damdor.Vario;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Damdor.Sequentio.Tests
{
    public class MaterialStepTests
    {
        private readonly List<Object> objects = new();
        private VarioStorage storage;

        [SetUp]
        public void SetUp()
        {
            storage = VarioStorage.Create();
            storage.Update("colorName", "_CustomColor");
            storage.Update("textureName", "_CustomTexture");
            storage.Update("shared", true);
        }

        [TearDown]
        public void TearDown()
        {
            for (var i = objects.Count - 1; i >= 0; i--) Object.DestroyImmediate(objects[i]);
            objects.Clear();
            storage.Release();
        }

        private T Keep<T>(T value) where T : Object
        {
            objects.Add(value);
            return value;
        }

        private Material CreateMaterial()
        {
            var shader = Shader.Find("Hidden/Sequentio/MaterialStepTest");
            Assert.That(shader, Is.Not.Null);
            return Keep(new Material(shader));
        }

        private Renderer CreateRenderer(Material material)
        {
            var renderer = Keep(new GameObject("Material step test")).AddComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            return renderer;
        }

        private TPlayer Init<TDefinition, TPlayer>(TDefinition definition)
            where TDefinition : StepDefinition
            where TPlayer : StepPlayer<TDefinition>, new()
        {
            var player = new TPlayer();
            player.Setup(definition);
            player.Init(storage, new ExecutionContext());
            return player;
        }

        private static MaterialColorProperty ColorProperty(bool custom) => new()
        {
            Type = custom ? MaterialColorPropertyType.Custom : MaterialColorPropertyType.MainColor,
            Name = VarioValue<string>.FromStorage(custom ? "colorName" : "unusedName")
        };

        private static MaterialTextureProperty TextureProperty(bool custom) => new()
        {
            Type = custom ? MaterialTexturePropertyType.Custom : MaterialTexturePropertyType.MainTexture,
            Name = VarioValue<string>.FromStorage(custom ? "textureName" : "unusedName")
        };

        [TestCase(false)]
        [TestCase(true)]
        public void MaterialColorUsesSelectedPropertyAndReferenceEndpoint(bool custom)
        {
            var target = CreateMaterial();
            var reference = CreateMaterial();
            var name = custom ? "_CustomColor" : "_Tint";
            target.SetColor(name, Color.red);
            reference.SetColor(name, Color.blue);
            var definition = new MaterialColorTweenStepDefinition();
            definition.Reset();
            definition.Target = target;
            definition.Property = ColorProperty(custom);
            definition.End = new TweenEndpoint<Material, Color>
            {
                ValueSource = TweenPointSource.ReferenceTarget,
                OtherTarget = reference,
                Value = Color.clear
            };
            var player = Init<MaterialColorTweenStepDefinition, MaterialColorTweenStepPlayer>(definition);

            player.SetProgress(0.5f);
            Assert.That(target.GetColor(name), Is.EqualTo(Color.LerpUnclamped(Color.red, Color.blue, 0.5f)));
            player.ReturnToInitialState();
            Assert.That(target.GetColor(name), Is.EqualTo(Color.red));
            Assert.That(reference.GetColor(name), Is.EqualTo(Color.blue));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void MaterialTextureOffsetUsesSelectedPropertyAndRestoresIt(bool custom)
        {
            var target = CreateMaterial();
            var name = custom ? "_CustomTexture" : "_BaseMap";
            target.SetTextureOffset(name, Vector2.one);
            target.SetTextureScale(name, new Vector2(2f, 3f));
            var definition = new MaterialTextureOffsetTweenStepDefinition();
            definition.Reset();
            definition.Target = target;
            definition.Property = TextureProperty(custom);
            definition.End = new Vector2(5f, 7f);
            var player = Init<MaterialTextureOffsetTweenStepDefinition, MaterialTextureOffsetTweenStepPlayer>(definition);

            player.SetProgress(0.5f);
            Assert.That(target.GetTextureOffset(name), Is.EqualTo(new Vector2(3f, 4f)));
            Assert.That(target.GetTextureScale(name), Is.EqualTo(new Vector2(2f, 3f)));
            player.ReturnToInitialState();
            Assert.That(target.GetTextureOffset(name), Is.EqualTo(Vector2.one));
        }

        [Test]
        public void MaterialNumericTweensInterpolateAndRestore()
        {
            var target = CreateMaterial();
            target.SetFloat("_Float", 2f);
            target.SetInt("_Int", 2);
            var floatDefinition = new MaterialFloatTweenStepDefinition();
            floatDefinition.Reset();
            floatDefinition.Target = target;
            floatDefinition.PropertyName = "_Float";
            floatDefinition.End = 6f;
            var intDefinition = new MaterialIntTweenStepDefinition();
            intDefinition.Reset();
            intDefinition.Target = target;
            intDefinition.PropertyName = "_Int";
            intDefinition.End = 6;
            var floats = Init<MaterialFloatTweenStepDefinition, MaterialFloatTweenStepPlayer>(floatDefinition);
            var ints = Init<MaterialIntTweenStepDefinition, MaterialIntTweenStepPlayer>(intDefinition);

            floats.SetProgress(0.5f);
            ints.SetProgress(0.5f);
            Assert.That(target.GetFloat("_Float"), Is.EqualTo(4f));
            Assert.That(target.GetInt("_Int"), Is.EqualTo(4));
            floats.ReturnToInitialState();
            ints.ReturnToInitialState();
            Assert.That(target.GetFloat("_Float"), Is.EqualTo(2f));
            Assert.That(target.GetInt("_Int"), Is.EqualTo(2));
        }

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void RendererColorRespectsMaterialSelection(bool shared, bool custom)
        {
            var original = CreateMaterial();
            var name = custom ? "_CustomColor" : "_Tint";
            original.SetColor(name, Color.red);
            var renderer = CreateRenderer(original);
            var definition = new RendererMaterialColorTweenStepDefinition();
            definition.Reset();
            definition.Target = renderer;
            definition.Property = ColorProperty(custom);
            // Leaving the default verifies that material instances are the default behavior.
            if (shared) definition.SharedMaterial = VarioValue<bool>.FromStorage("shared");
            if (!shared)
                LogAssert.Expect(LogType.Error,
                    "Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.");
            var player = Init<RendererMaterialColorTweenStepDefinition, RendererMaterialColorTweenStepPlayer>(definition);
            var modified = renderer.sharedMaterial;
            if (!shared) Keep(modified);
            Assert.That(ReferenceEquals(modified, original), Is.EqualTo(shared));
            definition.End = Color.blue;
            // Reinitialize so the new endpoint is captured, also exercising pooled player reuse.
            player.Reset();
            player.Setup(definition);
            player.Init(storage, new ExecutionContext());
            player.SetProgress(1f);

            Assert.That(modified.GetColor(name), Is.EqualTo(Color.blue));
            Assert.That(original.GetColor(name), Is.EqualTo(shared ? Color.blue : Color.red));
            player.ReturnToInitialState();
            Assert.That(modified.GetColor(name), Is.EqualTo(Color.red));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void MaterialTextureImmediateSwitchesAndRestores(bool custom)
        {
            var target = CreateMaterial();
            var initial = Keep(new Texture2D(1, 1));
            var next = Keep(new Texture2D(1, 1));
            var name = custom ? "_CustomTexture" : "_BaseMap";
            target.SetTexture(name, initial);
            var definition = new SetMaterialTextureStepDefinition
            {
                Target = target, Property = TextureProperty(custom), Value = next
            };
            var player = Init<SetMaterialTextureStepDefinition, SetMaterialTextureStepPlayer>(definition);

            player.SetProgress(1f);
            Assert.That(target.GetTexture(name), Is.SameAs(next));
            player.SetProgress(0f);
            Assert.That(target.GetTexture(name), Is.SameAs(initial));
            player.SetProgress(1f);
            player.ReturnToInitialState();
            Assert.That(target.GetTexture(name), Is.SameAs(initial));
        }

        [Test]
        public void MaterialKeywordAndShaderRestoreTheirInitialValues()
        {
            var target = CreateMaterial();
            var initialShader = target.shader;
            var nextShader = Shader.Find("Hidden/InternalErrorShader");
            Assert.That(nextShader, Is.Not.Null);
            var keywords = Init<SetMaterialKeywordStepDefinition, SetMaterialKeywordStepPlayer>(
                new SetMaterialKeywordStepDefinition { Target = target, Keyword = "SEQUENTIO_TEST", Value = true });
            keywords.SetProgress(1f);
            Assert.That(target.IsKeywordEnabled("SEQUENTIO_TEST"), Is.True);
            keywords.ReturnToInitialState();
            Assert.That(target.IsKeywordEnabled("SEQUENTIO_TEST"), Is.False);

            var shaders = Init<SetMaterialShaderStepDefinition, SetMaterialShaderStepPlayer>(
                new SetMaterialShaderStepDefinition { Target = target, Value = nextShader });
            shaders.SetProgress(1f);
            Assert.That(target.shader, Is.SameAs(nextShader));
            shaders.ReturnToInitialState();
            Assert.That(target.shader, Is.SameAs(initialShader));
        }
    }
}
