#!/bin/bash
# https://sean.thenewells.us/f-sql-provider-in-net-core-3-1/

# Run a restore to be sure we have all the dlls on the system.
# dotnet restore > /dev/null

# Find nuget's global directory
nugetRootPath=$(dotnet nuget locals global-packages -l | cut -d' ' -f2)
nugetRootPath="${nugetRootPath%?}"

# array of ${nameOfDep}|${pathToDllFolder}
packagesAndPaths=("System.Runtime.CompilerServices.Unsafe|4.6.0/lib/netstandard2.0")

mkdir -p lib

for i in "${packagesAndPaths[@]}"
do
  # all these vars split out for clarity
  name=$(echo "$i" | cut -d'|' -f1)
  path=$(echo "$i" | cut -d'|' -f2)
  nugetDll="$name.dll"
  pathFromNugetRoot="$(echo "$name" | awk '{print tolower($0)}')/$path"
  fullPath="$nugetRootPath/$pathFromNugetRoot/$nugetDll"

  cp "$fullPath" "./lib/$nugetDll"
  echo " $fullPath > Copied $nugetDll into lib for type provider."
done
