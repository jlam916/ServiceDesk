# Summary
# This script will remove the computer from the domain -> restart -> rename the computer -> restart -> add computer to the domain -> shutdown
# This will also initiate action in Configuration Manager to allow SCCM to see the machine automatically
<# Error Codes:
    0: Success
    12: SchTasks Error
    13: netdom Error
#>

param(
    [Parameter(mandatory=$true)]
    [string] $newComputerName=$(throws "New computer name is null"),
    [int] $step=1
)

#region Elevate Privaleges
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
        # Create a new process object that starts PowerShell
		$cmd = $myInvocation.MyCommand.Definition + " -WinUpdate"
        $proc = Start-Process Powershell -ArgumentList $cmd -Wait -PassThru -Verb "runAs"

        # Exit from the current, unelevated, process
        exit $proc.ExitCode
    }
#endregion

switch($step){
    1 {
    Return 123
        $step=2
        schtasks.exe /Create /sc onstart /tn RenameComputerTask /tr "powershell.exe -ExecutionPolicy bypass -file $PSCommandPath $newComputerName $step" /IT /RU IMBAdmin /RP c0unters1gn /Delay 0000:10
        if($LASTEXITCODE -ne 0){
            Exit 12
        }

        # Remove the computer from the domain
        netdom.exe Remove $env:COMPUTERNAME /Domain:Itservices.Network /UserD:AddComputer /PasswordD:Ciwmb2010 /Reboot:5
        if($LASTEXITCODE -ne 0){
            Exit 13
        }
        New-Item -Path "$PSScriptRoot\RemoveDomain.txt" -ItemType File
    }
    2 {
        $step=3
        schtasks.exe /Delete /tn RenameComputerTask /f

        schtasks.exe /Create /sc onstart /tn AddDomainTask /tr "powershell.exe -ExecutionPolicy bypass -file $PSCommandPath $newComputerName $step" /IT /RU IMBAdmin /RP c0unters1gn /Delay 0000:10
        if($LASTEXITCODE -ne 0){
            Exit 12
        }

        netdom.exe RenameComputer $env:COMPUTERNAME /NewName:$newComputerName /Force /Reboot:5
        if($LASTEXITCODE -ne 0){
            Exit 13
        }
        New-Item -Path "$PSScriptRoot\Rename.txt" -ItemType File
    }
    3 {
        $step=4
        schtasks.exe /Delete /tn AddDomainTask /f
        
        schtasks.exe /Create /sc onstart /tn ConfigManagerTask /tr "powershell.exe -ExecutionPolicy bypass -file $PSCommandPath $newComputerName $step" /IT /RU IMBAdmin /RP c0unters1gn /Delay 0000:10

        netdom.exe join $ComputerName /d:itservices.network /userd:AddComputer /passwordd:Ciwmb2010 /ou:OU=W8Systems,OU="DR3 SYSTEMS",DC=itservices,DC=network /Reboot:5
        if($LASTEXITCODE -ne 0){
            Exit 13
        }
        New-Item -Path "$PSScriptRoot\AddDomain.txt" -ItemType File
    }
    4 {
        schtasks.exe /Delete /tn ConfigManagerTask /f

        #Run COnfiguration Manager Actions
        ## Defining action ID's
        $HWInventory = "{00000000-0000-0000-0000-000000000001}"
        $SWInventory = "{00000000-0000-0000-0000-000000000002}"
        $DiscoveryDataRecord = "{00000000-0000-0000-0000-000000000003}"
        $MachinePolicyRetrievalEvaluation = "{00000000-0000-0000-0000-000000000021}"
        $FileCollection = "{00000000-0000-0000-0000-000000000010}"
        $SWMeteringUsageReport = "{00000000-0000-0000-0000-000000000022}"
        $WindowsInstallerSourceList = "{00000000-0000-0000-0000-000000000032}"
        $SoftwareUpdatesScan = "{00000000-0000-0000-0000-000000000113}"
        $SoftwareUpdatesStore = "{00000000-0000-0000-0000-000000000114}"
        $SoftwareUpdatesDeployment = "{00000000-0000-0000-0000-000000000108}"

        ## Connect to WMI
        $SMSClient = [wmiclass] ("\\"+ $env:COMPUTERNAME +"\ROOT\ccm:SMS_Client")

        $SMSClient.TriggerSchedule($HWInventory)
        $SMSClient.TriggerSchedule($SWInventory)
        $SMSClient.TriggerSchedule($DiscoveryDataRecord)
        $SMSClient.TriggerSchedule($MachinePolicyRetrievalEvaluation)
        $SMSClient.TriggerSchedule($FileCollection)
        $SMSClient.TriggerSchedule($SWMeteringUsageReport)
        $SMSClient.TriggerSchedule($WindowsInstallerSourceList)
        $SMSClient.TriggerSchedule($SoftwareUpdatesScan)
        $SMSClient.TriggerSchedule($SoftwareUpdatesStore)
        $SMSClient.TriggerSchedule($SoftwareUpdatesDeployment)
    }
}

Exit 0