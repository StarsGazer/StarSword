@echo off

set in_path=%cd%/Protos
set out_path=%cd%/Protos
set tool_path=%cd%/ProtoGen/protogen
 
rem �����ļ�
for /R %in_path% %%i in (*.proto) do echo %%~ni     
for /R %in_path% %%i in (*.proto) do %tool_path% -i:%%i -o:%out_path%/%%~ni.cs    
pause