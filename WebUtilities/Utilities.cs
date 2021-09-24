using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebUtilities
{
    /// <summary>
    /// Class with some utility methods.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Allows using a Cancellation Token as if it were a task.
        /// From https://github.com/docevaad/Anchor/blob/master/Tortuga.Anchor/Tortuga.Anchor.source/shared/TaskUtilities.cs
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that can be canceled, but never completed.</returns>
        public static Task AsTask(this CancellationToken cancellationToken)
        {
            TaskCompletionSource<object>? tcs = new TaskCompletionSource<object>();
            cancellationToken.Register(() => tcs.TrySetCanceled(), false);
            return tcs.Task;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="uri"></param>
        /// <param name="targetFilePath"></param>
        /// <param name="reportRate"></param>
        /// <returns></returns>
        [Obsolete("Use DownloadContainers instead.")]
        public static DownloadWithProgress CreateDownloadWithProgress(this IWebClient client, Uri uri, string targetFilePath, int reportRate = 50) => new DownloadWithProgress(client, uri, targetFilePath, reportRate);
        internal static string GetTimeoutMessage(Uri uri)
        {
            return $"Timeout occurred while waiting for {uri}";
        }

        /// <summary>
        /// Attempts to download <see cref="IWebResponseContent"/> to a file.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filePath"></param>
        /// <param name="overwrite"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="EndOfStreamException">If the response content header reports a ContentLength, 
        ///         the response content stream ended and the content wasn't the expected length.</exception>
        public static async Task<string> ReadAsFileAsync(this IWebResponseContent content, string filePath, bool overwrite, CancellationToken cancellationToken)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(filePath?.Trim()))
                throw new ArgumentNullException(nameof(filePath), "filename cannot be null or empty for ReadAsFileAsync");
            string pathname = Path.GetFullPath(filePath);
            if (!overwrite && File.Exists(filePath))
            {
                throw new InvalidOperationException(string.Format("File {0} already exists.", pathname));
            }

            try
            {
                using FileStream fileStream = new FileStream(pathname, FileMode.Create, FileAccess.Write, FileShare.None);
  
                long expectedLength = Math.Max(0, content.ContentLength ?? 0);

                using Stream? responseStream = await content.ReadAsStreamAsync().ConfigureAwait(false);
                await responseStream.CopyToAsync(fileStream, 81920, cancellationToken).ConfigureAwait(false);

                long fileStreamLength = fileStream.Length;

                if (expectedLength != 0 && fileStreamLength != expectedLength)
                    throw new EndOfStreamException($"File content length of '{fileStreamLength}' didn't match expected size '{expectedLength}'");

                return pathname;
            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (ObjectDisposedException)
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw;
            }
            catch (SystemException ex)
            {
                // Security/UnauthorizedAccess Excepton from `new FileStream`
                throw new IOException(ex.Message, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
