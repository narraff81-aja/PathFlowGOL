# PathFlowGOL

**Descrizione:** Sistema gestionale backend sviluppato in **ASP.NET Core** per il supporto operativo agli **OMLS** (Operatori del Mercato del Lavoro e dei Servizi) nell'ambito del **Programma GOL** (*Garanzia di Occupabilità dei Lavoratori*).

**Versione:** v0.2.0

**Stato del progetto:** 
* **Autenticazione e Gestione Sessione (`AuthController`):** Login tramite Cookie Authentication con emissione dei claim di ruolo, logout e invalidazione della sessione al cambio password.
* **Tabelle di Lookup / Anagrafiche di Base:** Espozione degli endpoint REST (con relativi DTO e Service) per la gestione e consultazione delle tabelle di supporto (`TitoliStudio`, `TipiBenefici`, `LavoriObiettivo`, `StatiIntervento`).


---

## Obiettivo del Progetto

PathFlowGOL nasce per modellare e supportare i flussi operativi del Programma GOL in modo tracciato, modulare e sicuro, separando chiaramente identità di accesso, profili operatore, anagrafica cittadini, patti, appuntamenti, documentazione e knowledge base (FAQ).

---

## Architettura e Stack Tecnologico

* **Framework:** .NET / ASP.NET Core Web API
* **Data Access:** Entity Framework Core (Migrations e `AppDbContext`) + MySQL
* **Modellazione Dominio:** Relazioni normalizzate (`1:1`, `1:N`), vincoli di integrità referenziale e `ValueConverter` personalizzati per gli enum del dominio GOL

---

## Struttura del Repository (`v0.2.0`)

* `Models/` – Entità del dominio (`Utente`, `ProfiloOperatore`, `Cittadino`, `GestionePatto`, `Appuntamento`, `ChecklistDocumentale`, `FAQ`, `Logging` e tabelle di lookup)
* `Data/` – `AppDbContext`, configurazioni Fluent API, indici univoci e `ValueConverter`
* `Migrations/` – Migrazioni Entity Framework Core per la generazione dello schema relazionale
* `Database/` – Script SQL di popolamento iniziale (`seed.sql`)
* `Docs/` – Diagramma Entità-Relazione del database (`ER-Diagram.jpg`) e foglio di calcolo con la matrice dei permessi RBAC (`Permessi_RBAC.xlsx`)
* `DTOs/` – Oggetti di trasferimento dati per input/output e validazione dati
* `Services/` – Logica applicativa e regole di autorizzazione/business
* `Controllers/` – Endpoint REST API

---

## Modello Dati

Il database separa l'identità di accesso (**`Utenti`**) dall'anagrafica operativa (**`ProfiliOperatori`**) tramite una relazione `1:1` opzionale (`IdOperatore` su `Utenti` con comportamento `SetNull` in eliminazione):

* La rimozione di un account `Utente` preserva lo storico operativo (`GestionePatti`, `Appuntamenti`, `FAQs`) collegato al relativo `ProfiloOperatore`.
* L'utenza `SuperAdmin` iniziale (`Admin123`) dispone di un profilo dedicato per mantenere coerente l'integrità delle chiavi esterne sulle tabelle operative.

La documentazione di riferimento è disponibile nella cartella [`Docs/`](Docs/):

* **Modello Entità-Relazione:** [`Docs/ER-Diagram.jpg`](Docs/ER-Diagram.jpg)
* **Matrice Permessi RBAC:** [`Docs/Permessi_RBAC.xlsx`](Docs/Permessi_RBAC.xlsx)

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

## Prossimi Sviluppi (`v0.3.0`)

Nel prossimo rilascio verranno reintegrati e completati i livelli applicativi (`DTOs`, `Services`, `Controllers`) e l'aggiornamento della matrice permessi per le aree successive:

* **Gestione Utenti e Profili Operatore:** Endpoint per amministrazione utenze, creazione/associazione `ProfiloOperatore` e allineamento dei controlli di accesso.

* **Moduli Operativi e Knowledge Base:** Avvio dell'implementazione dei moduli per `Cittadini`, `GestionePatti`, `ChecklistDocumentale`, `Appuntamenti` e `FAQs`.