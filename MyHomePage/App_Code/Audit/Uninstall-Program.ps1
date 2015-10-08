[cmdletbinding()]            

param (    
 [parameter(ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
 [string]$computerName,
 [parameter(ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
 [string]$appGUID,
 [parameter(ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]
 [string]$uninstallString
)    
# working .\Uninstall-Program.ps1 -uninstallString """C:\Program Files (x86)\Google\Chrome\Application\34.0.1847.137\Installer\setup.exe"" --uninstall --multi-install --chrome --system-level --verbose-logging""" -computerName Test
$return = @{}

try {
    if($uninstallString){
        $returnval = ([WMICLASS]"\\$computerName\ROOT\CIMV2:win32_process").Create("cmd.exe /c ""$uninstallString"" /S")
        # if chrome, kill chrome process to complete uninstall
        if($uninstallString.ToUpper().Contains("CHROME")){
            Start-Sleep -Seconds 2
            $processes = Get-WmiObject -Class Win32_Process -ComputerName $computerName -Filter "name='chrome.exe'"
            foreach ($process in $processes) {
                $returnval = $process.terminate()
            }
        }
    }
    else{
        $returnval = ([WMICLASS]"\\$computerName\ROOT\CIMV2:win32_process").Create("msiexec /x""$AppGUID"" /norestart /qn")
    }
} 
catch {
    write-error "Failed to trigger the uninstallation. Review the error message"
    $_
    $return.Add("ExitCode", 1)
}

$return.Add("ExitCode", 0)

Return $return