using System;

namespace RemoteAppWeb.Security
{
    /// <summary>
    /// Class that can be used to execute an action when it is disposed of.
    /// </summary>
    public class DisposeNotifier : IDisposable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposeNotifier"/> class.
        /// </summary>
        /// <param name="action">The action to execute when disposed of.</param>
        public DisposeNotifier(Action action)
        {
            _action = action;
        }

        #endregion 

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _action();
            }
            _disposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposeNotifier"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposeNotifier()
        {
            Dispose(false);
        }

        #endregion

        #region Fields

        private bool _disposed;
        private Action _action;

        #endregion
    }
}
