using System;
using System.Collections.Generic;
using Damdor.Vario;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Damdor.Sequentio.Tests
{
    public class AdditionalStepTests
    {
        private readonly List<Object> objects = new();
        private readonly List<string> assets = new();

        [TearDown]
        public void TearDown()
        {
            for (var i = objects.Count - 1; i >= 0; i--) Object.DestroyImmediate(objects[i]);
            objects.Clear();
            foreach (var path in assets) AssetDatabase.DeleteAsset(path);
            assets.Clear();
        }

        private T Keep<T>(T value) where T : Object
        {
            objects.Add(value);
            return value;
        }

        private T Component<T>() where T : Component
        {
            var gameObject = Keep(new GameObject(typeof(T).Name));
            var component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }

        private Material Material()
        {
            var shader = Shader.Find("Hidden/Sequentio/MaterialStepTest");
            Assert.That(shader, Is.Not.Null);
            return Keep(new Material(shader));
        }

        private Renderer Renderer(Material material)
        {
            var renderer = Component<MeshRenderer>();
            renderer.sharedMaterial = material;
            return renderer;
        }

        private static void Init<TDefinition>(TDefinition definition, StepPlayer<TDefinition> player)
            where TDefinition : StepDefinition
        {
            player.Setup(definition);
            player.Init(null);
        }

        private static void CheckTween<TDefinition, TTarget, TValue>(TDefinition definition,
            TweenStepPlayer<TDefinition, TTarget, TValue> player, TTarget target,
            TValue start, TValue end, TValue midpoint, Func<TValue> read)
            where TDefinition : TweenStepDefinition<TTarget, TValue>
            where TTarget : class
        {
            var initial = read();
            definition.Reset();
            definition.Target = target;
            definition.Start = new TweenEndpoint<TTarget, TValue> { ValueSource = TweenPointSource.ExplicitValue, Value = start };
            definition.End = new TweenEndpoint<TTarget, TValue> { ValueSource = TweenPointSource.ExplicitValue, Value = end };
            Init(definition, player);
            player.SetProgress(0.5f);
            Assert.That(read(), Is.EqualTo(midpoint));
            player.ReturnToInitialState();
            Assert.That(read(), Is.EqualTo(initial));
            player.Reset();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void MaterialTextureScaleSupportsMainAndCustomProperties(bool custom)
        {
            var material = Material();
            var name = custom ? "_CustomTexture" : "_BaseMap";
            material.SetTextureScale(name, new Vector2(2f, 4f));
            material.SetTextureOffset(name, Vector2.one);
            var definition = new MaterialTextureScaleTweenStepDefinition();
            definition.Reset();
            definition.Target = material;
            definition.Property = new MaterialTextureProperty
            {
                Type = custom ? MaterialTexturePropertyType.Custom : MaterialTexturePropertyType.MainTexture,
                Name = name
            };
            definition.End = new Vector2(6f, 8f);
            var player = new MaterialTextureScaleTweenStepPlayer();
            Init(definition, player);
            player.SetProgress(0.5f);
            Assert.That(material.GetTextureScale(name), Is.EqualTo(new Vector2(4f, 6f)));
            Assert.That(material.GetTextureOffset(name), Is.EqualTo(Vector2.one));
            player.ReturnToInitialState();
            Assert.That(material.GetTextureScale(name), Is.EqualTo(new Vector2(2f, 4f)));
        }

        [Test]
        public void MaterialVectorRespectsAxisFilterAndRestores()
        {
            var material = Material();
            material.SetVector("_Vector", new Vector4(2f, 4f, 6f, 8f));
            var definition = new MaterialVectorTweenStepDefinition();
            definition.Reset();
            definition.Target = material;
            definition.PropertyName = "_Vector";
            definition.End = new Vector4(6f, 8f, 10f, 12f);
            definition.AxisFilter = new Vector4AxisFilter { X = true, Y = false, Z = true, W = false };
            var player = new MaterialVectorTweenStepPlayer();
            Init(definition, player);
            player.SetProgress(0.5f);
            Assert.That(material.GetVector("_Vector"), Is.EqualTo(new Vector4(4f, 4f, 8f, 8f)));
            player.ReturnToInitialState();
            Assert.That(material.GetVector("_Vector"), Is.EqualTo(new Vector4(2f, 4f, 6f, 8f)));
        }

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public void RendererScaleAndVectorRespectMaterialSelection(bool shared, bool custom)
        {
            var material = Material();
            var name = custom ? "_CustomTexture" : "_BaseMap";
            var renderer = Renderer(material);
            var scaleDefinition = new RendererMaterialTextureScaleTweenStepDefinition();
            scaleDefinition.Reset();
            scaleDefinition.Target = renderer;
            scaleDefinition.SharedMaterial = shared;
            scaleDefinition.Property = new MaterialTextureProperty
            {
                Type = custom ? MaterialTexturePropertyType.Custom : MaterialTexturePropertyType.MainTexture,
                Name = name
            };
            scaleDefinition.End = new Vector2(5f, 7f);
            if (!shared)
                LogAssert.Expect(LogType.Error,
                    "Instantiating material due to calling renderer.material during edit mode. This will leak materials into the scene. You most likely want to use renderer.sharedMaterial instead.");
            var scales = new RendererMaterialTextureScaleTweenStepPlayer();
            Init(scaleDefinition, scales);
            var modified = renderer.sharedMaterial;
            if (!shared) Keep(modified);
            Assert.That(ReferenceEquals(modified, material), Is.EqualTo(shared));
            scales.SetProgress(0.5f);
            Assert.That(modified.GetTextureScale(name), Is.EqualTo(new Vector2(3f, 4f)));
            scales.ReturnToInitialState();
            Assert.That(modified.GetTextureScale(name), Is.EqualTo(Vector2.one));

            var vectorDefinition = new RendererMaterialVectorTweenStepDefinition();
            vectorDefinition.Reset();
            vectorDefinition.Target = renderer;
            vectorDefinition.SharedMaterial = shared;
            vectorDefinition.PropertyName = "_Vector";
            vectorDefinition.End = Vector4.one * 8f;
            var vectors = new RendererMaterialVectorTweenStepPlayer();
            Init(vectorDefinition, vectors);
            vectors.SetProgress(0.5f);
            Assert.That(modified.GetVector("_Vector"), Is.EqualTo(Vector4.one * 4f));
            if (!shared) Assert.That(material.GetVector("_Vector"), Is.EqualTo(Vector4.zero));
            vectors.ReturnToInitialState();
            Assert.That(modified.GetVector("_Vector"), Is.EqualTo(Vector4.zero));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PropertyBlockNumericTweensPreserveOtherOverridesAndUseMaterialFallback(bool overridden)
        {
            var material = Material();
            material.SetFloat("_Float", 2f);
            material.SetInt("_Int", 2);
            material.SetVector("_Vector", Vector4.one * 2f);
            material.SetColor("_CustomColor", Color.red);
            var renderer = Renderer(material);
            var block = new MaterialPropertyBlock();
            block.SetFloat("_Sentinel", 99f);
            if (overridden)
            {
                block.SetFloat("_Float", 8f);
                block.SetInt("_Int", 8);
                block.SetVector("_Vector", Vector4.one * 8f);
                block.SetColor("_CustomColor", Color.blue);
            }
            renderer.SetPropertyBlock(block);
            var floatDefinition = new RendererPropertyBlockFloatTweenStepDefinition();
            floatDefinition.Reset();
            floatDefinition.Target = renderer;
            floatDefinition.PropertyName = "_Float";
            floatDefinition.End = 12f;
            var floats = new RendererPropertyBlockFloatTweenStepPlayer();
            Init(floatDefinition, floats);
            var intDefinition = new RendererPropertyBlockIntTweenStepDefinition();
            intDefinition.Reset();
            intDefinition.Target = renderer;
            intDefinition.PropertyName = "_Int";
            intDefinition.End = 12;
            var ints = new RendererPropertyBlockIntTweenStepPlayer();
            Init(intDefinition, ints);
            var vectorDefinition = new RendererPropertyBlockVectorTweenStepDefinition();
            vectorDefinition.Reset();
            vectorDefinition.Target = renderer;
            vectorDefinition.PropertyName = "_Vector";
            vectorDefinition.End = Vector4.one * 12f;
            var vectors = new RendererPropertyBlockVectorTweenStepPlayer();
            Init(vectorDefinition, vectors);
            var colorDefinition = new RendererPropertyBlockColorTweenStepDefinition();
            colorDefinition.Reset();
            colorDefinition.Target = renderer;
            colorDefinition.PropertyName = "_CustomColor";
            colorDefinition.End = Color.green;
            var colors = new RendererPropertyBlockColorTweenStepPlayer();
            Init(colorDefinition, colors);

            floats.SetProgress(0.5f);
            ints.SetProgress(0.5f);
            vectors.SetProgress(0.5f);
            colors.SetProgress(0.5f);
            renderer.GetPropertyBlock(block);
            Assert.That(block.GetFloat("_Float"), Is.EqualTo(overridden ? 10f : 7f));
            Assert.That(block.GetInt("_Int"), Is.EqualTo(overridden ? 10 : 7));
            Assert.That(block.GetVector("_Vector"), Is.EqualTo(Vector4.one * (overridden ? 10f : 7f)));
            Assert.That(block.GetColor("_CustomColor"), Is.EqualTo(Color.Lerp(overridden ? Color.blue : Color.red, Color.green, 0.5f)));
            Assert.That(block.GetFloat("_Sentinel"), Is.EqualTo(99f));
            Assert.That(renderer.sharedMaterial, Is.SameAs(material));
            Assert.That(material.GetFloat("_Float"), Is.EqualTo(2f));
            foreach (var player in new StepPlayer[] { floats, ints, vectors, colors })
            {
                player.ReturnToInitialState();
                renderer.GetPropertyBlock(block);
                Assert.That(block.HasFloat("_Float"), Is.EqualTo(overridden));
                Assert.That(block.GetFloat("_Sentinel"), Is.EqualTo(99f));
            }
        }

        [Test]
        public void PropertyBlockReferenceEndpointAndEmptyBlockRestoration()
        {
            var target = Renderer(Material());
            var reference = Renderer(Material());
            reference.sharedMaterial.SetFloat("_Float", 2f);
            var block = new MaterialPropertyBlock();
            block.SetFloat("_Float", 8f);
            reference.SetPropertyBlock(block);
            var definition = new RendererPropertyBlockFloatTweenStepDefinition();
            definition.Reset();
            definition.Target = target;
            definition.PropertyName = "_Float";
            definition.End = new TweenEndpoint<Renderer, float>
            {
                ValueSource = TweenPointSource.ReferenceTarget, OtherTarget = reference, Value = 2f
            };
            var player = new RendererPropertyBlockFloatTweenStepPlayer();
            Init(definition, player);
            player.SetProgress(0.5f);
            target.GetPropertyBlock(block);
            Assert.That(block.GetFloat("_Float"), Is.EqualTo(5f));
            player.ReturnToInitialState();
            Assert.That(target.HasPropertyBlock(), Is.False);
            reference.GetPropertyBlock(block);
            Assert.That(block.GetFloat("_Float"), Is.EqualTo(8f));
            player.Reset();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PropertyBlockTextureSwitchesAndRestoresOriginalBlock(bool hasInitialTexture)
        {
            var material = Material();
            var initial = hasInitialTexture ? Keep(new Texture2D(1, 1)) : null;
            var next = Keep(new Texture2D(1, 1));
            material.SetTexture("_CustomTexture", initial);
            var renderer = Renderer(material);
            var definition = new SetRendererPropertyBlockTextureStepDefinition
            {
                Target = renderer, PropertyName = "_CustomTexture", Value = next
            };
            var player = new SetRendererPropertyBlockTextureStepPlayer();
            Init(definition, player);
            var block = new MaterialPropertyBlock();
            player.SetProgress(1f);
            renderer.GetPropertyBlock(block);
            Assert.That(block.GetTexture("_CustomTexture"), Is.SameAs(next));
            player.SetProgress(0f);
            Assert.That(renderer.HasPropertyBlock(), Is.False);
            Assert.That(material.GetTexture("_CustomTexture"), Is.EqualTo(initial));
            player.SetProgress(1f);
            player.ReturnToInitialState();
            Assert.That(renderer.HasPropertyBlock(), Is.False);
            Assert.That(material.GetTexture("_CustomTexture"), Is.EqualTo(initial));
        }

        [Test]
        public void LightTweensInterpolateAndRestore()
        {
            var light = Component<Light>();
            light.type = LightType.Spot;
            light.spotAngle = 90f;
            CheckTween(new LightSpotAngleTweenStepDefinition(), new LightSpotAngleTweenStepPlayer(), light,
                40f, 80f, 60f, () => light.spotAngle);
            CheckTween(new LightInnerSpotAngleTweenStepDefinition(), new LightInnerSpotAngleTweenStepPlayer(), light,
                10f, 30f, 20f, () => light.innerSpotAngle);
            CheckTween(new LightShadowStrengthTweenStepDefinition(), new LightShadowStrengthTweenStepPlayer(), light,
                0f, 1f, 0.5f, () => light.shadowStrength);
        }

        [Test]
        public void UIAndTextTweensInterpolateAndRestore()
        {
            var slider = Component<Slider>();
            CheckTween(new SliderValueTweenStepDefinition(), new SliderValueTweenStepPlayer(), slider,
                0f, 1f, 0.5f, () => slider.value);
            var raw = Component<RawImage>();
            CheckTween(new RawImageUVRectTweenStepDefinition(), new RawImageUVRectTweenStepPlayer(), raw,
                new Rect(0f, 0f, 1f, 1f), new Rect(2f, 4f, 3f, 5f), new Rect(1f, 2f, 2f, 3f), () => raw.uvRect);
            var text = Component<TextMeshProUGUI>();
            CheckTween(new TMPTextMaxVisibleCharactersTweenStepDefinition(), new TMPTextMaxVisibleCharactersTweenStepPlayer(), (TMP_Text)text,
                0, 10, 5, () => text.maxVisibleCharacters);
            var scroll = Component<ScrollRect>();
            var content = Component<RectTransform>();
            content.SetParent(scroll.transform, false);
            content.sizeDelta = new Vector2(1000f, 1000f);
            scroll.content = content;
            Canvas.ForceUpdateCanvases();
            CheckTween(new ScrollRectNormalizedPositionTweenStepDefinition(), new ScrollRectNormalizedPositionTweenStepPlayer(), scroll,
                Vector2.zero, Vector2.one, Vector2.one * 0.5f, () => scroll.normalizedPosition);

            var initial = Keep(new Texture2D(1, 1));
            var next = Keep(new Texture2D(1, 1));
            raw.texture = initial;
            var texturePlayer = new SetRawImageTextureStepPlayer();
            Init(new SetRawImageTextureStepDefinition { Target = raw, Value = next }, texturePlayer);
            texturePlayer.SetProgress(1f);
            Assert.That(raw.texture, Is.SameAs(next));
            texturePlayer.ReturnToInitialState();
            Assert.That(raw.texture, Is.SameAs(initial));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void LookAtCapturesPositionAndRestoresWorldRotation(bool transformTarget)
        {
            var target = Component<Transform>();
            target.position = new Vector3(1f, 2f, 3f);
            target.rotation = Quaternion.Euler(10f, 20f, 30f);
            var initial = target.rotation;
            var point = target.position + Vector3.right * 5f;
            var reference = Component<Transform>();
            reference.position = point;
            var definition = new LookAtTweenStepDefinition();
            definition.Reset();
            definition.Target = target;
            definition.LookAtTarget = transformTarget ? reference : null;
            definition.Position = point;
            var player = new LookAtTweenStepPlayer();
            Init(definition, player);
            reference.position = target.position;
            player.SetProgress(0.5f);
            Assert.That(Quaternion.Angle(target.rotation, Quaternion.Slerp(initial, Quaternion.LookRotation(Vector3.right), 0.5f)), Is.LessThan(0.01f));
            player.SetProgress(1f);
            Assert.That(Vector3.Dot(target.forward, Vector3.right), Is.GreaterThan(0.999f));
            player.ReturnToInitialState();
            Assert.That(Quaternion.Angle(target.rotation, initial), Is.LessThan(0.01f));
            player.Reset();
            definition.LookAtTarget = null;
            definition.Position = target.position;
            Init(definition, player);
            player.SetProgress(1f);
            Assert.That(Quaternion.Angle(target.rotation, initial), Is.LessThan(0.01f));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PunchPositionAndRotationRestoreBothCoordinateModes(bool local)
        {
            var parent = Component<Transform>();
            parent.position = Vector3.one * 10f;
            parent.rotation = Quaternion.Euler(0f, 90f, 0f);
            var target = Component<Transform>();
            target.SetParent(parent, false);
            target.localPosition = Vector3.one;
            target.localRotation = Quaternion.Euler(10f, 20f, 30f);
            var initialPosition = target.position;
            var initialRotation = target.rotation;
            var positionDefinition = new PunchPositionStepDefinition();
            positionDefinition.Reset();
            positionDefinition.Target = target;
            positionDefinition.Local = local;
            positionDefinition.Direction = Vector3.right;
            var positions = new PunchPositionStepPlayer();
            Init(positionDefinition, positions);
            positions.SetProgress(0.1f);
            Assert.That(Vector3.Distance(target.position, initialPosition), Is.GreaterThan(0.01f));
            var direction = local ? parent.TransformDirection(Vector3.right) : Vector3.right;
            Assert.That(Vector3.Dot((target.position - initialPosition).normalized, direction), Is.GreaterThan(0.999f));
            positions.SetProgress(1f);
            Assert.That(Vector3.Distance(target.position, initialPosition), Is.LessThan(0.0001f));
            positions.SetProgress(0.1f);
            positions.ReturnToInitialState();
            Assert.That(Vector3.Distance(target.position, initialPosition), Is.LessThan(0.0001f));

            var rotationDefinition = new PunchRotationStepDefinition();
            rotationDefinition.Reset();
            rotationDefinition.Target = target;
            rotationDefinition.Local = local;
            rotationDefinition.Direction = Vector3.up * 30f;
            var rotations = new PunchRotationStepPlayer();
            Init(rotationDefinition, rotations);
            rotations.SetProgress(0.1f);
            Assert.That(Quaternion.Angle(target.rotation, initialRotation), Is.GreaterThan(0.1f));
            rotations.SetProgress(1f);
            Assert.That(Quaternion.Angle(target.rotation, initialRotation), Is.LessThan(0.05f));
            rotations.SetProgress(0.1f);
            rotations.ReturnToInitialState();
            Assert.That(Quaternion.Angle(target.rotation, initialRotation), Is.LessThan(0.05f));
        }

        [Test]
        public void AnimatorParametersInterpolateSwitchAndRestore()
        {
            var path = AssetDatabase.GenerateUniqueAssetPath("Assets/SequentioStepTest.controller");
            assets.Add(path);
            var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
            controller.AddParameter("Float", AnimatorControllerParameterType.Float);
            controller.AddParameter("Int", AnimatorControllerParameterType.Int);
            controller.AddParameter("Bool", AnimatorControllerParameterType.Bool);
            controller.layers[0].stateMachine.AddState("Idle");
            var animator = Component<Animator>();
            animator.runtimeAnimatorController = controller;
            animator.Rebind();
            var floatDefinition = new AnimatorFloatTweenStepDefinition();
            var floatPlayer = new AnimatorFloatTweenStepPlayer();
            floatDefinition.Reset();
            floatDefinition.Target = animator;
            floatDefinition.ParameterName = "Float";
            floatDefinition.End = 8f;
            Init(floatDefinition, floatPlayer);
            floatPlayer.SetProgress(0.5f);
            Assert.That(animator.GetFloat("Float"), Is.EqualTo(4f));
            floatPlayer.ReturnToInitialState();
            Assert.That(animator.GetFloat("Float"), Is.EqualTo(0f));
            var intDefinition = new AnimatorIntTweenStepDefinition();
            intDefinition.Reset();
            intDefinition.Target = animator;
            intDefinition.ParameterName = "Int";
            intDefinition.End = 8;
            var intPlayer = new AnimatorIntTweenStepPlayer();
            Init(intDefinition, intPlayer);
            intPlayer.SetProgress(0.5f);
            Assert.That(animator.GetInteger("Int"), Is.EqualTo(4));
            intPlayer.ReturnToInitialState();
            Assert.That(animator.GetInteger("Int"), Is.EqualTo(0));
            var boolPlayer = new SetAnimatorBoolStepPlayer();
            Init(new SetAnimatorBoolStepDefinition { Target = animator, ParameterName = "Bool", Value = true }, boolPlayer);
            boolPlayer.SetProgress(1f);
            Assert.That(animator.GetBool("Bool"), Is.True);
            boolPlayer.SetProgress(0f);
            Assert.That(animator.GetBool("Bool"), Is.False);
            boolPlayer.SetProgress(1f);
            boolPlayer.ReturnToInitialState();
            Assert.That(animator.GetBool("Bool"), Is.False);
        }
    }
}
