@echo off
set ComputerName=%1

if [%1]==[] (
	:: Get computer name
	start msra.exe /offerRA 
) else (
	start msra.exe /offerRA %ComputerName%
)