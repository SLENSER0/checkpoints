namespace CheckpointSystem
{
    public class FinalTrack : BaseTrack
    {
        protected override void OnLapComplete()
        {
            OnTrackComplete();
        }

        protected override void ActivateNextCheckpoint()
        {
            DeactivateCurrentCheckpointAndActivateNext();
            
            if(_currentCheckpoint == _getLastCheckpoint && _isStarted)
            {
                OnLapComplete();
            }

            StartTrack();

        }
    
    }
}