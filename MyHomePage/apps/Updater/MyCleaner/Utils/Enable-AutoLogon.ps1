[CmdletBinding()]
param (
    [parameter(Mandatory=$true)]
    [string]$user,
    [parameter(Mandatory=$true)]
    [string]$password
)

######################## Elevate Privelages ########################
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
		$cmd = $myInvocation.MyCommand.Definition + " -user " + $user + " -password " + $password
        $proc = Start-Process Powershell -ArgumentList $cmd -Wait -PassThru -Verb "runAs"

        # Exit from the current, unelevated, process
        [Environment]::Exit($proc.ExitCode)
}
######################## Elevate Privelages ########################

Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name DefaultUserName -Value $user
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name DefaultPassword -Value $password
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name AutoAdminLogon -Value "1"
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name ForceAutoLogon -Value "1"
Set-ItemProperty -Path "HKLM:\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon" -Name AutoLogonCount -Value "1"