namespace Damdor.Sequentio
{
    /// <summary>
    /// Receives elapsed time from the shared sequence update provider.
    /// </summary>
    public interface ISequentioUpdateReceiver
    {
        /// <summary>
        /// Advances sequence playback by the supplied elapsed time.
        /// </summary>
        /// <param name="deltaTime">The elapsed time in seconds.</param>
        void Update(float deltaTime);
    }
}