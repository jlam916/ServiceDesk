@echo off
set ComputerName=%1

if [%1]==[] (
	:: Get computer name
	start eventvwr.exe
) else (
	start eventvwr.exe %ComputerName%
)