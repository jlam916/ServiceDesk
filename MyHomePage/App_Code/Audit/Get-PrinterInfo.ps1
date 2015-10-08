[CmdletBinding()]
param (
    [parameter(ValueFromPipeline=$true,
    ValueFromPipelineByPropertyName=$true,
    Mandatory=$true)]
    [string]$computername
)
Get-WmiObject -Class Win32_printer -ComputerName $computername | select Name, Default, PortName, Shared