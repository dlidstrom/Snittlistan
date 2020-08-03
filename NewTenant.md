# Ny subdomän

* Skapa databasen
* Ny Url reservation: netsh http add urlacl url=http://___.snittlistan.se:61026/ user=everyone
* Lägg till domänen i C:\Windows\System32\drivers\etc\hosts
* Ny binding i .vs\Snittlistan\config\applicationhost.config
* Uppdatera SiteWideConfig
* Kör Snittlistan.Tool.exe Initialize ... ...
* Kör Snittlistan.Tool.exe GetPlayersFromBits
* Kör Snittlistan.Tool.exe GetRostersFromBits
* Kör Snittlistan.Tool.exe RegisterMatches

* Fixa bilderna med https://realfavicongenerator.net
    - Skapa bilderna i Snittlistan.Web\Content\css\images

* Lägg till A record för domänen i Netcetera Control Panel. Ska peka på 81.27.111.95
* Uppdatera bindings i Snittlistan.main.wxs
* Installera ny version
* Kör LEWS med alla domännamn. Från C:\Users\Administrator\Downloads\letsencrypt-win-simple.v1.9.7.0-beta10
    N: Create New Certificate
    2: SAN certificate for all bindings of an IIS site
    1: Snittlistan
    6: [http-01] Self-host verification files (port 80 will be unavailable during validation)
* Lägg till webbplatsövervakning i site24x7 med en timmes intervall.

# Uppdatera certifikaten
* Kör LEWS från C:\Users\Administrator\Downloads\letsencrypt-win-simple.v1.9.7.0-beta10
    A: Renew all
