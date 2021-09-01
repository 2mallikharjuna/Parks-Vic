using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ParksVictoriaApi.Services
{
	/// <summary>
	/// File Service implementation
	/// </summary>
    public class FileSystemService : IFileService
    {
		private readonly ILogger<IFileService> _logger;

		/// <summary>
		/// Semaphone object to access file resource
		/// </summary>
		public static SemaphoreSlim ReadingSemaphoreSlim = new SemaphoreSlim(1, 1);

		/// <summary>
		/// FileSystemService constructor
		/// </summary>
		/// <param name="logger"></param>
		public FileSystemService(ILogger<IFileService> logger)
        {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}
		/// <summary>
		/// Dispose the unmanged objects
		/// </summary>
		public void Dispose()
        {
			ReadingSemaphoreSlim.Dispose();
		}
		/// <summary>
		/// Read the file content and return the Bytes array
		/// </summary>
		/// <param name="file"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
        public async Task<byte[]> GetBytesArrayFromFile(IFile file, CancellationToken cancellationToken = default)
        {
			byte[] buffer = null;
			try
			{
				string fullFilePath = file.FullPath;

				//allowing one user to access the file at a time to read
				await ReadingSemaphoreSlim.WaitAsync(cancellationToken);
				
				//Check file and file path exists
				if (string.IsNullOrEmpty(fullFilePath) || !file.Exists)
					throw new FileNotFoundException("Invalid file or directory");

				//Reading the file stream
				using (FileStream fs = file.FileStream)
				{
					buffer = new byte[fs.Length];
                    await fs.ReadAsync(buffer, 0, (int)fs.Length); //Asynchoronus file reading - Fastest way and support more than 2GB
				}
				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw new Exception(ex.Message);
			}
			finally
			{
				ReadingSemaphoreSlim.Release();
			}
			return buffer;
		}
    }
}
