[CmdletBinding()]
param (
    [parameter(Mandatory=$true)]
    [string]$floor
)

$printers = (net view \\DR3Print).split() -match $floor

Return $printers