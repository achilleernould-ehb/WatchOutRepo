# # 🚗 Watch Out – Een immersieve VR-ervaring over afleiding in het verkeer

Watch Out is een interactieve virtual reality-ervaring ontwikkeld in Unity. Het doel van dit project is om gebruikers bewust te maken van de gevaren van afleiding tijdens het rijden, met name het gebruik van smartphones achter het stuur. Via een realistische 360° video en een schokkend scenario ervaart de gebruiker wat een seconde van onoplettendheid kan veroorzaken.

## 🎯 Doelgroep

- Jonge bestuurders (18–25 jaar)
- Leerlingen tijdens verkeerseducatie
- Gebruikers tijdens terugkommomenten of preventiedagen

## 📹 Gebruikte technologieën

- **Unity URP** – versie 6000.0.49f1 (Silicon LTS)
- **Insta360 X5** – voor de opnames van de 360° video
- **Meta Quest 3** – voor de VR-ervaring + handtracking
- **Meta SDK All-in-One** – voor headset- en handherkenning

## 🧩 Functionaliteiten

- 360° video in een auto
- Head-tracking (event trigger bij > 45° hoofdrotatie)
- Handtracking zichtbaar in de scène
- Dynamisch event: een voetganger verschijnt en wordt aangereden
- Eindscherm met preventieve boodschap

## 🧠 Interactie & Gebruikersflow

1. Startmenu met een knop om de scène te starten  
2. Gebruiker bevindt zich in een rijdende auto (360° video)  
3. Bij afleiding (naar rechts kijken) wordt een event getriggerd  
4. Een voetganger verschijnt → botsing → zwart scherm  
5. Eindboodschap: **"Een seconde van onoplettendheid kan een leven kosten."**

## 🗃️ Projectstructuur

Alle onderstaande mappen bevinden zich in de `Assets/` folder van Unity:

| Map | Inhoud |
|-----|--------|
| `Scenes/` | Bevat de twee scènes van het project (start + auto) |
| `Scripts/` | Bevat 3 aangepaste scripts: scene-switch, hoofdrotatie-event, spawn van het object |
| `Resources/` | Eventuele extra content voor runtime toegang |
| `Settings/` | Unity-projectinstellingen |
| `TextMeshPro/` | Tekstsystemen voor UI-elementen |
| `XR/` | Configuraties voor XR-plugins (Meta) |
| `Plugins/Android/` | Externe Android-plugins |
| `Prefabs/` | Testobjecten (bv. placeholder voor voetganger) |
| `Models/` | 3D-modellen van auto’s (waarvan sommige verworpen zijn) |
| `Materials/` | Materialen en shaders voor 360° video |
| `Azerillo/` | 3D-model van de auto in de uiteindelijke scène |

## ⚙️ Installatie & testen

1. Open het project in Unity (6000.0.49f1)
2. Zorg dat de Meta SDK correct is geïnstalleerd (All-in-One package)
3. Verbind de Meta Quest 3 via USB-C
4. Zorg dat je een **Meta Developer-account** hebt en dat het toestel geregistreerd is
5. Build & Run op de headset
6. Herinitialiseer de boundaries in de headset voor correcte plaatsing

## 🧪 Debugging

- Gebruik de Unity Console voor foutopsporing
- Live preview is mogelijk, maar geeft geen echte headset-view
- Test regelmatig op het toestel voor correcte hoofdtracking en timing van events

## 🔄 Toekomstige verbeteringen

- Toevoegen van meerdere scenario’s (verschillende afleidingen)
- Mogelijkheid tot remactie of keuze van gebruiker
- Betere 3D-modellen of lichte optimalisaties
- Opname van gebruikersreacties voor educatieve doeleinden

## 🧑‍💻 Auteur

Dit project werd ontwikkeld als afstudeerproject aan de hogeschool, in het kader van verkeerspreventie bij jongeren.

---

