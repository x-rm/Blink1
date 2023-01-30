mkdir "C:\Program Files\Status Monitor" 2> NUL
mkdir "C:\Program Files\Status Monitor\Logs" 2> NUL

net stop "StatusMonitor"

xcopy .\bin\Debug\StatusMonitorService.exe.config  "C:\program files\Status Monitor"
xcopy /exclude:excludes.txt /Y .\bin\Debug\*.*  "C:\Program Files\Status Monitor" 

C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u "C:\program files\Status Monitor\StatusMonitorService.exe"

C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe "C:\program files\Status Monitor\StatusMonitorService.exe"
