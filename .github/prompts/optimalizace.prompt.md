---
name: optimalizace
description: "Optimalizuje kód pro lepší výkon"
model: Claude Haiku 4.5
---

Optimalizuj vybraný kód z hlediska výkonu.

1. Identifikuj bottlenecky (N+1 queries, zbytečné alokace)
2. Optimalizuj algoritmy a datové struktury
3. Minimalizuj alokace a GC pressure
4. Použij caching, lazy loading, paralelizaci kde je to vhodné

Optimalizuj pouze tam, kde to dává smysl. Zachovej čitelnost a udržovatelnost kódu.

💡 Začátek odpovědi napiš stručné hodnocení jednou větou a doplň score optimalizace na škále od 0 do 10. Následovat budou Identifikované problémy (⚠️) a nakonec navrheš optimalizovaný kód.

Je-li vybraný kód příliš rozsáhlý, nic neměň a vrať stručně zprávu "❌ Kód je příliš rozsáhlý na optimalizaci."

Pokud je kód již dostatečně optimalizovaný, nic neměň a vrať stručně zprávu "❌ Není potřeba optimalizovat kód."