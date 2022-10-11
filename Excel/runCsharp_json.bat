set GEN_CLIENT=dotnet ..\ExcelTools_Luban\DLL\Luban.ClientServer\Luban.ClientServer.dll

%GEN_CLIENT% -j cfg --^
 -d ..\ExcelTools_Luban\Defines\__root__.xml ^
 --input_data_dir ..\Excel ^
 --output_data_dir ..\Demo\Assets\Resources\ExcelData_json ^
 --output_code_dir ..\Demo\Assets\Scripts\HotFix\Data ^
 --gen_types code_cs_unity_json,data_json ^
 -s all
pause