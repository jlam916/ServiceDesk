[CmdletBinding()]
param (
    [parameter(Mandatory=$true)]
    [string]$computerName
)
$IPItems =  @()
$colItems = Get-WmiObject -Class "Win32_NetworkAdapterConfiguration" `
            -ComputerName $computerName | Where {$_.IPEnabled -match "True"}

foreach($colItem in $colItems){
    $config = New-Object PSObject
    $Description = $null
    $IP = $null
    $MAC = $null
    $DNS = $null

    $Description = $colItem.Description
    $IP = $colItem.IPAddress
    $MAC = $colItem.MACAddress
    $DNS = $colItem.DNSDomain
 
    $config | Add-Member NoteProperty Description $Description
    $config | Add-Member NoteProperty IP $IP[0]
    $config | Add-Member NoteProperty MAC $MAC
    $config | Add-Member NoteProperty DNS $DNS
 
    $IPItems += $config
}

Return $IPItems