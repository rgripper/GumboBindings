using System;

namespace Gumbo.Wrappers
{
    internal class DisposalAwareLazyFactory
    {
        private readonly Func<bool> _IsDisposed;

        private readonly string _ObjectName;

        public DisposalAwareLazyFactory(Func<bool> isDisposed, string objectName)
        {
            if (isDisposed == null)
            {
                throw new ArgumentNullException(nameof(isDisposed));
            }

            if (objectName == null)
            {
                throw new ArgumentNullException(nameof(objectName));
            }

            _IsDisposed = isDisposed;
            _ObjectName = objectName;
        }

        public Lazy<T> Create<T>(Func<T> factoryMethod)
        {
            return new Lazy<T>(() =>
            {
                if (_IsDisposed())
                {
                    throw new ObjectDisposedException(_ObjectName);
                }

                return factoryMethod();
            });
        }
    }
}
