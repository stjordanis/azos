@echo on

SET PROJECT_HOME=%AZIST_HOME%
SET LAST=%PROJECT_HOME:~-1%
IF %LAST% NEQ \ (SET PROJECT_HOME=%PROJECT_HOME%\)

set AZOS_HOME=%PROJECT_HOME%AZOS\
set OUT=%AZOS_HOME%out\nuget

set VER=1.0.205

nuget pack Azos.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.Web.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.Wave.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.Media.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.MongoDb.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.MySql.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.MsSql.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.Oracle.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.Sky.nuspec -Version %VER% -OutputDirectory "%OUT%"
nuget pack Azos.Sky.MongoDb.nuspec -Version %VER% -OutputDirectory "%OUT%"

 nuget push "%OUT%\Azos.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.Web.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.Wave.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.Media.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.MongoDb.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.MySql.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.MsSql.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.Oracle.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.Sky.%VER%.nupkg" %AZIST_NUGET_API_KEY% 
 nuget push "%OUT%\Azos.Sky.MongoDb.%VER%.nupkg" %AZIST_NUGET_API_KEY% 