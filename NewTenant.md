# Ny subdomän

* Lägg till A record för domänen i Netcetera Control Panel. Ska peka på 81.27.111.95
* Ny connectionString i Snittlistan.Tool.exe.config och Snittlistan.Web.config
* Skapa databasen
* Kör Snittlistan.Tool.exe /initialize
* Ny TenantConfiguration i Global.asax.cs
* Uppdatera bindings i Snittlistan.main.wxs
* Installera ny version
* Kör LEWS med alla domännamn. Från C:\Install\letsencrypt-win-simple.V1.9.3
  letsencrypt.exe --notaskscheduler --webroot "C:\Program Files\Snittlistan" --accepttos --emailaddress dlidstrom@gmail.com --manualhost snittlistan.se,vartansik.snittlistan.se
* Lägg till webbplatsövervakning i site24x7 med en timmes intervall.
