# 🚗 Watch Out – Een immersieve VR-ervaring over afleiding in het verkeer

_Watch Out_ is een interactieve virtual reality-ervaring ontwikkeld in Unity. Het doel van dit project is om gebruikers bewust te maken van de gevaren van afleiding tijdens het rijden, zoals smartphonegebruik, gesprekken met passagiers, afleiding door GPS of rijden in vermoeide toestand.  
Via realistische 360° video’s en interactieve scenario’s ervaart de gebruiker zelf wat een seconde van onoplettendheid kan veroorzaken.

---

## 🎯 Doelgroep

- Jonge bestuurders (18–25 jaar)
- Leerlingen tijdens verkeerseducatie
- Bestuurders tijdens terugkommomenten of preventiedagen
- Organisaties en bedrijven die inzetten op verkeersveiligheid

---

## 📹 Gebruikte technologieën

- **Unity URP** – versie 6000.0.49f1 (Silicon LTS)
- **Insta360 X5** – voor de opnames van de 360° video
- **Blender** – voor de witte minimalistische menuscène met auto + bandensporen
- **Meta Quest 3** – voor de VR-ervaring + handtracking
- **Meta SDK All-in-One** – voor headset- en handherkenning

---

## 🧩 Functionaliteiten

- Startmenu in een 3D-witte omgeving met crash-auto en rode bandensporen
- Keuze uit **vier scenario’s**:
  - 📱 **Telefoon** – gsm rinkelt op de passagiersstoel
  - 🗣️ **Passagier** – passagier probeert je aandacht te trekken
  - 🧭 **GPS** – navigatietoestel valt naar beneden
  - 😴 **Vermoeidheid** – oogleden zakken langzaam dicht
- **Head-tracking & handtracking**
- **Triggers** afhankelijk van blikrichting of reactie
- **Dubbele uitkomst** per scenario:
  - Crash → zwart scherm + confronterende eindboodschap
  - Succes → positieve eindboodschap

---

## 🧠 Interactie & Gebruikersflow

1. Startmenu: gebruiker kiest een scenario via handtracking
2. De 360° video start → gebruiker rijdt virtueel
3. Afleiding verschijnt (gsm, passagier, GPS of vermoeidheid)
4. **Twee mogelijke eindes**:
   - Crash → obstakel verschijnt → botsing → zwart scherm + waarschuwingsboodschap
   - Succes → gebruiker blijft geconcentreerd → positieve boodschap
5. Terugkeer naar menu voor keuze van een nieuw scenario of afsluiten

---

## 🗃️ Projectstructuur

Alle onderstaande mappen bevinden zich in de `Assets/` folder van Unity:

| Map                | Inhoud                                                  |
| ------------------ | ------------------------------------------------------- |
| `Scenes/`          | Bevat de menu-scène en de 4 scenario’s                  |
| `Scripts/`         | Aangepaste scripts: scene-switch, triggers, events      |
| `Resources/`       | Extra content voor runtime toegang                      |
| `Settings/`        | Unity-projectinstellingen                               |
| `TextMeshPro/`     | Tekstsystemen voor UI-elementen                         |
| `XR/`              | Configuraties voor XR-plugins (Meta)                    |
| `Plugins/Android/` | Externe Android-plugins                                 |
| `Prefabs/`         | Interactieve objecten (gsm, GPS, passagier, voetganger) |
| `Models/`          | 3D-modellen (auto’s, objecten)                          |
| `Materials/`       | Materialen en shaders voor 360° video en omgeving       |
| `Sounds/`          | Sounds effects                                          |

---

## ⚙️ Installatie & testen

1. Open het project in Unity (6000.0.49f1)
2. Zorg dat de Meta SDK correct is geïnstalleerd (All-in-One package)
3. Verbind de Meta Quest 3 via USB-C
4. Zorg dat je een **Meta Developer-account** hebt en dat het toestel geregistreerd is
5. Build & Run op de headset
6. Herinitialiseer de boundaries in de headset voor correcte plaatsing

---

## 🧪 Debugging

- Gebruik de Unity Console voor foutopsporing
- Live preview mogelijk, maar geeft geen echte headset-view
- Test regelmatig op de Quest 3 om timing van triggers correct af te stemmen
- Check of triggers pas activeren bij duidelijke blikrichting → voorkom te snelle activatie

---

## 🔄 Toekomstige verbeteringen

- Toevoegen van extra variaties binnen bestaande scenario’s
- Eye-tracking integratie (voor toekomstige headsets)
- Uitbreiding met meer realistische 3D-modellen zonder performantie te verliezen
- Mogelijkheid om keuzes te maken tijdens scenario’s (remmen, reageren)
- Gebruik van gebruikersdata voor educatieve evaluatie

---

## 🧑‍💻 Auteur

Achille Ernould

Dit project werd ontwikkeld als afstudeerproject aan de hogeschool, in het kader van verkeerspreventie en sensibilisering bij jonge bestuurders.

---
