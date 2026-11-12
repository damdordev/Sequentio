namespace Damdor.Sequentio
{
     /// <summary>
     /// Describes the current playback state of a sequence.
     /// </summary>
     public enum SequencePlayerState
     {
         /// <summary>
         /// Has no active execution.
         /// </summary>
         Idle,
         
         /// <summary>
         /// Advances playback on updates.
         /// </summary>
         Playing,
         
         /// <summary>
         /// Retains the execution without advancing time.
         /// </summary>
         Paused,
         
         /// <summary>
         /// Has reached the end of non-looping playback.
         /// </summary>
         Finished
     }
}