cd ../Cmd
git pull
go build
rmdir ..\Browser\Peernet.Browser.Infrastructure\Tools\CmdSource /s /q
xcopy /s ..\Cmd\ ..\Browser\Peernet.Browser.Infrastructure\Tools\CmdSource\ /e