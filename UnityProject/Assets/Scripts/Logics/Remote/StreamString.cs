using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rovio.TapMatch.Remote
{
    // Defines the data protocol for reading and writing strings on our stream
    internal class StreamString : IDisposable
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public void Dispose()
        {
            ioStream.Dispose();
        }

        public async Task<string> ReadString()
        {
            int len = 0;
            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            if (len > 0)
            {
                byte[] inBuffer = new byte[len];
                await ioStream.ReadAsync(inBuffer, 0, len);
                return streamEncoding.GetString(inBuffer);
            }
            else
            {
                return null;
            }
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int)UInt16.MaxValue;
            }
            ioStream.WriteByte((byte)(len / 256));
            ioStream.WriteByte((byte)(len & 255));
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }

        public void Close()
        {
            if (ioStream != null)
            {
                Dispose();
                ioStream.Close();
            }
        }
    }
}
