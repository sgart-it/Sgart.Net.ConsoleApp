# Esempio di servizio windows in .NET 5
In debug può essere eseguito senza installazione semplicemente premendo F5

ATTENZIONE eseguire prima Sgart.Net5.ConsoleApp per creare il DB


## registrare il servizio
I seguenti comandi servono per registrare l'applicativo come servizio windows, da eseguire con elevati privilegi.

Attenzione il comando "sc" funziona solo in CMD non in PowerShell

ATTENZIONE il servizio di default viene installato con l'utnete NT AUTHORITY\SYSTEM, configurarlo con l'utente corretto

## installazione
sc create "SgartNet5WorkerService" DisplayName="Sgart Demo Service" binPath="C:\Temp\Sgart.Net5.ConsoleApp\Sgart.Net5.WorkerService\bin\Release\net5.0\publish\Sgart.Net5.WorkerService.exe"
sc description SgartNet5WorkerService "Sgart.it servizio demo in .NET 5"

### avvio
sc start "SgartNet5WorkerService"

### arresto
sc stop "SgartNet5WorkerService"

### rimozione
sc delete "SgartNet5WorkerService"

### verifica configurazione
sc qc SgartNet5WorkerService

esempio di output
[SC] QueryServiceConfig SUCCESS

SERVICE_NAME: SgartNet5WorkerService
        TYPE               : 10  WIN32_OWN_PROCESS
        START_TYPE         : 3   DEMAND_START
        ERROR_CONTROL      : 1   NORMAL
        BINARY_PATH_NAME   : C:\Temp\Sgart.Net5.ConsoleApp\Sgart.Net5.WorkerService\bin\Debug\net5.0\Sgart.Net5.WorkerService.exe
        LOAD_ORDER_GROUP   :
        TAG                : 0
        DISPLAY_NAME       : SgartNet5WorkerService
        DEPENDENCIES       :
        SERVICE_START_NAME : LocalSystem

### PowerShell
I comandi PowerShell equivalenti sono 

sc create => New-Service 
sc start => Start-Service SgartNet5WorkerService
sc stop => Stop-Service SgartNet5WorkerService
sc delete => non ho trovato un equivalente
sc qc => Get-Service SgartNet5WorkerService | select *

esempio di registrazione nuovo servizio

$params = @{
      Name = "SgartNet5WorkerService"
      BinaryPathName = 'C:\Temp\Sgart.Net5.ConsoleApp\Sgart.Net5.WorkerService\bin\Release\net5.0\publish\Sgart.Net5.WorkerService.exe'
      DisplayName = "Sgart Demo Service"
      StartupType = "Manual"
      Description = "Sgart.it servizio demo in .NET 5"
    }
New-Service @params