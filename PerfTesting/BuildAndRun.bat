@echo off

echo setting up build environment
call "%VS140COMNTOOLS%\VsDevCmd.bat"

cd "%~p0"

echo building code
msbuild /t:Clean;Rebuild /p:Configuration=Release

echo deleting old test results
del SampleResults1.txt SampleResults2.txt SampleResults3.txt

echo running test 1
".\PerfTesting\bin\Release\PerfTesting.exe" >SampleResults1.txt

echo running test 2
".\PerfTesting\bin\Release\PerfTesting.exe" >SampleResults2.txt

echo running test 3
".\PerfTesting\bin\Release\PerfTesting.exe" >SampleResults3.txt

echo tests complete
pause