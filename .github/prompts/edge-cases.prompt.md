---
name: edge-cases
description: "Identifikuje rizika a edge cases"
model: Claude Haiku 4.5
agent: 'ask'
---

Analyzuj potenciální problémy ve vybraném kódu:

1. Jaké jsou možné edge cases (prázdné vstupy, null, extrémy)?
2. Kde chybí validace nebo error handling?
3. Jaká jsou bezpečnostní rizika?
4. Kde může dojít k race conditions nebo memory leaks?

Zaměř se jen na zásadnější rizika a edge cases, které mohou vést k chybám nebo zranitelnostem. Vždy:

- 🐞 Popiš problém stručně a jasně
- 💡 Navrhni konkrétní řešení nebo mitigaci. Navržené řešení je idiomatické.
- 🔗 Doplň odkazy do dokumentace, pokud je to relevantní