using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                throw new ArgumentNullException("isDisposed");
            }

            if (objectName == null)
            {
                throw new ArgumentNullException("objectName");
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
