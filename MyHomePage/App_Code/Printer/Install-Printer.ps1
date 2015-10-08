[CmdletBinding()]
param (
    [parameter(Mandatory=$true)]
    [string]$computerName,
    [parameter(Mandatory=$true)]
    [string]$printerName
)

rundll32.exe printui.dll,PrintUIEntry /ga /c\\$computerName /n\\Dr3print\$printerName
Start-Sleep -seconds 5

sc.exe \\$computerName stop spooler
Start-Sleep -seconds 2
sc.exe \\$computerName start spooler