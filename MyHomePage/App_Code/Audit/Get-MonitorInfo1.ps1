[CmdletBinding()]
param (
[parameter(ValueFromPipeline=$true,
ValueFromPipelineByPropertyName=$true)]
[string]$ComputerName="$env:COMPUTERNAME"
)
<#
#region Elevate
    # Get the ID and security principal of the current user account
    $myWindowsID=[System.Security.Principal.WindowsIdentity]::GetCurrent()
    $myWindowsPrincipal=new-object System.Security.Principal.WindowsPrincipal($myWindowsID)
 
    # Get the security principal for the Administrator role
    $adminRole=[System.Security.Principal.WindowsBuiltInRole]::Administrator
    # Check to see if we are currently running "as Administrator"
    if ($myWindowsPrincipal.IsInRole($adminRole)){
       $Host.UI.RawUI.WindowTitle = $myInvocation.MyCommand.Definition + "(Elevated)"
       $Host.UI.RawUI.BackgroundColor = "DarkBlue"
       clear-host
    }
    else{
        $return = Start-Process Powershell -ArgumentList $myInvocation.MyCommand.Definition -Wait -Verb "runAs" -WindowStyle Hidden -PassThru

        # Exit from the current, unelevated, process
        Return $return
}
#endregion
#>

$ActiveMonitors = Get-WmiObject -Namespace root\wmi -Class wmiMonitorID -ComputerName $ComputerName
$monitorInfo = @()
 
        foreach ($monitor in $ActiveMonitors)
        {
            $mon = New-Object PSObject
            $manufacturer = $null
            $product = $null
            $serial = $null
            $name = $null
            $year = $null
 
            $monitor.ManufacturerName | foreach {$manufacturer += [char]$_}
            $monitor.ProductCodeID | foreach {$product += [char]$_}
            $monitor.SerialNumberID | foreach {$serial += [char]$_}
            $monitor.UserFriendlyName | foreach {$name += [char]$_}
 
            $mon | Add-Member NoteProperty Manufacturer $manufacturer
            $mon | Add-Member NoteProperty ProductCode $product
            $mon | Add-Member NoteProperty SerialNumber $serial
            $mon | Add-Member NoteProperty Name $name
            $mon | Add-Member NoteProperty Year $monitor.YearOfManufacture
 
            $monitorInfo += $mon
        }
        $monitorInfo # save object to file to retrieve