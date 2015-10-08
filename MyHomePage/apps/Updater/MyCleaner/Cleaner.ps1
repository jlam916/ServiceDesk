#========================================================================
# Created on:   1/12/2014 12:47 PM
# Last update:  4/13/2014 3:21 PM
# Created by:   Randell Koen
# Filename: 	Cleaner.ps1
#========================================================================

param(
	[switch] $Java,
	[switch] $Chrome,
	[switch] $VirusScan,
	[switch] $WinUpdate,
	[switch] $CCleaner
)
Set-Variable SUCCESS -Option Constant -Value 0
Set-Variable REBOOT -Option Constant -Value 1

function Get-ScriptDirectory{ 
	if($hostinvocation -ne $null){
		Split-Path $hostinvocation.MyCommand.path
	}
	else{
		Split-Path $script:MyInvocation.MyCommand.Path
	}
}


if($Java){
	$scriptPath = Get-ScriptDirectory
	if([System.IntPtr]::Size -eq 8){
		$javaFull =  dir "HKLM:\SOFTWARE\Wow6432Node\JavaSoft\Java Runtime Environment"  | select -expa pschildname -Last 1
		$jverMajor = $javaFull.Substring(2, 1)
		$jver = $javaFull.Substring($javaFull.indexOf("_") + 1)
		if($jverMajor -lt 8){
			$cmd = "$scriptPath\Utils\jre-8u25-windows-i586.exe"
			& $cmd /s
		}
		if($jver -lt 25){
			$cmd = "$scriptPath\Utils\jre-8u25-windows-i586.exe"
			& $cmd /s
		}
	}
	else{
		$java =  dir "HKLM:\SOFTWARE\JavaSoft\Java Runtime Environment"  | select -expa pschildname -Last 1
		$jverMajor = $javaFull.Substring(2, 1)
		$jver = $java.Substring($java.indexOf("_") + 1)
		if($jverMajor -lt 8){
			$cmd = "$scriptPath\Utils\jre-8u25-windows-i586.exe"
			& $cmd /s
		}
		if($jver -lt 25){
			$cmd = "$scriptPath\Utils\jre-8u25-windows-i586.exe"
			& $cmd /s
		}
	}
}

if($Chrome){
	$scriptPath = Get-ScriptDirectory
	if([System.IntPtr]::Size -eq 8){
		if(Test-Path -Path "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"){
			Write-Host Removing Google Chrome...
			$exe = "$scriptPath\Utils\ChromeUninstall.exe"
			$args = "--uninstall --multi-install --chrome --system-level"
			$proc = (Start-Process $exe $args -PassThru)
			$proc | Wait-Process
			
			if(Test-Path -Path "C:\Program Files (x86)\Google\Chrome"){
				Remove-Item -Path "C:\Program Files (x86)\Google\Chrome" -Recurse -Force
			}
			
			MsiExec.exe /i $scriptPath\Utils\ChromeFix.msi /QN /norestart
			Write-Host Done!
		}
	}
	else{
		if(Test-Path -Path "C:\Users\calrecycle\AppData\Local\Google\Chrome"){
			Write-Host Removing Google Chrome...
			$exe = "$scriptPath\Utils\ChromeUninstall.exe"
			$args = "--uninstall --multi-install --chrome --system-level"
			$proc = (Start-Process $exe $args -PassThru)
			$proc | Wait-Process
			MsiExec.exe /i $scriptPath\Utils\ChromeFix.msi /QN /norestart
			Write-Host Done!
		}
	}
}

if($VirusScan){
		# Defender for Windows 8
        $Host.UI.RawUI.WindowTitle = "Virus Update/Scan"
		Update-MPSignature
		Start-MpScan -ScanType QuickScan
}

