[Reflection.Assembly]::LoadWithPartialName("System.Messaging")
$pqs = [System.Messaging.MessageQueue]::GetPrivateQueuesByMachine(".")
foreach($q in $pqs)
  {
    Write-Host "       "$q.QueueName  -ForegroundColor gray
	$q.SetPermissions("EVERYONE", [System.Messaging.MessageQueueAccessRights]::FullControl, [System.Messaging.AccessControlEntryType]::Set)
  }
