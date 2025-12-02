using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Jha.MP4;

public class MP4File : IDisposable
{
    public List<Box> Nested { get; } = [];
    private Stream Stream { get; }
    public MP4File(Stream stream)
    {
        long startIndex = 0;
        Stream = stream;
        do
        {
            var box = Box.CreateBox(stream, startIndex);
            startIndex += box.Size;
            Nested.Add(box);
        } while (startIndex < stream.Length);
        FTYP = Nested.Where(x => x.Type.Equals(BoxName.ftyp)).Cast<BoxFTYP>().Single();
        MOOV = Nested.Where(x => x.Type.Equals(BoxName.moov)).Cast<BoxMOOV>().Single();
    }
    public BoxFTYP FTYP { get; }
    public BoxMOOV MOOV { get; }
    public static MP4File Parse(Stream stream)
    {
        return new MP4File(stream);
    }
    public static MP4File Parse(string filename)
    {
        return new MP4File(File.OpenRead(filename));
    }
    #region IDisposable Support
    private bool disposedValue = false; // Zjištění redundantních volání

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Stream.Dispose();
            }
            // TODO: Uvolněte nespravované prostředky (nespravované objekty) a přepište finalizační metodu níže.
            // TODO: Nastavte velká pole na hodnotu null.
            disposedValue = true;
        }
    }
    // TODO: Přepište finalizační metodu, jenom pokud metoda Dispose(bool disposing) výše obsahuje kód pro uvolnění nespravovaných prostředků.
    ~MP4File() {
      // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
      Dispose(false);
    }
    // Tento kód je přidaný pro správnou implementaci odstranitelného vzoru.
    public void Dispose()
    {
        // Neměňte tento kód. Kód pro vyčištění vložte do výše uvedené metody Dispose(bool disposing).
        Dispose(true);
        // TODO: Zrušte komentář následujícího řádku, pokud se výše přepisuje finalizační metoda.
        GC.SuppressFinalize(this);
    }
    #endregion
}
