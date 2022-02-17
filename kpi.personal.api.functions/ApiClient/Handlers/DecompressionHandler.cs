using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace kpi.personal.api.functions.ApiClient.Handlers
{
    public class DecompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors;

        public DecompressionHandler()
        {
            Compressors = new Collection<ICompressor>();
            Compressors.Add(new GZipCompressor());
            Compressors.Add(new DeflateCompressor());
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            if ((response.Content != null) && (response.Content.Headers.ContentEncoding != null) && (response.Content.Headers.ContentEncoding.Count > 0))
            {
                var encoding = response.Content.Headers.ContentEncoding.First();

                var compressor = Compressors.FirstOrDefault(c => c.EncodingType.Equals(encoding, StringComparison.InvariantCultureIgnoreCase));

                if (compressor != null)
                {
                    response.Content = await DecompressContentAsync(response.Content, compressor).ConfigureAwait(false);
                }
            }

            return response;
        }

        private static async Task<HttpContent> DecompressContentAsync(HttpContent compressedContent, ICompressor compressor)
        {
            using (compressedContent)
            {
                MemoryStream decompressed = new MemoryStream();
                await compressor.Decompress(await compressedContent.ReadAsStreamAsync(), decompressed).ConfigureAwait(false);

                // set position back to 0 so it can be read again
                decompressed.Position = 0L;

                var newContent = new StreamContent(decompressed);
                // copy content type so we know how to load correct formatter
                newContent.Headers.ContentType = compressedContent.Headers.ContentType;
                return newContent;
            }
        }
    }
}