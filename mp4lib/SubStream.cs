using System;
using System.IO;

namespace JHa
{
    public class SubStream : Stream
    {
        private long length;
        public long StartPosition;
        public SubStream(Stream baseStream, long startPosition, long length)
        {
            BaseStream = baseStream;
            StartPosition = startPosition;
            this.length = length;
        }
        public SubStream(Stream baseStream, long startPosition) : this(baseStream, startPosition, baseStream.Length - startPosition)
        {

        }

        private Stream BaseStream { get; }
        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => BaseStream.CanWrite;
        public override long Length => length;
        public override long Position { get; set; }

        public override void Flush() => BaseStream.Flush();

        private void SetBaseStreamPosition()
        {
            BaseStream.Position = StartPosition + Position;
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            lock (BaseStream)
            {
                SetBaseStreamPosition();
                var readed = BaseStream.Read(buffer, offset, count);
                Position += readed;
                return readed;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: Position = offset; break;
                case SeekOrigin.Current: Position += offset; break;
                case SeekOrigin.End: Position = Length + offset; break;
            }
            return Position;
        }

        public override void SetLength(long value) => length = value;

        public override void Write(byte[] buffer, int offset, int count)
        {
            lock (BaseStream)
            {
                SetBaseStreamPosition();
                BaseStream.Write(buffer, offset, count);
                Position += count;
            }
        }
        public override int Read(Span<byte> buffer)
        {
            lock (BaseStream)
            {
                SetBaseStreamPosition();
                var readed = BaseStream.Read(buffer);
                Position += readed;
                return readed;
            }
        }

        public Stream RootStream()
        {
            if (BaseStream is SubStream pom)
                return pom.RootStream();
            return BaseStream;
        }
    }
}