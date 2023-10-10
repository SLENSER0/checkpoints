namespace CheckpointSystem
{
    public class FinalTrack : BaseTrack
    {
        
        public override void ResetTrack()
        {
            _checkpointsManager.CurrentCheckpoint = -1;
            _checkpointsManager.DeactivateAllCheckpointsExceptFirst();
            IsStarted = false;
        }
        
        
        protected override void OnLapComplete()
        {
            if (IsStarted)
            {
                TrackComplete();
            }
            else
            {
                _checkpointsManager.StartTrack();
                IsStarted = true;
            }
            
        }

        
    }
}
