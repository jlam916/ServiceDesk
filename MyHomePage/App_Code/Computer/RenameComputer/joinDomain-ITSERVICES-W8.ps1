[CmdletBinding()]
param(
	[Parameter(Mandatory=$True)]
	[string] $ComputerName
)

netdom.exe join $ComputerName /d:itservices.network /userd:AddComputer /passwordd:Ciwmb2010 /ou:OU=W8Systems,OU="DR3 SYSTEMS",DC=itservices,DC=network

if($LASTEXITCODE -eq 0){
	Return 0
}
else{
	Return 1
}