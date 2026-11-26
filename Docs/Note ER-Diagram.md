# Scelte nel Modello ER

I valori dei campi che sono pressoché pochi e tendono ad essere costanti li ho trasformati in Enum in C# (`PercorsoGOL`, `RuoloUtente`, `StatoAppuntamento`, `StatoRendicAppunt`, `StatoRendicPatto` e `StatoUtente`), ma lato DB non ho creato il relativo campo di tipo `ENUM(...)` e nemmeno l'uso del valore `TINYINT`, ma stringhe fisse date dalla conversione. Fa eccezione `RuoloUtente` dove nel DB è salvato l'intero.

I valori dei campi che sono molto variabili e possono subire molte modifiche li ho trasformati in tabelle di lookup (`Corso`, `LavoroObiettivo`, `StatoIntervento`, `TipoBeneficio`, `TitoloStudio`).

I dati di ogni OMLS sono separati nelle 2 tabelle per distinguere i dati di login (`Utente`) dall'anagrafica (`ProfiloOperatore`). Tra le 2 tabelle c'è la relazione `1:1`. Se un OMLS non lavora più per l'ente perderà il record in `Utente` ma manterrà il `ProfiloOperatore` che rimarrà collegato alle altre tabelle `GestionePatto` e `FAQ`.

La suddivisione dei dati di un Cittadino tra `Cittadino` e `GestionePatto` sembra innaturale, ma i dati provenienti dalla Regione di ciascun intervento GOL includono i campi in `Cittadino` e parte dei campi di `GestionePatto`(gli altri sono complilati dall'OMLS incaricato). L'unica tabella non è la soluzione ideale poiché per ad esempio per i percorsi di Upskilling o di Reskilling, il percorso è suddiviso in 3 parti / fasi: OR (Orientamento); Corso; ACC (accompagnamento al lavoro). 
Un cittadino che fa OR e ACC con lo stesso ente avrà 2 interventi GOL distinti con 2 IdPatto testuale diversi, ovvero 2 record distinti in `GestionePatto`! Questi 2 record cosa avranno in comune? I dati anagrafici del Cittadino!
E poiché è possibile che tra un intervento e l'altro abbia cambiato indirizzo e/o telefono e/o mail, questi ultimi si possono considerare, in un certo verso, legati all'intervento stesso!

Visto che questo gestionale non gestisce la fase del Corso, la tabella `Corso` è slegata da `GestionePatto` ed il campo `GestionePatto.CorsoScelto` è puramente informativo!

La tabella `ChecklistDocumentale` è stata mantenuta così semplice invece di una seria relazione `N:N` con `GestionePatto`perché non ho ricevuto abbastanza informazioni al riguardo!

