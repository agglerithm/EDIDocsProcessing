#get-childitem C:\sandbox\masstransit-read-only\bin\* -Recurse | foreach-object -process {$_.FullName}

get-childitem C:\sandbox\masstransit-read-only\bin\* -Recurse | foreach-object -process {Copy-Item $_.FullName -Destination .\lib\MassTransit}

