using System;

namespace Components
{
    public class ProgresslessOperation
    {
        public ProgresslessOperation(string title, (Action<Action> subscribe, Action<Action> unsubscribe) startSubscription, bool hideWholeProgress = true)
        {
            this.Title = title;

            this.StartSubscription = startSubscription;
            this.HideWholeProgress = hideWholeProgress;
            this.HideWholeProgress = hideWholeProgress;
        }

        public string Title { get; }

        public bool HideWholeProgress { get; }

        public (Action<Action> subscribe, Action<Action> unsubscribe) StartSubscription { get; }
    }
}
