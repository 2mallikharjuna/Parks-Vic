using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ParksVictoriaApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemFile : IFile
    {
        private readonly string fileName;
        private readonly string directoryPath;
        /// <summary>
        /// Get File exists property
        /// </summary>
        public bool Exists => File.Exists(FullPath);

        /// <summary>
        /// Ready with file info object
        /// </summary>
        public FileInfo FileInfo => new FileInfo(FullPath);

        /// <summary>
        /// SystemFile paramterised constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        public SystemFile(string name, string directory)
        {
            fileName = Path.GetFileName(name);
            directoryPath = directory?? Environment.CurrentDirectory;
        }
        /// <summary>
        /// Gets File path
        /// </summary>
        public string FullPath => Path.Combine(directoryPath, fileName);

        /// <summary>
        /// Gets file stream
        /// </summary>
        public FileStream FileStream
        {
            get
            {
                return FileInfo.OpenRead();
            }
        }
    }
}
