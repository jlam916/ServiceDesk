﻿[cmdletbinding()]            
param(            
 [parameter(ValueFromPipeline=$true,ValueFromPipelineByPropertyName=$true)]            
 [string[]]$ComputerName = $env:computername            
)  

begin {            
 $UninstallRegKey32="SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall" 
 $UninstallRegKey64="SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall"
 $programs = @()          
} 

process {            
 foreach($Computer in $ComputerName) {            
  Write-Verbose "Working on $Computer"            
  if(Test-Connection -ComputerName $Computer -Count 1 -ea 0) {            
   $HKLM   = [microsoft.win32.registrykey]::OpenRemoteBaseKey('LocalMachine',$computer)            
   $UninstallRef  = $HKLM.OpenSubKey($UninstallRegKey32)            
   $Applications = $UninstallRef.GetSubKeyNames()            

   foreach ($App in $Applications) {            
    $AppRegistryKey  = $UninstallRegKey32 + "\\" + $App            
    $AppDetails   = $HKLM.OpenSubKey($AppRegistryKey)            
    $AppGUID   = $App            
    $AppDisplayName  = $($AppDetails.GetValue("DisplayName"))            
    #$AppVersion   = $($AppDetails.GetValue("DisplayVersion"))            
    $AppPublisher  = $($AppDetails.GetValue("Publisher"))            
    $AppInstalledDate = $($AppDetails.GetValue("InstallDate"))            
    $AppUninstall  = $($AppDetails.GetValue("UninstallString"))  
              
    if(!$AppDisplayName) { continue }            
        $OutputObj = New-Object -TypeName PSobject             
        #$OutputObj | Add-Member -MemberType NoteProperty -Name ComputerName -Value $Computer.ToUpper()            
        $OutputObj | Add-Member -MemberType NoteProperty -Name AppName -Value $AppDisplayName            
        #$OutputObj | Add-Member -MemberType NoteProperty -Name AppVersion -Value $AppVersion            
        $OutputObj | Add-Member -MemberType NoteProperty -Name AppVendor -Value $AppPublisher            
        $OutputObj | Add-Member -MemberType NoteProperty -Name InstalledDate -Value $AppInstalledDate            
        $OutputObj | Add-Member -MemberType NoteProperty -Name UninstallKey -Value $AppUninstall            
        $OutputObj | Add-Member -MemberType NoteProperty -Name AppGUID -Value $AppGUID            
        $programs += $OutputObj# | Select ComputerName, DriveName            
   } 
   
   # 64 bit applications    
   $UninstallRef  = $HKLM.OpenSubKey($UninstallRegKey64)            
   $Applications = $UninstallRef.GetSubKeyNames()            

   foreach ($App in $Applications) {            
    $AppRegistryKey  = $UninstallRegKey64 + "\\" + $App            
    $AppDetails   = $HKLM.OpenSubKey($AppRegistryKey)            
    $AppGUID   = $App            
    $AppDisplayName  = $($AppDetails.GetValue("DisplayName"))            
    #$AppVersion   = $($AppDetails.GetValue("DisplayVersion"))            
    $AppPublisher  = $($AppDetails.GetValue("Publisher"))            
    $AppInstalledDate = $($AppDetails.GetValue("InstallDate"))            
    $AppUninstall  = $($AppDetails.GetValue("UninstallString"))  
              
    if(!$AppDisplayName) { continue }            
        $OutputObj = New-Object -TypeName PSobject             
        #$OutputObj | Add-Member -MemberType NoteProperty -Name ComputerName -Value $Computer.ToUpper()            
        $OutputObj | Add-Member -MemberType NoteProperty -Name AppName -Value $AppDisplayName            
        #$OutputObj | Add-Member -MemberType NoteProperty -Name AppVersion -Value $AppVersion            
        $OutputObj | Add-Member -MemberType NoteProperty -Name AppVendor -Value $AppPublisher            
        $OutputObj | Add-Member -MemberType NoteProperty -Name InstalledDate -Value $AppInstalledDate            
        $OutputObj | Add-Member -MemberType NoteProperty -Name UninstallKey -Value $AppUninstall            
        $OutputObj | Add-Member -MemberType NoteProperty -Name AppGUID -Value $AppGUID            
        $programs += $OutputObj# | Select ComputerName, DriveName            
   }           
  }            
 }            
}            

end {Return $programs}