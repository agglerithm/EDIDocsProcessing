
Write-Host ""
Write-Host "Purge MassTransit Queues"
Write-Host ""

[Reflection.Assembly]::LoadWithPartialName("System.Messaging")

do
{
	$input = Read-Host "Do you want to purge all MT queues including Error queues? (Y/N)...  (or press 'P' to be prompted for each queue)"
	if ($input -eq "Y")
	{
		$input = Read-Host "Are you sure you want to also purge Error queues? (Y/N)"
	}
}
while (($input -ne "Y") -and ($input -ne "N") -and ($input -ne "P"))

if ($input -eq "P") 
{
	$mode = "PROMPT"
}
if ($input -eq "Y") 
{
	$mode = "PURGE-ALL"
	Write-Host "Purge ALL queues"
}
if ($input -eq "N") 
{
	$mode = "PURGE-NON-ERROR"
	Write-Host "Purge Non-Error queues"
}


$queues = [System.Messaging.MessageQueue]::GetPrivateQueuesByMachine("localhost")

foreach($q in $queues)
{
   if (($q.QueueName -like ("private$\mt_*")) -and ((-not($q.QueueName -like ("*_error"))) -or ($mode -eq "PURGE-ALL") -or ($mode -eq "PROMPT")) )
   {
      $purgeQ = "Y"
	  
      if ($mode -eq "PROMPT")
	  {
	  	 
		do
		{
			$input = Read-Host "Purge "$q.QueueName "? (Y/N)"
			$purgeQ = $input
		}
		while (($input -ne "Y") -and ($input -ne "N"))
	  }
   
      if ($purgeQ -eq "Y")
		{
		  Write-Host "Purging "$q.QueueName -ForegroundColor gray
	  	  $q.Purge()
		}
   
   }
}


function execute-Sql{
param($server, $db, $sql )
$sqlConnection = new-object System.Data.SqlClient.SqlConnection
$sqlConnection.ConnectionString = 'server=' + $server + ';integrated security=TRUE;database=' + $db 
$sqlConnection.Open()
$sqlCommand = new-object System.Data.SqlClient.SqlCommand
$sqlCommand.CommandTimeout = 120
$sqlCommand.Connection = $sqlConnection
$sqlCommand.CommandText= $sql
$text = $sql
Write-Progress -Activity "Executing SQL" -Status "Ejecuting SQL => $text..."
Write-Host "Ejecuting SQL => $text..."
$result = $sqlCommand.ExecuteNonQuery()
$sqlConnection.Close()
}


do
{
	$input = Read-Host "Do you want to clear the MassTransit DB Tables? (Y/N)"
}
while (($input -ne "Y") -and ($input -ne "N"))

if ($input -eq "Y")
{
	$server = Read-Host "Which server? (blank for localhost)"

	if (!$server)
	{
		$server = "localhost"
	}

	execute-Sql $server "masstransit" "truncate table HealthSaga"
	execute-Sql $server "masstransit" "truncate table SubscriptionClientSaga"
	execute-Sql $server "masstransit" "truncate table SubscriptionSaga"
	execute-Sql $server "masstransit" "truncate table TimeoutSaga"
}





