set GEN_CLIENT=dotnet DLL\Luban.ClientServer\Luban.ClientServer.dll

%GEN_CLIENT% -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir ..\..\..\project\Excel ^
 --output_data_dir ..\..\Unity\Assets\Resources\output_json ^
 --output_code_dir ..\..\Unity\Assets\Scripts\HotFix\Data ^
 --gen_types code_cs_unity_json,data_json ^
 -s all
pause