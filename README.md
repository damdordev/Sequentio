# Sequentio

Sequentio is a tween library for Unity for building parameterized animations in the Inspector or in code.

- **Parameters:** expose targets, durations, values, and other options that programmers can set through typed storage.
- **Editor:** make and preview animations directly in the Inspector.
- **Scene sequences:** author animations directly on scene objects.
- **Sequence assets:** save reusable animations as assets and apply them to multiple objects.
- **Sequences from code:** build and play animations at runtime.
- **Extensible steps:** write your own steps and combine them with the built-in step library.
- **Reduced allocations:** extensive caching and pooling reuse internal players, plans, executions, settings, and storage to reduce garbage collection pressure.

## Table of Contents

- [Installation](#installation)
- [Core concepts](#core-concepts)
  - [Vario](#vario)
  - [StepDefinition](#stepdefinition)
  - [StepPlayer (step runner)](#stepplayer-step-runner)
  - [ISequenceDefinition](#isequencedefinition)
  - [ISequencePlayer](#isequenceplayer)
- [Usage](#usage)
  - [Creating sequence](#creating-sequence)
    - [Creating simple animation of the scene](#creating-simple-animation-of-the-scene)
    - [Creating sequence asset](#creating-sequence-asset)
    - [Creating sequence from code](#creating-sequence-from-code)
    - [Creating sequence from code based on SequenceAsset](#creating-sequence-from-code-based-on-sequenceasset)
  - [Running sequence](#running-sequence)
    - [Running via code](#running-via-code)
    - [Running automatically](#running-automatically)
    - [Delayed running](#delayed-running)
    - [Settings](#settings)
    - [Error handling](#error-handling)
  - [TweenEndpoint](#tweenendpoint)
  - [Creating custom steps](#creating-custom-steps)
    - [Tween steps](#tween-steps)
    - [Immediate steps](#immediate-steps)
    - [General step](#general-step)
    - [Creating attributes](#creating-attributes)
- [Advanced](#advanced)
  - [SequenceCompiler](#sequencecompiler)
  - [SequenceExecutor](#sequenceexecutor)

## Installation

Sequentio is currently under development. It is available through Unity Package Manager (UPM) using this Git address:

```text
git@github.com:damdordev/Sequentio.git
```

Install Git and configure SSH access to GitHub before using this address.

**Via Package Manager**

1. Open **Window > Package Manager** in Unity.
2. Click **+ > Add package from git URL...**
3. Enter `git@github.com:damdordev/Sequentio.git` and click **Add**.

**Via manifest.json**

Add the package to the `dependencies` object in your project's `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.damdor.sequentio": "git@github.com:damdordev/Sequentio.git"
  }
}
```

Keep your existing dependencies. The package also depends on Vario. If Vario is unavailable through your configured registries, add their Git sources to the same dependencies object:

```json
"com.damdor.vario": "git@github.com:damdordev/Vario.git",
```

## Core concepts

### Vario

Sequentio uses the parameter system from [Vario](https://github.com/damdordev/Vario). Each sequence definition has a `VarioStorage` containing named, typed parameters. Step fields use `VarioValue<T>`, which can hold a constant or reference a parameter in storage. For example, a position step can get its target from a `Transform` parameter named `target`.
 
### StepDefinition

A `StepDefinition` is a serializable object describing one tween or another action. It contains configuration rather than runtime state. Definitions specify options such as whether the step is enabled, its delay, and whether it runs alongside the previous step. Concrete definitions add targets, duration, endpoints, and other parameters.

### StepPlayer (step runner)

`StepPlayer` is the runtime counterpart of a step definition: it resolves parameters, captures initial values, applies progress, and restores values when requested. This is the step runner role; the current API calls it `StepPlayer`, rather than `StepRunner`. Definitions and step players are plain C# objects, not Unity components.

### ISequenceDefinition

`ISequenceDefinition` describes a sequence's ordered steps and parameter storage. `SequencePlayer`, `SequenceAsset`, and `RuntimeSequence` implement this interface. Definitions can be compiled into a timed execution plan.

### ISequencePlayer

`ISequencePlayer` exposes the runtime controls and state of a sequence, including `Play()`, `Pause()`, `Stop()`, `Init()`, duration, and current time. `SequencePlayer`, `SequenceAssetPlayer`, and `RuntimeSequence` implement this interface.

## Usage

### Creating sequence

#### Creating simple animation of the scene

The easiest way to create an animation is to put its definition directly on a scene object:

1. Select a GameObject and use **Add Component** to add `SequencePlayer`.
2. In its Inspector, click **+** in **Steps** and select **Transform > Position**. The step is displayed as **Move**.
3. Select the step and assign the `Transform` you want to animate to **Target**, using a raw value.
4. Set **Local** to `true` to animate local position and **Duration** to `1` second.
5. Expand **Start** and set **Source** to `CurrentValue` with **Shift** `(0, 0, 0)`.
6. Expand **End** and set **Source** to `CurrentValue` with **Shift** `(100, 0, 0)`.
7. Click play button 

The target moves 100 units along its local X axis over one second. Both endpoints are resolved from the target's position when the step initializes.

#### Creating sequence asset

A `SequenceAsset` stores an animation separately from the scene:

- One asset can be applied to many objects by supplying different parameter values.
- Only one shared definition is needed; runtime step players and execution data are pooled, reducing repeated definition allocations.

To create the same position animation as a reusable asset:

1. In the Project window, select **Create > Damdor > Sequntio > Sequence asset**. (`Sequntio` is the current menu spelling.)
2. In the asset Inspector, add a **Transform > Position** step.
3. Set **Local** to `true`, **Duration** to `1`, **Start > Source** to `CurrentValue` with zero shift, and **End > Source** to `CurrentValue` with **Shift** `(100, 0, 0)`.
4. The asset has no scene object to move. In **Storage**, add a parameter named `target` of type `Transform`. Leave its value unassigned in the asset.
5. In the step's **Target** field, choose the Vario source `Storage` and select the `target` parameter.
6. On a GameObject in the scene, add `SequenceAssetPlayer`.
7. Assign the asset to its **Asset** field.
8. In **Storage Override**, add a `Transform` parameter named `target` and assign the scene object's transform. The name and type must match the asset parameter.
9. Press Play button

Your reusable animation is ready. Add more asset players with different `target` overrides to animate other objects with the same definition.

#### Creating sequence from code

Use `RuntimeSequence.Create()` to obtain a sequence, then add definitions with `AddStep<T>()`:

```csharp
using Damdor.Sequentio;
using UnityEngine;

public sealed class MoveFromCode : MonoBehaviour
{
    [SerializeField] private Transform target;
    private RuntimeSequence sequence;

    private void Start()
    {
        sequence = RuntimeSequence.Create();
        sequence.Storage.Update("target", target);

        var step = sequence.AddStep<PositionTweenStepDefinition>();
        step.Target = Damdor.Vario.VarioValue<Transform>.FromStorage("target");
        step.Local = true;
        step.Duration = 1f;
        step.InitOnStart = true;
        step.Start = new TweenEndpoint<Transform, Vector3>
        {
            ValueSource = TweenPointSource.CurrentValue,
            Value = Vector3.zero
        };
        step.End = new TweenEndpoint<Transform, Vector3>
        {
            ValueSource = TweenPointSource.CurrentValue,
            Value = new Vector3(100f, 0f, 0f)
        };

        sequence.Play();
    }

    private void OnDestroy()
    {
        sequence?.Release();
        sequence = null;
    }
}
```

Call `Release()` when the `RuntimeSequence` is no longer needed, even if playback has finished. It unregisters updates and returns pooled resources for reuse. Do not use the sequence after releasing it.

#### Creating sequence from code based on SequenceAsset

Use the position asset created above, whose step reads the `target` parameter and moves it by `(100, 0, 0)`.

```csharp
using Damdor.Sequentio;
using UnityEngine;

public sealed class MoveFromAsset : MonoBehaviour
{
    [SerializeField] private SequenceAsset asset;
    [SerializeField] private Transform someTarget;
    private RuntimeSequence sequence;

    private void Start()
    {
        sequence = RuntimeSequence.Create(asset);
        sequence.Play();
    }

    private void OnDestroy()
    {
        sequence?.Release();
        sequence = null;
    }
}
```

For a sequence created with `RuntimeSequence.Create()`, set parameters directly with `sequence.Storage.Update("target", someTarget)` before playback. For an asset-backed sequence, `sequence.Storage` refers to the source asset's storage. Use the callback above for per-instance overrides so multiple players can share the asset safely.

### Running sequence

#### Running via code

All sequence players expose the same controls through `ISequencePlayer`:

```csharp
using Damdor.Sequentio;

public static class SequenceControls
{
    public static void Play(ISequencePlayer sequence) => sequence.Play();
    public static void Pause(ISequencePlayer sequence) => sequence.Pause();
    public static void Stop(ISequencePlayer sequence) => sequence.Stop();
}
```

`Play()` starts playback, resumes a paused sequence, or restarts a finished sequence. `Pause()` preserves the current position. `Stop()` releases the current execution and uses the configured stop behavior. You can override that behavior with `Stop(StopBehaviour.LeaveInCurrentState)` or `Stop(StopBehaviour.ReturnToInitialState)`.

Configure steps, storage, and settings before starting playback: compilation evaluates timings and clones storage and settings for the execution.

#### Running automatically

The automatic running option is `Settings.AutoPlay` (**Auto Play** in the Inspector). Set it on `SequencePlayer` or `SequenceAssetPlayer`:

| AutoPlayMode | When playback starts |
| --- | --- |modify
| `None` | Only when started explicitly. |
| `Awake` | During the component's `Awake` callback. |
| `Start` | During the component's `Start` callback. |
| `OnEnable` | Each time the component is enabled. |

`RuntimeSequence` has no Unity lifecycle callbacks; call `Play()` explicitly.

#### Delayed running

Loading and displaying a lot of data can cause a stutter on the first frame of an entrance animation. Initialize the sequence first, let a frame pass, then start playback:

```csharp
using Cysharp.Threading.Tasks;
using Damdor.Sequentio;

public static class DelayedSequencePlayback
{
    public static async UniTask PlayNextFrame(ISequencePlayer someSequence)
    {
        someSequence.Init();
        await UniTask.NextFrame();
        someSequence.Play();
    }
}
```

This example requires [UniTask](https://github.com/Cysharp/UniTask). Disable automatic playback when controlling the start this way.

`Init()` creates the execution and pauses it. Enable **Init On Start** on steps that should immediately apply their starting values: these steps establish the first-frame pose during initialization, keeping the transition seamless while you wait. Steps without that option initialize when their scheduled start is evaluated.

#### Settings

`SequenceSettings` contains the following options:

| Setting | Meaning |
| --- | --- |
| `AutoPlay` | Selects the automatic playback callback described above. Defaults to `None`. |
| `Loop` | `None` plays once; `Loop` wraps to the opposite end; `PingPong` reverses direction at each end. |
| `DefaultStopBehaviour` | `ReturnToInitialState` restores target values captured when steps initialized; `LeaveInCurrentState` keeps their current values. This controls parameterless `Stop()`. |
| `Timescale` | `Normal` uses Unity's scaled delta time; `Unscaled` uses unscaled delta time, allowing playback while `Time.timeScale` is zero. |
| `Speed` | Multiplies elapsed playback time. `1` is normal speed, `2` is twice as fast, and `0.5` is half speed. Defaults to `1`. |
| `Direction` | The initial direction: `Forward` starts at the beginning and advances; `Reverse` starts at the end and moves backward. |

Defaults are no looping, normal timescale, forward direction, and restoring initial values on stop. `Direction` specifies the initial direction; `PingPong` changes the active direction during playback.

#### Error handling

`SequentioSettings.LogStepErrors` controls whether step exceptions are logged to the Unity Console; it defaults to `true`. `SequentioSettings.StepErrorHandler` receives error reports independently of that logging setting.

Each report contains the operation (`ErrorSource`), `Definition`, `Player` when available, and `Exception`. Reported operations include duration calculation, player creation, initialization, applying progress, and restoring initial values.

```csharp
using Damdor.Sequentio;
using UnityEngine;

public static class SequenceErrorReporting
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Configure()
    {
        SequentioSettings.LogStepErrors = false;
        SequentioSettings.StepErrorHandler = HandleError;
    }

    private static void HandleError(SequentioStepErrorHandlerData data)
    {
        Debug.LogError(
            $"Sequence step {data.Definition.GetType().Name} failed during {data.ErrorSource}");
        Debug.LogException(data.Exception);
    }
}
```

The handler is global. Keep it free of exceptions and release any scene references it captures when they are no longer needed. Step errors are reported without automatically stopping the entire sequence.

### TweenEndpoint

Many tween steps use `TweenEndpoint<TComponent, TValue>` for their start and end values. This struct represents a single endpoint that can be an exact value or a value relative to a target.

| Inspector Source | Position example | Result |
| --- | --- | --- |
| `ExplicitValue` | **Value** = `(100, 0, 0)` | The endpoint is exactly `(100, 0, 0)`. |
| `CurrentValue` | **Shift** = `(100, 0, 0)` | The endpoint is the animated target's position plus `(100, 0, 0)`. |
| `ReferenceTarget` | **Other target** = a reference transform, **Shift** = `(100, 0, 0)` | The endpoint is the reference transform's position plus `(100, 0, 0)`. |

For position tweens, **Local** determines whether these positions use local or world coordinates. Endpoints are evaluated when the step initializes; a reference target is sampled at that time rather than followed continuously.

In code, the source is `ValueSource`, the explicit value or shift is `Value`, and the reference target is `OtherTarget`:

```csharp
using Damdor.Sequentio;
using UnityEngine;

public static class PositionEndpoints
{
    public static TweenEndpoint<Transform, Vector3> Exact() => new()
    {
        ValueSource = TweenPointSource.ExplicitValue,
        Value = new Vector3(100f, 0f, 0f)
    };

    public static TweenEndpoint<Transform, Vector3> Relative() => new()
    {
        ValueSource = TweenPointSource.CurrentValue,
        Value = new Vector3(100f, 0f, 0f)
    };

    public static TweenEndpoint<Transform, Vector3> FromReference(Transform referenceTarget) => new()
    {
        ValueSource = TweenPointSource.ReferenceTarget,
        OtherTarget = referenceTarget,
        Value = new Vector3(100f, 0f, 0f)
    };
}
```

Both `Value` and `OtherTarget` are Vario values, so they can also reference storage parameters. Assigning a value directly to an endpoint, such as `step.End = new Vector3(100f, 0f, 0f)`, creates an `ExplicitValue` endpoint.

### Creating custom steps

Create both a `StepDefinition` and a matching `StepPlayer<TDefinition>`. The definition stores serialized parameters and calculates duration; the player implements runtime behavior.

Mark the definition with `[Serializable]` and `[SequentioStep(typeof(MyPlayer), "Menu/Path", "Display name")]`. The bundled code generator registers the definition/player pair and its step-picker metadata automatically. Use the short attribute spelling `[SequentioStep(...)]` as shown in the examples. Concrete players need a public parameterless constructor; the implicit constructor is sufficient.

The helper base classes below cover common cases. Because players are pooled, override `Reset()` to clear any additional state or references your player retains, calling `base.Reset()` too.

#### Tween steps

`TweenStepDefinition<TComponent, TValue>` supplies a target, duration, easing, and two endpoints. `TweenStepPlayer<TDefinition, TComponent, TValue>` evaluates them and interpolates between the endpoint values using the easing curve. It also captures and restores the initial value.

Here is a simple local-position tween without axis filtering:

```csharp
using System;
using Damdor.Sequentio;
using UnityEngine;

[Serializable]
[SequentioStep(typeof(LocalPositionStepPlayer), "Custom/Local Position", "Local Position")]
public sealed class LocalPositionStepDefinition : TweenStepDefinition<Transform, Vector3>
{
}

public sealed class LocalPositionStepPlayer
    : TweenStepPlayer<LocalPositionStepDefinition, Transform, Vector3>
{
    protected override Vector3 GetValue(Transform component) => component.localPosition;

    protected override void SetValue(Transform component, Vector3 value)
        => component.localPosition = value;
}
```

The base class uses Vario's numeric operations for addition and interpolation. After scripts compile, the step appears under **Custom > Local Position**. When adding it from code, call `Reset()` before configuring its target and endpoints.

#### Immediate steps

`ValueImmediateStepDefinition<TComponent, TValue>` describes an assignment with a duration of zero seconds. Its matching player captures the initial value and applies the configured value when the step executes. It also supports restoring the initial value.

This example changes `UnityEngine.UI.Text.text`:

```csharp
using System;
using Damdor.Sequentio;
using UnityEngine.UI;

[Serializable]
[SequentioStep(typeof(SetTextStepPlayer), "Custom/Text", "Set Text")]
public sealed class SetTextStepDefinition : ValueImmediateStepDefinition<Text, string>
{
}

public sealed class SetTextStepPlayer
    : ValueImmediateStepPlayer<SetTextStepDefinition, Text, string>
{
    protected override string GetValue(Text target) => target.text;

    protected override void SetValue(Text target, string value) => target.text = value;
}
```

Set **Target** to the text component and **Value** to the desired string. For TextMesh Pro, Sequentio already includes `SetTMPTextTextStepDefinition`.

#### General step

For behavior beyond the helpers, derive directly from `StepDefinition` and `StepPlayer<TDefinition>`. Implement duration calculation on the definition and progress handling on the player.

This one-second step simply logs its current normalized progress:

```csharp
using System;
using Damdor.Sequentio;
using Damdor.Vario;
using UnityEngine;

[Serializable]
[SequentioStep(typeof(LogProgressStepPlayer), "Custom/Log Progress", "Log Progress")]
public sealed class LogProgressStepDefinition : StepDefinition
{
    [SerializeField, SequenceStepParameterGroup("Logging", 0)]
    private VarioValue<float> duration = 1f;

    public VarioValue<float> Duration
    {
        get => duration;
        set => duration = value;
    }

    public override float CalculateDuration(VarioStorage storage) => duration.Evaluate(storage);

    public override void Reset()
    {
        base.Reset();
        duration = 1f;
    }
}

public sealed class LogProgressStepPlayer : StepPlayer<LogProgressStepDefinition>
{
    public override void SetProgress(float t)
    {
        Debug.Log($"Progress: {t:P0}");
    }
}
```

Progress runs from `0` to `1` during forward playback and decreases during reverse playback. Logging every update is useful for demonstration, but allocates strings. For steps that modify targets, also implement `Init()` to capture initial state and `ReturnToInitialState()` to restore it.

#### Creating attributes

Custom steps can expose their own serialized fields. Prefer `VarioValue<T>` for configurable parameters so callers can supply either constants or storage references. Resolve them with `Evaluate(storage)` during duration calculation or player initialization.

For example, the general step above exposes `Duration` as a Vario value:

```csharp
var sequence = RuntimeSequence.Create();
sequence.Storage.Update("logDuration", 2f);

var step = sequence.AddStep<LogProgressStepDefinition>();
step.Reset();
step.Duration = VarioValue<float>.FromStorage("logDuration");
sequence.Play();

// Retain the sequence and call sequence.Release() when it is no longer needed.
```

`SequenceStepParameterGroupAttribute` organizes serialized fields in the step Inspector. Its first argument is the group name; the optional second argument controls ordering within that group, with lower numbers first. Add fields like these inside a custom definition:

```csharp
[SerializeField, SequenceStepParameterGroup("Appearance", 0)]
private VarioValue<Color> color = Color.white;

[SerializeField, SequenceStepParameterGroup("Appearance", 10)]
private VarioValue<float> intensity = 1f;
```

You can also use the existing group names `SequenceStepParameterGroups.General` and `SequenceStepParameterGroups.Motion`. Fields without a group attribute appear in **Other**. Grouping controls Inspector presentation; your player still needs to evaluate and use the fields.

## Advanced

### SequenceCompiler

`SequenceCompiler` compiles an `ISequenceDefinition` into a `SequencePlan`. Compilation evaluates enabled flags, delays, and durations, groups parallel steps, and calculates start times, end times, and the total duration. The plan contains cloned storage and settings, making it useful for inspecting future execution without playing the animation.

```csharp
// definition is an ISequenceDefinition; settings is a SequenceSettings instance.
var plan = SequenceCompiler.CreateDefinitionPlan(definition, settings);
try
{
    Debug.Log($"Duration: {plan.Duration}");
    foreach (var step in plan.Steps)
        Debug.Log($"Step {step.OriginalIndex}: {step.StartTime}–{step.EndTime}");
}
finally
{
    plan.Release();
}
```

To run a plan, create a `SequenceExecution`. This data object contains mutable playback state and a runtime player for each successfully compiled step. Its settings and storage are cloned from the plan, and its players are separate instances, so one plan can produce many independent executions.

```csharp
var plan = SequenceCompiler.CreateDefinitionPlan(definition, settings);
try
{
    var execution = SequenceCompiler.CreateExecution(plan);
    try
    {
        Debug.Log($"Players: {execution.Steps.Count}, duration: {execution.Duration}");
        // Inspect execution.Storage, execution.Settings, and execution.Steps here.
    }
    finally
    {
        execution.Release();
    }
}
finally
{
    plan.Release();
}
```

Definitions and referenced Unity objects remain shared: cloning storage does not clone scene objects. To animate different targets, set parameters on each execution's storage before creating its executor. Parameters affecting compiled timing should instead be supplied through the optional `CreateDefinitionPlan` callback before compilation.

### SequenceExecutor

`SequenceExecutor` takes a `SequenceExecution` and manages one invocation of the sequence. It initializes steps, advances time, applies progress, handles speed, direction, and looping, and can restore captured initial values.

Unlike `RuntimeSequence`, the executor does not register itself for Unity updates. If you use it directly, call `Update(dt)` from your own update loop, passing scaled or unscaled delta time as appropriate. `Update` returns `SequenceExecutorUpdateResult.Finished` when non-looping playback finishes. `SetTime(time)` seeks within the sequence.

```csharp
// Create once, before entering your update loop.
var plan = SequenceCompiler.CreateDefinitionPlan(definition, settings);
var execution = SequenceCompiler.CreateExecution(plan);
plan.Release();

// Set per-execution target overrides before initialization, if needed:
// execution.Storage.Update("target", someTarget);
var executor = SequenceExecutor.Create(execution);

// Call each frame from your update loop:
// var result = executor.Update(Time.deltaTime);

// When this invocation is no longer needed:
executor.ReturnToInitialState(); // Optional: restore captured target values.
executor.Release();
execution.Release();
```

Release both the executor and the execution when finished. Releasing the executor alone does not release its execution. For typical gameplay code, `RuntimeSequence`, `SequencePlayer`, and `SequenceAssetPlayer` manage compilation, execution, and updates for you.
