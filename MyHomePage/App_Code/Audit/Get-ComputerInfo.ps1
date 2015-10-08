[CmdletBinding()]
param (
[parameter(ValueFromPipeline=$true,
ValueFromPipelineByPropertyName=$true)]
[string]$ComputerName="$env:COMPUTERNAME"
)

$table = @{}

$ComputerInfo = Get-WmiObject -Class Win32_ComputerSystem -ComputerName $ComputerName | select Name, Manufacturer, Model, SystemType, Description, NumberOfProcessors, NumberOfLogicalProcessors, @{Name="RAM";
Expression={[math]::round($($_.TotalPhysicalMemory/1GB), 2)}}

$table.Add("Name", $ComputerInfo.Name)
$table.Add("Manufacturer", $ComputerInfo.Manufacturer)
$table.Add("Model", $ComputerInfo.Model)
$table.Add("SystemType", $ComputerInfo.SystemType)
$table.Add("Description", $ComputerInfo.Description)
$table.Add("NumberOfProcessors", $ComputerInfo.NumberOfProcessors)
$table.Add("NumberOfLogicalProcessors", $ComputerInfo.NumberOfLogicalProcessors)
$table.Add("RAM", $ComputerInfo.RAM)

$serial = Get-WmiObject -Class Win32_SystemEnclosure -ComputerName $ComputerName | select SerialNumber

$table.Add("Serial", $serial.SerialNumber)
Return $table