using UnityEngine;

namespace Damdor.Sequentio
{
    /// <summary>
    /// Reads renderer-level overrides with a shared-material fallback and preserves unrelated values on writes.
    /// Returning to the initial state restores the entire renderer-level block captured by this step.
    /// </summary>
    internal sealed class RendererPropertyBlockState
    {
        private Renderer target;
        private MaterialPropertyBlock initialBlock;
        private MaterialPropertyBlock workingBlock;

        public void Capture(Renderer renderer)
        {
            target = renderer;
            initialBlock ??= new MaterialPropertyBlock();
            workingBlock ??= new MaterialPropertyBlock();
            target.GetPropertyBlock(initialBlock);
        }

        public void Restore() => target.SetPropertyBlock(initialBlock.isEmpty ? null : initialBlock);

        public void Reset()
        {
            target = null;
            initialBlock?.Clear();
            workingBlock?.Clear();
        }

        private MaterialPropertyBlock Read(Renderer renderer)
        {
            renderer.GetPropertyBlock(workingBlock);
            return workingBlock;
        }

        public Color GetColor(Renderer renderer, int id)
        {
            var block = Read(renderer);
            if (block.HasColor(id)) return block.GetColor(id);
            var material = renderer.sharedMaterial;
            return material != null ? material.GetColor(id) : default;
        }

        public float GetFloat(Renderer renderer, int id)
        {
            var block = Read(renderer);
            if (block.HasFloat(id)) return block.GetFloat(id);
            var material = renderer.sharedMaterial;
            return material != null ? material.GetFloat(id) : default;
        }

        public int GetInt(Renderer renderer, int id)
        {
            var block = Read(renderer);
            // Unity's SetInt/GetInt APIs store legacy integer properties as floats, like Material.SetInt/GetInt.
            if (block.HasFloat(id)) return block.GetInt(id);
            var material = renderer.sharedMaterial;
            return material != null ? material.GetInt(id) : default;
        }

        public Vector4 GetVector(Renderer renderer, int id)
        {
            var block = Read(renderer);
            if (block.HasVector(id)) return block.GetVector(id);
            var material = renderer.sharedMaterial;
            return material != null ? material.GetVector(id) : default;
        }

        public Texture GetTexture(Renderer renderer, int id)
        {
            var block = Read(renderer);
            if (block.HasTexture(id)) return block.GetTexture(id);
            var material = renderer.sharedMaterial;
            return material != null ? material.GetTexture(id) : null;
        }

        public void SetColor(Renderer renderer, int id, Color value)
        {
            var block = Read(renderer);
            block.SetColor(id, value);
            renderer.SetPropertyBlock(block);
        }

        public void SetFloat(Renderer renderer, int id, float value)
        {
            var block = Read(renderer);
            block.SetFloat(id, value);
            renderer.SetPropertyBlock(block);
        }

        public void SetInt(Renderer renderer, int id, int value)
        {
            var block = Read(renderer);
            block.SetInt(id, value);
            renderer.SetPropertyBlock(block);
        }

        public void SetVector(Renderer renderer, int id, Vector4 value)
        {
            var block = Read(renderer);
            block.SetVector(id, value);
            renderer.SetPropertyBlock(block);
        }

        public void SetTexture(Renderer renderer, int id, Texture value)
        {
            var block = Read(renderer);
            block.SetTexture(id, value);
            renderer.SetPropertyBlock(block);
        }
    }
}
