if [%1]==[] (
	Exit 1
)

netdom.exe RenameComputer %ComputerName% /NewName:%1 /Force /Reboot:0