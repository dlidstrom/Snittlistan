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

"Installing new version..."

$pinfo = New-Object System.Diagnostics.ProcessStartInfo
$pinfo.WorkingDirectory = $ScriptDirectory
$pinfo.FileName = "msiexec.exe"
$pinfo.RedirectStandardError = $true
$pinfo.RedirectStandardOutput = $true
$pinfo.UseShellExecute = $false
$pinfo.Arguments = "/l* Snittlistan_install.log /i Snittlistan.msi"
$p = New-Object System.Diagnostics.Process
$p.StartInfo = $pinfo
$p.Start() | Out-Null
$p.WaitForExit()
$stdout = $p.StandardOutput.ReadToEnd()