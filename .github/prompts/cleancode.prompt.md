---
name: cleancode
description: "Navrhne čistější kód"
model: Claude Sonnet 4.5
---

Navrhni u vybraného kódu takové změny, které:

- zlepší čitelnost a srozumitelnost
- sníží duplicity a zlepší strukturu
- dodrží best practices pro daný programovací jazyk a framework

Nenavrhuj techncký refaktoring, přidávání handlování chyb nebo optimalizace či cokoliv jiného mimo čistoty kódu. 

Zaměř se pouze na zásadnější nedostatky v kódu. Do výsledku vypiš základní nedostatky kódu (⚠️) a následně navrhni kompletní refaktorovaný kód.

Nedává-li jakákkoliv změna smysl, pouze odpověd stručně: "❌ Není potřeba kód refaktorovat."