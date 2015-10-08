param(
	[Parameter(Mandatory = $true)]
	[string] $UninstallString
)
$old_ErrorActionPreference = $ErrorActionPreference
$ErrorActionPreference = 'SilentlyContinue'

#Note: some uninstall strings start with ". Remove it.
if($UninstallString.StartsWith("`"")){
    $UninstallString = $UninstallString.replace("`"", "")
}
        
# Could be an Msiexec uninstall string or filepath
if($UninstallString.StartsWith("M")){
    $UninstallString = $UninstallString.replace("/I", "/X")
    $args = $UninstallString.Substring("MSiexec.exe ".Length, $UninstallString.Length - "MSiexec.exe ".Length)
    Start -FilePath "Msiexec.exe" -ArgumentList "$args /passive" -wait
}
else{
    Start -FilePath "$uninstall" -wait
}

$ErrorActionPreference = $old_ErrorActionPreference 