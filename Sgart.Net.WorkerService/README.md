# Esempio di servizio windows in .NET 5

In debug può essere eseguito senza installazione semplicemente premendo F5

ATTENZIONE eseguire prima Sgart.Net.ConsoleApp per creare il DB


## registrare il servizio

I seguenti comandi servono per registrare l'applicativo come servizio windows, da eseguire con elevati privilegi.

Attenzione il comando "sc" funziona solo in CMD non in PowerShell

ATTENZIONE il servizio di default viene installato con l'utnete NT AUTHORITY\SYSTEM, configurarlo con l'utente corretto

## installazione

sc create "SgartNetWorkerService" DisplayName="Sgart Demo Service" binPath="C:\Temp\Sgart.Net.ConsoleApp\Sgart.Net.WorkerService\bin\Release\net5.0\publish\Sgart.Net.WorkerService.exe"
sc description SgartNet6WorkerService "Sgart.it servizio demo in .NET 6"

### avvio

sc start "SgartNetWorkerService"

### arresto

sc stop "SgartNetWorkerService"

### rimozione

sc delete "SgartNetWorkerService"

### verifica configurazione

sc qc SgartNetWorkerService

esempio di output
[SC] QueryServiceConfig SUCCESS

SERVICE_NAME: SgartNetWorkerService
        TYPE               : 10  WIN32_OWN_PROCESS
        START_TYPE         : 3   DEMAND_START
        ERROR_CONTROL      : 1   NORMAL
        BINARY_PATH_NAME   : C:\Temp\Sgart.Net.ConsoleApp\Sgart.Net.WorkerService\bin\Debug\net6.0\Sgart.Net.WorkerService.exe
        LOAD_ORDER_GROUP   :
        TAG                : 0
        DISPLAY_NAME       : SgartNetWorkerService
        DEPENDENCIES       :
        SERVICE_START_NAME : LocalSystem

### PowerShell

I comandi PowerShell equivalenti sono 

sc create => New-Service 
sc start => Start-Service SgartNetWorkerService
sc stop => Stop-Service SgartNetWorkerService
sc delete => non ho trovato un equivalente
sc qc => Get-Service SgartNetWorkerService | select *

esempio di registrazione nuovo servizio

$params = @{
      Name = "SgartNetWorkerService"
      BinaryPathName = 'C:\Temp\Sgart.Net.ConsoleApp\Sgart.Net.WorkerService\bin\Release\net5.0\publish\Sgart.Net.WorkerService.exe'
      DisplayName = "Sgart Demo Service"
      StartupType = "Manual"
      Description = "Sgart.it servizio demo in .NET 6"
    }
New-Service @params