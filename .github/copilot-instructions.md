# Copilot Instructions

## Build, test, lint

```shell
# Sestavení
dotnet build mp4.slnx

# Spuštění všech testů
dotnet test mp4.slnx

# Spuštění jednoho testu
dotnet test mp4.slnx --filter "FullyQualifiedName~MP4Test.ParseHeaders"
```

## Architektura

Knihovna pro parsování MP4 souborů (ISO Base Media File Format). MP4 soubor je strom typovaných „boxů" (atomů), každý identifikovaný 4-znakovým kódem (např. `moov`, `trak`, `stbl`).

**Tři projekty:**
- `JHa.MP4` — jádro knihovny (multi-target: net8.0, net10.0, netstandard2.0)
- `JHa.MP4.Test` — xUnit testy (net10.0, net4.8.1)
- `mp4cmd` — CLI nástroj pro inspekci/extrakci stop (`-s` zobrazí strukturu, `-e` extrahuje)

**Hierarchie tříd boxů:**
- `Box` — abstraktní základ; čte velikost + 4-znakový typ ze streamu
- `BoxNested` — kontejnerový box, rekurzivně parsuje potomky
- `FullBox<TFlags>` — rozšiřuje `Box` o byte verze + flags enum
- `CountedListBox<T>` — rozšiřuje `FullBox` pro boxy s počítaným polem položek

**Registrace boxů** je deklarativní: třídy jsou označeny atributem `[Box("xxxx")]` a automaticky nalezeny přes `BoxFactory` reflexí při startu. Neznámé typy se vrátí k základní třídě `Box`. Jedna třída může obsluhovat více typů boxů (např. `BoxNested` má `[Box("edts")][Box("dinf")][Box("udta")]`).

**Klíčová závislost:** `Lojzik.JHa.Common` poskytuje `String4` (4-znakové typové kódy) a `SubStream` (řezy streamu).

## Konvence

- Třídy boxů se jmenují `BoxXXXX`, kde `XXXX` odpovídá MP4 kódu velkými písmeny (např. `BoxSTCO` pro `stco`, `BoxMOOV` pro `moov`).
- Konstanty známých názvů boxů jsou v `BoxName` jako `static readonly String4` pole.
- Každý box vlastní svou parsovací logiku přepsáním `ReadData()`.
- `FindMandatoryBox<T>()` vyhodí `BoxNotFoundException` pro povinné potomky; `FindBox<T>()` vrací null pro volitelné.
- Primární konstruktory s parametrem `SubStream` (např. `public class BoxSTCO(SubStream stream) : CountedListBox<UInt32>(stream)`).
- C# 14.0, nullable reference types zapnuty, file-scoped namespaces v `JHa.MP4`.
- Testovací MP4 soubory jsou v kořeni testovacího projektu, kopírované do výstupu přes `CopyToOutputDirectory`.
