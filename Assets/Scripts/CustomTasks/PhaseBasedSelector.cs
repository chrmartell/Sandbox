using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class PhaseBasedSelector : Composite
    {
        public SharedInt CurrentPhase;
        public List<string> IncludedTasksPerStage = new List<string>();
        //[Tooltip("Seed the random number generator to make things easier to debug")]
        public int seed = 0;
        //[Tooltip("Do we want to use the seed?")]
        public bool useSeed = false;

        public int visualBossPhase;

        // A list of indexes of every child task. This list is used by the Fischer-Yates shuffle algorithm.
        private List<int> childIndexList = new List<int>();
        // The random child index execution order.
        private Stack<int> childrenExecutionOrder = new Stack<int>();
        // The task status of the last child ran.
        private TaskStatus executionStatus = TaskStatus.Inactive;

        public override void OnAwake()
        {
            // If specified, use the seed provided.
            if (useSeed) {
                Random.InitState(seed);
            }

            /*childIndexList.Clear();
            for (int i = 0; i < children.Count; ++i) {
                childIndexList.Add(i);
            }*/
        }

        public override void OnStart()
        {
            // Select considered child indices based on the current stage
            childIndexList.Clear();
            string[] tasks = IncludedTasksPerStage[CurrentPhase.Value].Split(',');
            for (int i = 0; i < tasks.Length; i++) {
                childIndexList.Add(int.Parse(tasks[i]));
            }
            //childIndexList = IncludedTasksPerStage[CurrentPhase.Value].Split(',').Select(int.Parse).ToList();

            visualBossPhase = CurrentPhase.Value;

            // Randomize the indecies
            ShuffleChilden();
        }

        public override int CurrentChildIndex()
        {
            // Peek will return the index at the top of the stack.
            return childrenExecutionOrder.Peek();
        }

        public override bool CanExecute()
        {
            // Continue exectuion if no task has return success and indexes still exist on the stack.
            return childrenExecutionOrder.Count > 0 && executionStatus != TaskStatus.Success;
        }

        public override void OnChildExecuted(TaskStatus childStatus)
        {
            // Pop the top index from the stack and set the execution status.
            if (childrenExecutionOrder.Count > 0) {
                childrenExecutionOrder.Pop();
            }
            executionStatus = childStatus;
        }

        public override void OnConditionalAbort(int childIndex)
        {
            // Start from the beginning on an abort
            childrenExecutionOrder.Clear();
            executionStatus = TaskStatus.Inactive;
            ShuffleChilden();
        }

        public override void OnEnd()
        {
            // All of the children have run. Reset the variables back to their starting values.
            executionStatus = TaskStatus.Inactive;
            childrenExecutionOrder.Clear();
        }

        public override void OnReset()
        {
            // Reset the public properties back to their original values
            seed = 0;
            useSeed = false;
        }

        private void ShuffleChilden()
        {
            // Use Fischer-Yates shuffle to randomize the child index order.
            for (int i = childIndexList.Count; i > 0; --i) {
                int j = Random.Range(0, i);
                int index = childIndexList[j];
                childrenExecutionOrder.Push(index);
                childIndexList[j] = childIndexList[i - 1];
                childIndexList[i - 1] = index;
            }
        }
    }
}