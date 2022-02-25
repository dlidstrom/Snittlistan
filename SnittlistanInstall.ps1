# Right click and choose "Run with PowerShell"

param($ScriptDirectory)

trap {
    $_
    Read-Host
    exit 1
}

function Get-ScriptDirectory {
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    if ($Invocation.PSScriptRoot)
    {
        $Invocation.PSScriptRoot
    }
    ElseIf ($Invocation.MyCommand.Path)
    {
        Split-Path $Invocation.MyCommand.Path
    }
    Else
    {
        $Invocation.InvocationName.Substring(0, $Invocation.InvocationName.LastIndexOf("\"))
    }
}

# run as admin
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator))
{
    $arguments = "& '" + $MyInvocation.MyCommand.Definition + "' " + (Get-ScriptDirectory)
    Start-Process powershell -Verb runAs -ArgumentList $arguments
    Break
}

$winver = [System.Environment]::OSVersion.Version
if ($winver.Major -eq 6 -and $winver.Minor -ge 3) {
    # perform check for Windows Server 2012 R2 (Get-WindowsFeature is not available otherwise)
    $feature = Get-WindowsFeature NET-WCF-HTTP-Activation45
    if (-not $feature.Installed) {
        Write-Host -Fore Red "HTTP Activation needs to be installed."
        Read-Host
        break
    }
}

if (-not (Test-Path ..\service-account.txt)) {
    Read-Host -AsSecureString "Enter Service account" | ConvertFrom-SecureString | Out-File ..\service-account.txt
}

$serviceAccount = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\service-account.txt))))

if (-not (Test-Path ..\service-password.txt)) {
    Read-Host -AsSecureString "Enter Service password" | ConvertFrom-SecureString | Out-File ..\service-password.txt
}

$servicePassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\service-password.txt))))

if (-not (Test-Path ..\email-password.txt)) {
    Read-Host -AsSecureString "Enter Email password" | ConvertFrom-SecureString | Out-File ..\email-password.txt
}

$emailPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\email-password.txt))))

if (-not (Test-Path ..\db-host.txt)) {
    Read-Host -AsSecureString "Enter Database host" | ConvertFrom-SecureString | Out-File ..\db-host.txt
}

$dbHost = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\db-host.txt))))

if (-not (Test-Path ..\db-name.txt)) {
    Read-Host -AsSecureString "Enter Database name" | ConvertFrom-SecureString | Out-File ..\db-name.txt
}

$dbName = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\db-name.txt))))

if (-not (Test-Path ..\db-name2.txt)) {
    Read-Host -AsSecureString "Enter Second Database name" | ConvertFrom-SecureString | Out-File ..\db-name2.txt
}

$dbName2 = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\db-name2.txt))))

if (-not (Test-Path ..\db-username.txt)) {
    Read-Host -AsSecureString "Enter Database username" | ConvertFrom-SecureString | Out-File ..\db-username.txt
}

$dbUsername = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\db-username.txt))))

if (-not (Test-Path ..\db-password.txt)) {
    Read-Host -AsSecureString "Enter Database password" | ConvertFrom-SecureString | Out-File ..\db-password.txt
}

$dbPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto(
    [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR(
        (ConvertTo-SecureString (Get-Content ..\db-password.txt))))

$settings = @{
    SERVICE_ACCOUNT = $serviceAccount
    SERVICE_PASSWORD = $servicePassword
    EMAIL_PASSWORD = $emailPassword
    DB_HOST = $dbHost
    DB_NAME = $dbName
    DB_NAME2 = $dbName2
    DB_USERNAME = $dbUsername
    DB_PASSWORD = $dbPassword
}

$settingsFormatted = ($settings.Keys | % { "$_=$($settings[$_])" }) -join "`n"
"Settings:`n$settingsFormatted"

"Installing new version..."
$settingsJoined = ($settings.Keys | % { "$_=""$($settings[$_])""" }) -join " "

$pinfo = New-Object System.Diagnostics.ProcessStartInfo
$pinfo.WorkingDirectory = $ScriptDirectory
$pinfo.FileName = "msiexec.exe"
$pinfo.RedirectStandardError = $true
$pinfo.RedirectStandardOutput = $true
$pinfo.UseShellExecute = $false
$pinfo.Arguments = "/l* Snittlistan_install.log /i Snittlistan.msi $settingsJoined"
$p = New-Object System.Diagnostics.Process
$p.StartInfo = $pinfo
$p.Start() | Out-Null
$stdout = $p.StandardOutput.ReadToEnd()
$p.WaitForExit()
