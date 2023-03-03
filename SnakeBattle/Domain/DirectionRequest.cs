using System.Threading.Tasks;

namespace Domain
{
    public class DirectionRequest
    {
        private readonly Task<Direction> getNextDirectionTask;
        public bool DirectionIsUsed { get; set; }
        public bool IsTaskCompleted => getNextDirectionTask.IsCompleted;
        public bool IsTaskCompletedSuccessfully => getNextDirectionTask.IsCompletedSuccessfully;
        public Direction Direction => getNextDirectionTask.Result;

        public DirectionRequest(Task<Direction> getNextDirectionTask)
        {
            this.getNextDirectionTask = getNextDirectionTask;
        }
    }
}
