for %%i in (%~dp0..\..\Proto\*.proto) do (  
	%~dp0protoc.exe --proto_path=%~dp0..\..\Proto\ --csharp_out=%~dp0..\..\..\Demo\Assets\Scripts\HotFix\Net\Proto %%i
)

pause