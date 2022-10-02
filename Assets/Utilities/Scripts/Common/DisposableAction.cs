using System;

namespace Utilities.Common
{
    using System;
    public class DisposableAction : IDisposable
    {
        Action action;

        public DisposableAction(Action action)
        {
            this.action = action;
        }

        void IDisposable.Dispose()
        {
            action();
        }
    }
}