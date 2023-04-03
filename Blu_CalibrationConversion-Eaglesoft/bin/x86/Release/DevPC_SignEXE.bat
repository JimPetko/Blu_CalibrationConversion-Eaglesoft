"C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" sign /v /a /ac "%~dp0\r3csr45cross2020.cer" /tr http://sha256timestamp.ws.symantec.com/sha256/timestamp /td SHA256 /s MY /n "DIGITAL DOC LLC" "%~dp0\*.exe"
pause
"C:\Program Files (x86)\Microsoft SDKs\ClickOnce\SignTool\signtool.exe" verify /v /kp *.exe
pause
