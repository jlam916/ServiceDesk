[CmdletBinding()]
param(
	[Parameter(Mandatory=$True)]
	[string] $ComputerName
)

netdom.exe Remove $ComputerName /Domain:Itservices.Network /UserD:AddComputer /PasswordD:Ciwmb2010

if($LASTEXITCODE -eq 0){
	Return 0
}
else{
	Return 1
}