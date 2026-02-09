---
name: dokumentace
description: "Vytvoří dokumentaci ke kódu"
model: Claude Sonnet 4.5
---

Na základě vybraného kódu vytvoř dokumentaci, která bude využívat markdown a bude obsahovat následující části:

- 💡 velmi stručný popis funkce kódu jednou větou
- 📋 popis algoritmu jako stručný seznam kroků
- 📦 příklady použití, pokud je to relevantní
- 🧩 je-li to relevantní, doplň diagramy pomocí mermaid

Pokud některá z částí není relevantní, vynech ji.

Nadpis dokumentace bude vždy název funkcionality, modulu, třídy, metody nebo toho, co je dokumentováno.

Dokumentuj pouze public API. Dokumentace je v češtině.