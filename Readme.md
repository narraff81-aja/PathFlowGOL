# PathFlowGOL (v0.1.0)

**Descrizione:** Sistema gestionale backend sviluppato in **ASP.NET Core** per il supporto operativo agli **OMLS** (Operatori del Mercato del Lavoro e dei Servizi) nell'ambito del **Programma GOL** (*Garanzia di Occupabilità dei Lavoratori*).

**Versione:** v0.1.0

**Stato del progetto:** Primo rilascio strutturale (*Data Layer & Domain Modeling*). Include le entità del dominio, la configurazione di Entity Framework Core con Migrations per MySQL, lo script SQL di popolamento iniziale (`seed.sql`) e il diagramma Entità-Relazione.

---

## Obiettivo del Progetto

PathFlowGOL nasce per modellare e supportare i flussi operativi del Programma GOL in modo tracciato, modulare e sicuro, separando chiaramente identità di accesso, profili operatore, anagrafica cittadini, patti, appuntamenti, documentazione e knowledge base (FAQ).

---

## Architettura e Stack Tecnologico

* **Framework:** .NET / ASP.NET Core Web API
* **Data Access:** Entity Framework Core (Migrations e `AppDbContext`) + MySQL
* **Modellazione Dominio:** Relazioni normalizzate (`1:1`, `1:N`), vincoli di integrità referenziale e `ValueConverter` personalizzati per gli enum del dominio GOL

---

## Struttura del Repository (`v0.1.0`)

* `Models/` – Entità del dominio (`Utente`, `ProfiloOperatore`, `Cittadino`, `GestionePatto`, `Appuntamento`, `ChecklistDocumentale`, `FAQ`, `Logging` e tabelle di lookup)
* `Data/` – `AppDbContext`, configurazioni Fluent API, indici univoci e `ValueConverter`
* `Migrations/` – Migrazioni Entity Framework Core per la generazione dello schema relazionale
* `Database/` – Script SQL di popolamento iniziale (`seed.sql`)
* `Docs/` – Diagramma Entità-Relazione del database (`ER-Diagram.png`)

---

## Modello Dati

Il database separa l'identità di accesso (**`Utenti`**) dall'anagrafica operativa (**`ProfiliOperatori`**) tramite una relazione `1:1` opzionale (`IdOperatore` su `Utenti` con comportamento `SetNull` in eliminazione):

* La rimozione di un account `Utente` preserva lo storico operativo (`GestionePatti`, `Appuntamenti`, `FAQs`) collegato al relativo `ProfiloOperatore`.
* L'utenza `SuperAdmin` iniziale (`Admin123`) dispone di un profilo dedicato per mantenere coerente l'integrità delle chiavi esterne sulle tabelle operative.

Il diagramma completo delle relazioni è disponibile in [`Docs/ER-Diagram.png`](Docs/ER-Diagram.png).

---

## Setup e Avvio Locale

### 1. Prerequisiti
* .NET SDK (9.0 o superiore)
* MySQL Server

### 2. Ripristino Dipendenze (NuGet)

Tramite "Package Manager Console" (o "Console di Gestione dei pacchetti") in Visual Studio:
```
Install-Package Pomelo.EntityFrameworkCore.MySql
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 9.0.0
Install-Package Swashbuckle.AspNetCore -Version 9.0.6
```

Oppure dal terminale / prompt dei comandi nella cartella principale: 
`dotnet restore`

### 3. Configurazione
Configurare i parametri di connessione verso MySQL in `appsettings.json`

### 4. Creazione Database e Seed Iniziale
      
1. Applicare le migration per generare lo schema:
    - Tramite "Package Manager Console" (o "Console di Gestione dei pacchetti") in Visual Studio:
`Update-Database`
    - Oppure dal terminale / prompt dei comandi nella cartella principale: 
`dotnet ef database update`

2. Eseguire lo script [`Database/seed.sql`](Database/seed.sql) sul database appena creato. Lo script popola:
    - Le tabelle di anagrafica/lookup (`TitoliStudio`, `TipiBenefici`, `LavoriObiettivo`, `StatiIntervento`)

    - L'utenza `SuperAdmin` iniziale e i profili/utenti di prova (con password già cifrate tramite `IPasswordHasher<Utente>`)

### 5. Credenziali di Test (`seed.sql`)
SuperAdmin: `Admin123` / `Admin123`

---

## Prossimi Sviluppi (`v0.2.0`)

Nel prossimo rilascio verranno reintegrati e completati i livelli applicativi (`DTOs`, `Services`, `Controllers`) per le prime aree funzionali:

* **Autenticazione e Gestione Sessione (`AuthController`):** Login tramite Cookie Authentication con emissione dei claim di ruolo, logout e invalidazione della sessione al cambio password.
* **Tabelle di Lookup / Anagrafiche di Base:** Espozione degli endpoint REST (con relativi DTO e Service) per la gestione e consultazione delle tabelle di supporto (`TitoliStudio`, `TipiBenefici`, `LavoriObiettivo`, `StatiIntervento`).

