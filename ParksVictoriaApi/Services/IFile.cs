using Microsoft.Extensions.Logging;
using System.IO;

namespace ParksVictoriaApi.Services
{
    public interface IFile
    {
        bool Exists { get; }
        string FullPath { get; }
        FileStream FileStream { get; } 
    }
}