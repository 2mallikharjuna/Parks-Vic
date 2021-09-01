using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ParksVictoriaApi.Services
{
    public interface IFileService : IDisposable
    {
        public Task<Byte[]> GetBytesArrayFromFile(IFile file, CancellationToken cancellationToken = default);
    }
}
