[Reflection.Assembly]::LoadWithPartialName("System.Messaging")

function RecreateQueue
{param ($Name, $Transactional)

# create a new queue...
if ([System.Messaging.MessageQueue]::Exists($Name))
  {
  	[System.Messaging.MessageQueue]::Delete($Name)
	write-Output "$Name Deleted"
  }

$qnew = [System.Messaging.MessageQueue]::Create($Name, $Transactional)
$qnew.SetPermissions("Everyone", [System.Messaging.MessageQueueAccessRights]::FullControl)
write-Output "$Name Created with Transactional = $Transactional"

# access the existing queue
$q1 = new-object System.Messaging.MessageQueue $Name

$trans = new-object System.Messaging.MessageQueueTransaction

# Create a message
$msg = new-object System.Messaging.Message "Test Message"
$msg.Label = "A label"

# Send the message to the queue
if ($Transactional) {$trans.Begin()}
$q1.Send($msg,$trans)
if ($Transactional) {$trans.Commit()}

# Messages in our queue will hold bodies of type string. Set up an array of type
# names for the formatter (can there be more than one?).
$q1.Formatter.TargetTypeNames = ,"System.String"

# tell the queue we'd like to see some additional properties on
# messages we retrieve
$q1.MessageReadPropertyFilter.ArrivedTime = $true
$q1.MessageReadPropertyFilter.SentTime = $true

# Better have a timeout on peek and retrieve operations; if the queue
# is empty they will block the powershell session otherwise
$ts = new-object TimeSpan 30000000 # 3 seconds for example

#$pkmsg = $q1.Peek($ts)

#$pkmsg

# Retreive and remove the first message in the queue
$rmsg = $q1.Receive($ts)

# look at its properties..
$rmsg.body

Write-Output "$Name Tested"
Write-Output ""
}

$isTransactional = $false

#RecreateQueue ".\private$\mt_server1" $isTransactional
#RecreateQueue ".\private$\mt_server" $isTransactional
#RecreateQueue ".\private$\mt_client" $isTransactional
#RecreateQueue ".\private$\mt_servercontrol" $isTransactional
#RecreateQueue ".\private$\mt_error" $isTransactional
#RecreateQueue ".\private$\test_servicebus" $isTransactional
#RecreateQueue ".\private$\mt_testSubscriber" $isTransactional
#RecreateQueue ".\private$\mt_testPublisher" $isTransactional

RecreateQueue ".\private$\mt_deferred" $isTransactional
RecreateQueue ".\private$\mt_subscription" $isTransactional
RecreateQueue ".\private$\mt_timeout" $isTransactional
RecreateQueue ".\private$\mt_timeout_control" $isTransactional
RecreateQueue ".\private$\mt_systemview" $isTransactional
RecreateQueue ".\private$\mt_systemview_control" $isTransactional

RecreateQueue ".\private$\mt_MessagingTests" $isTransactional
RecreateQueue ".\private$\mt_MessagingTests_error" $isTransactional

RecreateQueue ".\private$\mt_edi_docs_in" $isTransactional
RecreateQueue ".\private$\mt_edi_docs_out" $isTransactional
RecreateQueue ".\private$\mt_edi_docs_out_error" $isTransactional

RecreateQueue ".\private$\mt_orders_in" $isTransactional
RecreateQueue ".\private$\mt_orders_in_error" $isTransactional

RecreateQueue ".\private$\mt_shipment_requests_in" $isTransactional
RecreateQueue ".\private$\mt_shipment_requests_in_error" $isTransactional

RecreateQueue ".\private$\mt_email_service" $isTransactional
RecreateQueue ".\private$\mt_email_service_error" $isTransactional


RecreateQueue ".\private$\mt_send_shipment_requests" $isTransactional
RecreateQueue ".\private$\mt_send_shipment_requests_error" $isTransactional