if($WinUpdate){
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
		$cmd = $myInvocation.MyCommand.Definition + " -WinUpdate"
        $proc = Start-Process Powershell -ArgumentList $cmd -Wait -PassThru -Verb "runAs"

        # Exit from the current, unelevated, process
        [Environment]::Exit($proc.ExitCode)
}
######################## Elevate Privelages ########################
    $old_ErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = 'SilentlyContinue'
    $Host.UI.RawUI.WindowTitle = "Windows Update"
	$scriptPath = Get-ScriptDirectory
	$resultcode= @{0="Not Started"; 1="In Progress"; 2="Succeeded"; 3="Succeeded With Errors"; 4="Failed" ; 5="Aborted" }
	$progress = 5
	Write-Progress -Activity 'Windows Updates' -Status 'Performing Windows Updates' -CurrentOperation 'Checking for updates' -PercentComplete ($progress++) -Id 0
	$updateSession = new-object -com "Microsoft.Update.Session"
	
	if(Test-Path -Path "C:\Windows\System32\Tasks\LCScheduler"){
		schtasks.exe /Delete /TN LCScheduler /f
	}

	$Updates=$updateSession.CreateupdateSearcher().Search($criteria).Updates
	 
	if ($Updates.Count -eq 0){
	    Write-Progress -Activity 'Windows Updates' -Status 'Done' -Completed  -Id 0
        $richTextBox.AppendText("Done!`n")
		[Environment]::Exit($SUCCESS)
	}   
	 else {
		$updatesToDownload = New-object -com "Microsoft.Update.UpdateColl"
		$count = 0;
		foreach($update in $Updates){
			$count++;
			Write-Progress -Activity 'Adding Updates' -Status 'Collecting specified updates' -PercentComplete ($count/$Updates.Count *100) -Id 1
			$addThisUpdate = $True
			if ($update.InstallationBehavior.CanRequestUserInput){
				$addThisUpdate = $False
			}
			if (!$update.EulaAccepted){
				$update.AcceptEula()	
			}
			if ($addThisUpdate){
				$updatesToDownload.Add($update) | Out-Null
			}
		}
		Write-Progress -Activity 'Adding Updates' -Status 'Done' -Completed -Id 1
		if ($updatesToDownload.Count -eq 0){
			Write-Progress -Activity 'Windows Updates' -Status 'Done' -Completed  -Id 0
            $richTextBox.AppendText("Done!`n")
			[Environment]::Exit($SUCCESS)
		}
		else{
			# Display updates
			Write-Progress -Activity 'Windows Updates' -Status 'Performing Windows Updates' -CurrentOperation "Downloading $($updatesToDownload.Count) Updates" -PercentComplete ($progress += 10)  -Id 0
			$downloader = $updateSession.CreateUpdateDownloader()
			#Below downloads all at once. Dont do this to show progress in the foreach loop below
			#$downloader.Updates = $updatesToDownload
			#$downloader.Download()
			
            $count = 0
			foreach($update in $updatesToDownload){
                $count++
				Write-Progress -Activity 'Downloading' -Status 'Downloading Windows Updates' -CurrentOperation "Downloading $($update.Title)" -PercentComplete ($count/$updatesToDownload.Count *100)  -Id 2
				$indivUpdate = New-object -com "Microsoft.Update.UpdateColl"
				$indivUpdate.Add($update) | Out-Null
				$downloader.Updates = $indivUpdate
				$downloader.Download() | Out-Null
			}
		}
		Write-Progress -Activity 'Downloading' -Status 'Done' -Completed  -Id 2
		Write-Progress -Activity 'Windows Updates' -Status 'Performing Windows Updates' -CurrentOperation "Installing" -PercentComplete ($progress += 45)  -Id 0
		
		# install updates
		$reboot = $False
		$updatesToInstall = New-object -com "Microsoft.Update.UpdateColl"
		foreach($update in $updatesToDownload){
			if ($update.IsDownloaded){
				$updatesToInstall.Add($update) | Out-Null
				if ($update.InstallationBehavior.RebootBehavior -gt 0){
					$reboot = $True	
				}
			}
		}
		
		if ($updatesToInstall.Count -eq 0){
			Write-Progress -Activity 'Windows Updates' -Status 'Done' -Completed  -Id 0
            $richTextBox.AppendText("Done!`n")
			[Environment]::Exit($SUCCESS)
		}
		
		# Perform the installation
		$installer = $updateSession.CreateUpdateInstaller()
	    #$installer.Updates = $updatesToInstall
		#$installationResult = $installer.Install()
		
		$count = 0
		foreach($update in $updatesToInstall){
			$count++
			Write-Progress -Activity 'Installing' -Status 'Installing Windows Updates' -CurrentOperation "Installing $($update.Title)" -PercentComplete ($count/$updatesToInstall.Count * 100) -Id 3
			$indivUpdate = New-Object -com "Microsoft.Update.UpdateColl"
			$indivUpdate.Add($update) | Out-Null
			$installer.Updates = $indivUpdate
			$installer.Install() | Out-Null
		}
		Write-Progress -Activity 'Installing' -Status 'Done' -Completed  -Id 3
		Write-Progress -Activity 'Windows Updates' -Status 'Done' -Completed  -Id 0
		
		#$Global:counter=-1
	    #$installer.updates | Format-Table -autosize -property Title,EulaAccepted,@{label='Result';
	    #expression={$ResultCode[$installationResult.GetUpdateResult($Global:Counter++).resultCode ]}} 
	
		if ($reboot){
            [Environment]::Exit($REBOOT)
		}

	}
    $ErrorActionPreference = $old_ErrorActionPreference 
}

if($CCleaner){
	if([System.IntPtr]::Size -eq 8){
		$exe = "C:\Program Files\CCleaner\CCleaner64.exe"
		$args = "/Auto"
		$proc = (Start-Process $exe $args -PassThru)
		$proc | Wait-Process
	}
	else{
		$exe = "C:\Program Files\CCleaner\CCleaner.exe"
		$args = "/Auto"
		$proc = (Start-Process $exe $args -PassThru)
		$proc | Wait-Process
	}
}