# Demo console application .Net 5 + EF Core 5 + Nlog

# Servizio windows

Per configurarlo come servizio windows aggiungere in Program.cs 

.ConfigureServices((hostContext, services) => {
	...
	services.AddHostedService<SgartWorker>();
}

se serve scrivere nel windows log

.ConfigureServices((hostContext, services) => {
	...
    services.AddHostedService<SgartWorker>()
        .Configure<EventLogSettings>(config =>
        {
            config.LogName = "Sgart Net5 Service";
            config.SourceName = "Sgart Net5 Service Source";
        });
}

services.AddHostedService<SgartWorker>();

sostituire .UseConsoleLifetime()
=> .UseWindowsService()

## Registrare il servizio

Per registrare il servizio

sc create "SgartNet5ConsoleService" binPath="C:\SgartNet5ConsoleService\Sgart.Net5.ConsoleApp.exe" 
sc start "SgartNet5ConsoleService"