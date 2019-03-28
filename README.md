# mp4lib
simple mp4 parser.

```csharp
using JHa.MP4;

using (var mp4file = MP4File.Parse(filename))
{
  foreach (var trak in mp4file.MOOV.TRAKS)
  {
    using (var filewrite = File.OpenWrite(trackname))
    {
       trak.WriteAllTo(stream);
    }
  }
}
```
